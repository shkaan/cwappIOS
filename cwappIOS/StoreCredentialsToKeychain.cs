using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;
using System.Net.Http.Headers;
using System.Net.Http;


namespace cwappIOS
{
    public class StoreCredentialsToKeychain
    {
        public void SaveCredentials(string userName, string token)
        {
            if (!string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(token))
            {
                Account account = new Account
                {
                    Username = userName
                };
                account.Properties.Add("token", token);
                AccountStore.Create().Save(account, "cwappIOS");
            }
        }

        public string UserName
        {
            get
            {
                var account = AccountStore.Create().FindAccountsForService("cwappIOS").FirstOrDefault();
                return (account != null) ? account.Username : null;
            }
        }

        public string token
        {
            get
            {
                var account = AccountStore.Create().FindAccountsForService("cwappIOS").FirstOrDefault();
                return (account != null) ? account.Properties["token"] : null;
            }
        }

        public bool CheckIfTokenIsStoredAndValid(string token)
        {
            if (token != null)
            {
                InitializeHttpClient httpClient = new InitializeHttpClient();
                try
                {
                    var response = httpClient.GetHttpClient().GetAsync("api/verifyToken" + "?token=" + token).Result;
                    //response.EnsureSuccessStatusCode();
                    if (response.IsSuccessStatusCode)
                    {
                    var responseContent = response.Content;
                        string jsonString = responseContent.ReadAsStringAsync().Result;
                        SignInJsonResponse apiResponse = JsonConvert.DeserializeObject<SignInJsonResponse>(jsonString);
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
