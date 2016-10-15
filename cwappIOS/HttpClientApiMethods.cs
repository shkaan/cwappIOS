using ModernHttpClient;
using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace cwappIOS
{


    public class HttpClientApiMethods
    {
        private string token = new StoreCredentialsToKeychain().token;

        public HttpClient GetHttpClient()
        {
            var httpClient = new HttpClient(new NativeMessageHandler());

            httpClient.BaseAddress = new Uri("http://192.168.1.101:3000/");
            //httpClient.Timeout = new TimeSpan(0,0,5);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (token != null)
            {
                httpClient.DefaultRequestHeaders.Add("x-access-token", token);
            }
            return httpClient;
        }



        public bool CheckIfTokenIsStoredAndValid()
        {
            if (token != null)
            {
                try
                {
                    var response = GetHttpClient().GetAsync("api/verifyToken").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = response.Content;
                        string jsonString = responseContent.ReadAsStringAsync().Result;
                        var apiResponse = JsonConvert.DeserializeObject<MainTableModel>(jsonString);
                        return apiResponse.success;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (HttpRequestException)
                {
                    return false;
                }

            }
            else return false;
        }

    }


}
