using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using HRGeoLocator.Models;
using Microsoft.Extensions.Configuration;

namespace HRGeoLocator.Services.Interface
{
    internal class WebCamService : IWebCamService
    {
        private readonly String _apiKeyValue = String.Empty;
        private readonly static HttpClient _client = new HttpClient();
        private readonly static string _ServiceURL = @"https://webcamstravel.p.rapidapi.com/webcams/list/nearby={0},{1},{2}?show=webcams%3Aimage%2Clocation%2Cplayer";

        private WebCamService()
        {
            //Dummy for DI.
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public WebCamService(IConfiguration config)
        {
            if (config != null)
            {
                _apiKeyValue = config[WebCamConstant.API_KEY_SECRET];
            }
        }
        public async Task<WebCamsTravelRootObject> GetWebCamsNearAsync(float wgs84_lat, float wgs84_lon, float radiusInKilometers)
        {
            if (_client == null)
            {
                throw new ArgumentNullException("_client");
            }
            _client.DefaultRequestHeaders.Add(WebCamConstant.API_KEY, _apiKeyValue);
            String query = String.Empty;
            try
            {
                query = String.Format(_ServiceURL, wgs84_lat.ToString().Replace(",", "."), wgs84_lon.ToString().Replace(",", "."), radiusInKilometers.ToString().Replace(",", "."));
            }
            catch(Exception ex)
            {
                int i = 42;
                //TODO
            }
            using (var streamTask = _client.GetAsync(query))
            {
                await streamTask;
                if (streamTask.IsCompletedSuccessfully)
                {
                    using (var taskJson = streamTask.Result.Content.ReadAsStringAsync())
                    {
                        await taskJson;
                        var geoname = JsonSerializer.Deserialize<WebCamsTravelRootObject>(taskJson.Result);
                        return geoname;
                    }
                }
                else
                {
                    throw new Exception("Can not get result from : " + _ServiceURL);
                }

            }
        }
    }
}
