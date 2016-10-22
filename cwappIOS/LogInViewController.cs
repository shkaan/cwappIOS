using System;
using UIKit;

namespace cwappIOS
{
    public partial class LogInViewController : UIViewController
    {

        //public event EventHandler OnLoginSuccess;

        public LogInViewController(IntPtr handle) : base(handle)
        {
        }

        partial void SubmitButton_TouchUpInside(UIButton sender)
        {
            //Validate Username & Password.
            HttpClientAndApiMethods httpClient = new HttpClientAndApiMethods();

            httpClient.SubmitLogInData(usenameField.Text, passwordField.Text);

            //List<KeyValuePair<string, string>> body = httpClient.GetLogInFields(usenameField.Text, passwordField.Text);
            //var content = new FormUrlEncodedContent(body);

            //HttpClient httpClient = GetHttpClient();
            //HttpResponseMessage response = await httpClient.PostAsync("api/auth", content);

            //if (response.IsSuccessStatusCode)
            //{
            //    HttpContent responseContent = response.Content;
            //    string jsonString = await responseContent.ReadAsStringAsync().ConfigureAwait(false);
            //    MainTableModel apiResponse = JsonConvert.DeserializeObject<MainTableModel>(jsonString);
            //    VerifyCredentials(apiResponse);
            //}
        }

        //public override void ViewDidLoad()
        //{
        //    base.ViewDidLoad();
        //}

        //private List<KeyValuePair<string, string>> GetLogInFields()
        //{
        //    return new List<KeyValuePair<string, string>>
        //    {
        //        new KeyValuePair<string, string>("username", usenameField.Text),
        //        new KeyValuePair<string, string>("password", passwordField.Text),
        //    };
        //}

        //private void VerifyCredentials(MainTableModel jsonResponse)
        //{
        //    if (jsonResponse.success == true)
        //    {
        //        //We have successfully authenticated a the user,
        //        //Now store credentials to device and fire login success event 

        //        StoreCredentialsToKeychain saveCreds = new StoreCredentialsToKeychain();
        //        saveCreds.SaveCredentials("testUser", jsonResponse.token);
        //        OnLoginSuccess?.Invoke(jsonResponse, new EventArgs());
        //    }
        //    else
        //    {
        //        new UIAlertView("Log In Error", jsonResponse.message, null, "OK", null).Show();
        //    }
        //}

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