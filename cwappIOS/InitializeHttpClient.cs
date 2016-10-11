using ModernHttpClient;
using System;
using System.Net.Http;
using System.Net.Http.Headers;


namespace cwappIOS
{
    internal class InitializeHttpClient
    {
        //private HttpClient httpClient;

        //public InitializeHttpClient(HttpClient httpClient)
        //{
        //    this.httpClient = httpClient;
        //}

        public HttpClient GetHttpClient()
        {
            var httpClient = new HttpClient(new NativeMessageHandler());

            httpClient.BaseAddress = new Uri("http://192.168.1.101:3000/");
            //httpClient.Timeout = new TimeSpan(0,0,5);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return httpClient;

        }

        
    }
}
