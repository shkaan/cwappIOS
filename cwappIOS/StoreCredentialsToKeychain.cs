using Foundation;
using Security;
using System.Linq;
using Xamarin.Auth;


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
                AccountStore.Create().Save(account, "crossWordsDB");
            }
        }

        public string UserName
        {
            get
            {
                var account = AccountStore.Create().FindAccountsForService("crossWordsDB").FirstOrDefault();
                return (account != null) ? account.Username : null;
            }
        }

        public string token
        {
            get
            {
                var account = AccountStore.Create().FindAccountsForService("crossWordsDB").FirstOrDefault();
                return (account != null) ? account.Properties["token"] : null;
            }
        }


    }
}
