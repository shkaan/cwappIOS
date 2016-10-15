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

    }
}
