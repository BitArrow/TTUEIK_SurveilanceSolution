using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using UWP.DTO;
using UWP.Services.Interfaces;

namespace UWP.Services
{
    public class APIBaseService
    {
        private readonly HttpClient _httpClient;
        private readonly IB2CService _b2c;

        public APIBaseService()
        {
            _b2c = App.Container.GetRequiredService<IB2CService>();

            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(App.WebApiEndpoint);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.BearerToken);
        }

        public async Task<T> GetAsync<T>(string requestUri)
        {
            try
            {
                await ValidateToken();
                return await ReadContentAsync<T>(await _httpClient.GetAsync(requestUri));
            }
            catch (Exception ex)
            {
                Debug.Write("APIService: " + ex.ToString());
                //throw new HttpRequestException(ex.ToString());
                return default(T);
            }
        }

        public async Task<T> PostAsync<T>(string requestUri, object body)
        {
            try
            {
                await ValidateToken();
                var json = JsonConvert.SerializeObject(body);
                var stringBody = new StringContent(json, Encoding.UTF8, "application/json");
                return await ReadContentAsync<T>(await _httpClient.PostAsync(requestUri, stringBody));
            }
            catch (Exception ex)
            {
                Debug.Write("APIService: " + ex.ToString());
                //throw new HttpRequestException(ex.ToString());
                return default(T);
            }
        }

        public async Task<T> PutAsync<T>(string requestUri, object body)
        {
            try
            {
                await ValidateToken();
                var json = JsonConvert.SerializeObject(body);
                var stringBody = new StringContent(json, Encoding.UTF8, "application/json");
                return await ReadContentAsync<T>(await _httpClient.PutAsync(requestUri, stringBody));
            }
            catch (Exception ex)
            {
                Debug.Write("APIService: " + ex.ToString());
                //throw new HttpRequestException(ex.ToString());
                return default(T);
            }
        }

        public async Task DeleteAsync(string requestUri)
        {
            try
            {
                await ValidateToken();
                await _httpClient.DeleteAsync(requestUri);
            }
            catch (Exception ex)
            {
                Debug.Write("APIService: " + ex.ToString());
                //throw new HttpRequestException(ex.ToString());
            }
        }

        private async Task<T> ReadContentAsync<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            try
            {
                return JsonConvert.DeserializeObject<T>(content);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorDto>(content);
                Debug.Write(error.ToString());
                return default(T);
            }
        }

        private async Task ValidateToken()
        {
            if (App.RefreshTokenExpires < DateTime.Now.AddSeconds(30))
            {
                await _b2c.LogOut();
                await _b2c.Login();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.BearerToken);
            }
            else if (App.BearerExpires.AddMinutes(1) < DateTime.Now)
            {
                await _b2c.Refresh();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.BearerToken);
            }
        }

        public async Task<T> GetAsync<T>(Uri requestUri)
        {
            return await GetAsync<T>(requestUri.ToString());
        }

        public async Task<T> PostAsync<T>(Uri requestUri, object body)
        {
            return await PostAsync<T>(requestUri.ToString(), body);
        }

        public async Task<T> PutAsync<T>(Uri requestUri, object body)
        {
            return await PutAsync<T>(requestUri.ToString(), body);
        }

        public async Task DeleteAsync(Uri requestUri)
        {
            await DeleteAsync(requestUri.ToString());
        }

        
    }
}
