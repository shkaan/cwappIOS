using Foundation;
using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Web;
using UIKit;

namespace cwappIOS
{
    public partial class TestTableController : UITableViewController
    {
        public static NSString cellIdentifier = new NSString("MainCell");
        // private UITableView table;
        public static MainTableModel mainTableModel;
        private InitializeHttpClient httpClient;
        private StoreCredentialsToKeychain _token;


        public TestTableController(IntPtr handle) : base(handle)
        {
            TableView.RegisterClassForCellReuse(typeof(MainCell), cellIdentifier);

        }

        public override async void ViewDidLoad()
        {
            base.ViewDidLoad();
            await GetApiData();
            TableView.Source = new MainTableSource(mainTableModel);
            
            //ProcessData();
            //table = new UITableView(View.Bounds);

            //string[] dataArray = new string[] { "kurazz", "palazz", "stojadin", "miladin", "milojca" };

        }



        //public override void ViewDidAppear(bool animated)
        //{
        //    base.ViewDidAppear(animated);

        //    //table.Source = new MainTableSource(mainTableModel);
        //    //Add(TableView);
        //}



        private async Task GetApiData()
        {
            httpClient = new InitializeHttpClient();
            //model = new MainTableModel();
            _token = new StoreCredentialsToKeychain();

            string queryString = GetQueryString("0", "100", _token.token);

            var response = await httpClient.GetHttpClient().GetAsync("api/userEntries" + "?" + queryString);//"?token=" + _token.token);

            if (response.IsSuccessStatusCode)
            {

                var responseContent = response.Content;
                string jsonString = await responseContent.ReadAsStringAsync().ConfigureAwait(false);
                mainTableModel = JsonConvert.DeserializeObject<MainTableModel>(jsonString);
                //ProcessData(mainTableModel);
            }
        }

        private string GetQueryString(string offsetValue, string limitValue, string token)
        {
            NameValueCollection queryString = HttpUtility.ParseQueryString(string.Empty);

            queryString["offset"] = offsetValue;
            queryString["limit"] = limitValue;
            queryString["token"] = token;

            return queryString.ToString();
        }
    }
}