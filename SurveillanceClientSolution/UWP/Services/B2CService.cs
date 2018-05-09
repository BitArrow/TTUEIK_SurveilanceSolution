using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
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
using Windows.Security.Authentication.Web;

namespace UWP.Services
{
    public class B2CService : IB2CService
    {
        private readonly IPasswordVaultService _vaultService;
        private readonly HttpClient _httpClient;

        public B2CService()
        {
            _vaultService = App.Container.GetRequiredService<IPasswordVaultService>();

            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(App.BaseHost);
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
        }

        public async Task Login()
        {
            string queryString =
                $"&client_id={App.ClientId}" +
                $"&response_type={App.ResponseType}" +
                $"&redirect_uri={Uri.EscapeDataString(App.ApplicationRedirect)}" +
                $"&response_mode={App.ResponseModeQuery}" +
                $"&scope={Uri.EscapeDataString(App.OfflineScope)}" +
                $"&state={App.StateData}";

            var startURI = new Uri(App.BaseHost + App.AuthorizePath + queryString);
            var endURI = new Uri(App.ApplicationRedirect);

            try
            {
                WebAuthenticationResult webAuthenticationResult =
                  await WebAuthenticationBroker
                        .AuthenticateAsync(WebAuthenticationOptions.None,
                                            startURI, endURI);

                string result;
                switch (webAuthenticationResult.ResponseStatus)
                {
                    case WebAuthenticationStatus.Success:
                        result = webAuthenticationResult.ResponseData.ToString();
                        await ContinueOauthFlow(result);
                        await ValidateLoginAsync();
                        break;
                    case WebAuthenticationStatus.ErrorHttp:
                        result = webAuthenticationResult.ResponseErrorDetail.ToString();
                        break;
                    case WebAuthenticationStatus.UserCancel:
                        result = "User canceled login";
                        break;
                    default:
                        result = webAuthenticationResult.ResponseData.ToString();
                        break;
                }
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
            }
        }

        private async Task ValidateLoginAsync()
        {
            var localHttpClient = new HttpClient();
            localHttpClient.BaseAddress = new Uri(App.WebApiEndpoint);
            localHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.BearerToken);
            await localHttpClient.GetAsync("user/validate");
        }

        public async Task Refresh()
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
                  new KeyValuePair<string, string>("grant_type", "refresh_token"),
                  new KeyValuePair<string, string>("client_id", App.ClientId),
                  new KeyValuePair<string, string>("refresh_token", App.RefreshToken)
             });

            var response = await _httpClient.PostAsync(App.TokenPath, formContent);
            var result = await response.Content.ReadAsStringAsync();

            GetTokenValues(result);
        }

        public async Task LogOut()
        {
            _vaultService.RemoveExistingTokens();
        }

        private async Task ContinueOauthFlow(string callback)
        {
            var queryCollection = ParseQueryString(callback);
            var code = queryCollection["code"];
            await GetToken(code);
        }

        private Dictionary<string, string> ParseQueryString(string requestQueryString)
        {
            Dictionary<string, string> rc = new Dictionary<string, string>();
            string[] ar1 = requestQueryString.Split(new char[] { '&', '?' });
            foreach (string row in ar1)
            {
                if (string.IsNullOrEmpty(row)) continue;
                int index = row.IndexOf('=');
                if (index > -1)
                    rc[Uri.UnescapeDataString(row.Substring(0, index))] = Uri.UnescapeDataString(row.Substring(index + 1)); // use Unescape only parts          
            }
            return rc;
        }

        private async Task GetToken(string code)
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
                  new KeyValuePair<string, string>("grant_type", "authorization_code"),
                  new KeyValuePair<string, string>("client_id", App.ClientId),
                  new KeyValuePair<string, string>("code", code),
                  new KeyValuePair<string, string>("redirect_uri", App.ApplicationRedirect)
             });

            var response = await _httpClient.PostAsync(App.TokenPath, formContent);
            var result = await response.Content.ReadAsStringAsync();
            
            GetTokenValues(result);
        }

        private void GetTokenValues(string authResult)
        {
            if (string.IsNullOrEmpty(authResult))
                return;
            try
            {
                Debug.WriteLine(authResult);
                dynamic data = JObject.Parse(authResult);
                App.BearerToken = data.access_token;
                App.BearerExpires = DateTime.Now + new TimeSpan(0, 0, (int)data.expires_in);
                App.RefreshToken = data.refresh_token;
                App.RefreshTokenExpires = DateTime.Now + new TimeSpan(0, 0, (int)data.refresh_token_expires_in);

                _vaultService.SaveTokens();
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
            }
        }
    }
}
