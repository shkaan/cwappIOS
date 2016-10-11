﻿using Foundation;
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
        //private UITableView table;
        public static MainTableModel mainTableModel;
        private InitializeHttpClient httpClient;
        private StoreCredentialsToKeychain _token;

        private string[] testTable = new string[] { "kurazz", "palazz", "kriminalazz" };




        public TestTableController(IntPtr handle) : base(handle)
        {
            //TableView.RegisterClassForCellReuse(typeof(MainCell), cellIdentifier);
            //table.RegisterClassForCellReuse(typeof(MainCell), cellIdentifier);

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            GetApiData();
            MainTableView.Source = new MainTableSource(mainTableModel);


            //MainTableView.Source = new MainTableSource(mainTableModel);

            //MainTableView.WeakDelegate = this;

            //MainTableView.Source = new MainTableSource(mainTableModel);

            //this.MainTableView.DataSource = new MainTableSource();

            //Add(table);

            //ProcessData();

            //string[] dataArray = new string[] { "kurazz", "palazz", "stojadin", "miladin", "milojca" };

        }

        //public override void ViewWillAppear(bool animated)
        //{
        //    base.ViewWillAppear(animated);
        //    MainTableView.Source = new MainTableSource(mainTableModel);
        //}

        //public override void ViewDidLayoutSubviews()
        //{
        //    base.ViewDidLayoutSubviews();
        //}



        //public override void ViewDidAppear(bool animated)
        //{
        //    base.ViewDidAppear(animated);

        //    //table.Source = new MainTableSource(mainTableModel);
        //    //Add(TableView);
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