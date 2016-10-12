using CoreGraphics;
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
        public static MainTableModel mainTableModel;
        private InitializeHttpClient httpClient;
        private StoreCredentialsToKeychain _token;


        public TestTableController(IntPtr handle) : base(handle)
        {
            //TableView.RegisterClassForCellReuse(typeof(MainCell), cellIdentifier);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            GetApiData();

            MainTableView.Source = new MainTableSource(mainTableModel);

        }




        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            MainTableSource.RowClicked += (object sender, EventArgs e) =>
            {
                //var sampleView = new UIView() as ModalView;
                //var button = new UIButton();

                //button.Frame = new CGRect(100f, 100f, 80f, 50f);
                //button.SetTitle("Title", UIControlState.Normal);
                //button.BackgroundColor = UIColor.Green;
                //button.Alpha = 1;
                ////sampleView.Frame = new CoreGraphics.CGRect(100f, 100f, 200f, 300f);
                //sampleView.BackgroundColor = UIColor.Black;
                //sampleView.Alpha = 0.5f;
                //MainTableView.ScrollEnabled = false;
                //this.View.AddSubview(sampleView);
                //sampleView.AddSubview(button);
               // View.AddSubview(sampleView);
            };
        }

                

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }

        //public override void ViewWillAppear(bool animated)
        //{
        //    base.ViewWillAppear(animated);
        //}

        //public override void ViewDidLayoutSubviews()
        //{
        //    base.ViewDidLayoutSubviews();
        //}



        //public override void ViewDidAppear(bool animated)
        //{
        //    base.ViewDidAppear(animated);
        //}



        //private async Task GetApiData()
        private void GetApiData()
        {
            httpClient = new InitializeHttpClient();
            //model = new MainTableModel();
            _token = new StoreCredentialsToKeychain();

            string queryString = GetQueryString("0", "100", _token.token);

            var response = httpClient.GetHttpClient().GetAsync("api/userEntries" + "?" + queryString).Result;//"?token=" + _token.token);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content;
                string jsonString = responseContent.ReadAsStringAsync().Result;
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