using Foundation;
using System;
using UIKit;

namespace cwappIOS
{
    public partial class MainTableController : UITableViewController
    {
        public static NSString cellIdentifier = new NSString("MainCell");
        //private MainTableModel mainTableModel;
        //private HttpClient httpClient = new HttpClientApiMethods().GetHttpClient();
        private HttpClientAndApiMethods httpClient = new HttpClientAndApiMethods();
        private ModelFields rowData;
        private MainTableSource activeSourceInstance;
        private ModalBackground modal;


        public MainTableController(IntPtr handle) : base(handle)
        {
            //TableView.RegisterClassForCellReuse(typeof(MainCell), cellIdentifier);

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            MainTableModel inititalData = httpClient.GetApiData();

            MainTableView.Source = new MainTableSource(inititalData, httpClient);
            //GetApiData();

            //MainTableView.Source = new MainTableSource(mainTableModel);
        }


        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
        }

        public override void ViewDidAppear(bool animated)
        {

            base.ViewDidAppear(animated);
            MainTableSource.RowClicked -= OnRowClicked;
            MainTableSource.RowClicked += OnRowClicked;
            MainTableSource.DeleteRowClicked -= OnDeleteRowClicked;
            MainTableSource.DeleteRowClicked += OnDeleteRowClicked;
        }

        private void OnRowClicked(object sender, ModelFields e)
        {
            MainTableView.ScrollEnabled = false;

            var bounds = View.Bounds;
            rowData = e as ModelFields;
            activeSourceInstance = sender as MainTableSource;

            modal = new ModalBackground(bounds);
            modal.qField.Text = rowData.question;
            modal.aField.Text = rowData.answer;
            View.AddSubview(modal);

            modal.submitButton.TouchUpInside += SubmitEditedFields;
            modal.cancelButton.TouchUpInside += cancelEditing;
        }

        private void OnDeleteRowClicked(object sender, ModelFields e)
        {
            bool deleted = httpClient.DeleteRow(e.entryid);

            if (deleted == true)
            {
                MainTableSource.successIndicator = true;
                Console.WriteLine("ITS TRUE!");
            }
            else
            {
                MainTableSource.successIndicator = false;
                Console.WriteLine("Bedak");
            }
        }

        

        private void SubmitEditedFields(object sender, EventArgs e)
        {
            if (modal.qField.Text == rowData.question && modal.aField.Text == rowData.answer)
            {
                Console.WriteLine("nothing changed!");
                new UIAlertView("Warning", "You didn't make any changes", null, "Back", null).Show();
            }
            else
            {
                var sendData = new SendFields();
                sendData.question = modal.qField.Text;
                sendData.answer = modal.aField.Text;

                MainTableModel result = httpClient.SendEditedFields(sendData, rowData.entryid);

                if (result.success == true)
                {
                    modal.Hide();
                    activeSourceInstance.tableData[activeSourceInstance.currentIndexPath.Row] = result.apiData[0];
                    MainTableView.ReloadData();
                    MainTableView.ScrollEnabled = true;
                }
                else
                {
                    new UIAlertView("Error", result.message, null, "Back", null).Show();
                }

            }
        }


        private void cancelEditing(object sender, EventArgs e)
        {
            modal.Hide();
            MainTableView.ScrollEnabled = true;
        }


        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }
    }





    //private void GetApiData()
    //{
    //    string queryString = GetQueryString("0", "100");

    //    var apiResponse = httpClient.GetAsync("api/userEntries" + "?" + queryString).Result;//"?token=" + _token.token);

    //    if (apiResponse.IsSuccessStatusCode)
    //    {
    //        var responseContent = apiResponse.Content;
    //        string jsonString = responseContent.ReadAsStringAsync().Result;
    //        mainTableModel = JsonConvert.DeserializeObject<MainTableModel>(jsonString);
    //    }
    //}





    //private bool DeleteRow(int id)
    //{
    //    //Console.WriteLine("row id is: " + id);

    //    var apiResponse = httpClient.DeleteAsync("api/deleteRow/" + id).Result;

    //    if (apiResponse.IsSuccessStatusCode)
    //    {
    //        var responseContent = apiResponse.Content.ReadAsStringAsync().Result;
    //        var convertedData = JsonConvert.DeserializeObject<MainTableModel>(responseContent);
    //        return convertedData.success;
    //    }
    //    return false;
    //}


    //private string GetQueryString(string offsetValue, string limitValue)
    //{
    //    NameValueCollection queryString = HttpUtility.ParseQueryString(string.Empty);

    //    queryString["offset"] = offsetValue;
    //    queryString["limit"] = limitValue;
    //    //queryString["token"] = token;

    //    return queryString.ToString();
    //}
}
