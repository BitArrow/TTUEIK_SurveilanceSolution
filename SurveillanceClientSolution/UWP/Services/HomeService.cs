using Newtonsoft.Json;
using Restup.Webserver.Attributes;
using Restup.Webserver.Http;
using Restup.Webserver.Models.Contracts;
using Restup.Webserver.Models.Schemas;
using Restup.Webserver.Rest;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UWP.Services.Interfaces;

namespace UWP.Services
{
    public class HomeService : APIBaseService, IHomeService
    {
        public async Task SendTest()
        {
            try
            {
                var httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri("http://192.168.0.11:8800");
                var result = await httpClient.GetAsync("api/" + Guid.NewGuid().ToString());
                var content = await result.Content.ReadAsStringAsync();
                var returned = JsonConvert.DeserializeObject<DataReturned>(content);
                for (int i = 0; i < 20000000; i++)
                {
                    await httpClient.GetAsync("api/" + Guid.NewGuid().ToString());
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async Task StartServer()
        {
            try
            {
                App.RegisterRestup();
                await App.HttpServer.StartServerAsync();
            }
            catch (Exception ex)
            { }
        }

        public async Task StopServer()
        {
            App.HttpServer.StopServer();
        }
    }

    class DataReturned
    {
        public DateTime Time { get; set; }
        public string Code { get; set; }
    }

    
}
