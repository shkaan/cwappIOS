using Foundation;
using ModernHttpClient;
using Newtonsoft.Json;
using Security;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using UIKit;

namespace cwappIOS
{
    public partial class LogInViewController : UIViewController
    {

        public event EventHandler OnLoginSuccess;

        public LogInViewController(IntPtr handle) : base(handle)
        {
        }

        async partial void SubmitButton_TouchUpInside(UIButton sender)
        {
            //Validate Username & Password.

            List<KeyValuePair<string, string>> body = GetLogInFields();
            var content = new FormUrlEncodedContent(body);

            HttpClientApiMethods httpClient = new HttpClientApiMethods();
            //HttpClient httpClient = GetHttpClient();
            var response = await httpClient.GetHttpClient().PostAsync("api/auth", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content;
                string jsonString = await responseContent.ReadAsStringAsync().ConfigureAwait(false);
                MainTableModel apiResponse = JsonConvert.DeserializeObject<MainTableModel>(jsonString);
                VerifyCredentials(apiResponse);
            }


        }

        private List<KeyValuePair<string, string>> GetLogInFields()
        {
            return new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("username", usenameField.Text),
                new KeyValuePair<string, string>("password", passwordField.Text),
            };
        }

        private void VerifyCredentials(MainTableModel jsonResponse)
        {
            if (jsonResponse.success == true)
            {
                //We have successfully authenticated a the user,
                //Now store credentials to device and fire login success event 

                StoreCredentialsToKeychain saveCreds = new StoreCredentialsToKeychain();
                saveCreds.SaveCredentials("testUser", jsonResponse.token);
                OnLoginSuccess?.Invoke(jsonResponse, new EventArgs());
            }
            else
            {
                new UIAlertView("Log In Error", jsonResponse.message, null, "OK", null).Show();
            }
        }

        //private HttpClient GetHttpClient()
        //{
        //    var httpClient = new HttpClient(new NativeMessageHandler());

        //    httpClient.BaseAddress = new Uri("http://192.168.1.101:3000/");
        //    httpClient.DefaultRequestHeaders.Accept.Clear();
        //    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //    return httpClient;

        //}

        //void StoreKeysInKeychain(string key, string value)
        //{
        //    var s = new SecRecord(SecKind.GenericPassword)
        //    {
        //        ValueData = NSData.FromString(value),
        //        Generic = NSData.FromString(key)
        //    };

        //    var err = SecKeyChain.Add(s);
        //}


    }
}