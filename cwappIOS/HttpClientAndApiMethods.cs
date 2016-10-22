using ModernHttpClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using UIKit;

namespace cwappIOS
{
    class HttpClientAndApiMethods : HttpClient
    {
        private string token = new StoreCredentialsToKeychain().token;
        public static event EventHandler OnLoginSuccess;
        public HttpClient httpClient;
        //public MainTableModel mainTableModel;

        public HttpClientAndApiMethods()
        {
            //constructor with default settings for all routes

            httpClient = new HttpClient(new NativeMessageHandler());

            httpClient.BaseAddress = new Uri("http://192.168.1.101:3000/" /*"http://www.ukrstenica.com"*/);
            //httpClient.Timeout = new TimeSpan(0,0,5);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (token != null)
            {
                httpClient.DefaultRequestHeaders.Add("x-access-token", token);
            }

        }

        //main table data getter -- userentries route takes 2 arguments, offset and limit

        public MainTableModel GetApiData()
        {
            string queryString = GetQueryString("0", "100");

            var apiResponse = httpClient.GetAsync("api/userEntries" + "?" + queryString).Result;//"?token=" + _token.token);

            MainTableModel initialData = new MainTableModel();

            if (apiResponse.IsSuccessStatusCode)
            {
                var responseContent = apiResponse.Content;
                string jsonString = responseContent.ReadAsStringAsync().Result;
                return initialData = JsonConvert.DeserializeObject<MainTableModel>(jsonString);
            }
            initialData.success = false;
            initialData.message = "Server error";
            return initialData;
        }

        public async Task<MainTableModel> GetApiData(int offset, int limit)
        {
            string queryString = GetQueryString(offset.ToString(), limit.ToString());

            var apiResponse = await httpClient.GetAsync("api/userEntries" + "?" + queryString);//"?token=" + _token.token);

            MainTableModel newData = new MainTableModel();

            if (apiResponse.IsSuccessStatusCode)
            {
                var responseContent = apiResponse.Content;
                string jsonString = await responseContent.ReadAsStringAsync();
                return newData = JsonConvert.DeserializeObject<MainTableModel>(jsonString);
            }
            newData.success = false;
            newData.message = "Server error";
            return newData;

        }


        private string GetQueryString(string offsetValue, string limitValue)
        {
            NameValueCollection queryString = HttpUtility.ParseQueryString(string.Empty);

            queryString["offset"] = offsetValue;
            queryString["limit"] = limitValue;
            //queryString["token"] = token;

            return queryString.ToString();
        }


        //if change detected to question or answer fields send those rows to editrow route, confirm they have been saved and return updated data to view

        public MainTableModel SendEditedFields(SendFields dataToSend, int rowId)
        {
            var jsonData = new StringContent(JsonConvert.SerializeObject(dataToSend).ToString(), Encoding.UTF8, "application/json");

            var response = httpClient.PostAsync("api/editRow/" + rowId, jsonData).Result;

            MainTableModel updatedData = new MainTableModel();

            if (response.IsSuccessStatusCode)
            {
                var apiResponse = response.Content.ReadAsStringAsync().Result;
                updatedData = JsonConvert.DeserializeObject<MainTableModel>(apiResponse);
                return updatedData;

            }
            updatedData.success = false;
            updatedData.message = "Server Error";
            return updatedData;

        }

        //send row marked for deletition to deleterow route and confirm it's been deleted

        public bool DeleteRow(int id)
        {
            //Console.WriteLine("row id is: " + id);

            var apiResponse = httpClient.DeleteAsync("api/deleteRow/" + id).Result;

            if (apiResponse.IsSuccessStatusCode)
            {
                var responseContent = apiResponse.Content.ReadAsStringAsync().Result;
                var convertedData = JsonConvert.DeserializeObject<MainTableModel>(responseContent);
                return convertedData.success;
            }
            return false;
        }

        //check if token exists and it's validity before fireing log in or main view

        public bool CheckIfTokenIsStoredAndValid()
        {
            if (token != null)
            {
                try
                {
                    var response = httpClient.GetAsync("api/verifyToken").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = response.Content;
                        string jsonString = responseContent.ReadAsStringAsync().Result;
                        var apiResponse = JsonConvert.DeserializeObject<MainTableModel>(jsonString);
                        return apiResponse.success;
                    }
                    else
                    {
                        new UIAlertView("Error", "Server Error", null, "Back", null).Show();
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


        //Log in calls and data:

        public List<KeyValuePair<string, string>> GetLogInFields(string username, string password)
        {
            return new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password),
            };
        }

        public async void SubmitLogInData(string username, string password)
        {
            List<KeyValuePair<string, string>> body = GetLogInFields(username, password);

            var content = new FormUrlEncodedContent(body);

            HttpResponseMessage response = await httpClient.PostAsync("api/auth", content);

            if (response.IsSuccessStatusCode)
            {
                HttpContent responseContent = response.Content;
                string jsonString = await responseContent.ReadAsStringAsync().ConfigureAwait(false);
                MainTableModel apiResponse = JsonConvert.DeserializeObject<MainTableModel>(jsonString);
                VerifyCredentials(username, apiResponse);
            }
        }

        private void VerifyCredentials(string username, MainTableModel jsonResponse)
        {
            if (jsonResponse.success == true)
            {
                //We have successfully authenticated a the user,
                //Now store credentials to device and fire login success event 

                StoreCredentialsToKeychain saveCreds = new StoreCredentialsToKeychain();
                saveCreds.SaveCredentials(username, jsonResponse.token);
                OnLoginSuccess?.Invoke(jsonResponse, new EventArgs());

                //if (OnLoginSuccess != null)
                //    OnLoginSuccess(jsonResponse, EventArgs.Empty);
            }
            else
            {
                new UIAlertView("Log In Error", jsonResponse.message, null, "OK", null).Show();
            }
        }



    }
}
