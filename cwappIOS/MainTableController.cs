using CoreAnimation;
using CoreGraphics;
using Foundation;
using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using UIKit;

namespace cwappIOS
{
    public partial class MainTableController : UITableViewController
    {
        public static NSString cellIdentifier = new NSString("MainCell");
        public static MainTableModel mainTableModel;
        private HttpClient httpClient = new HttpClientApiMethods().GetHttpClient();
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

            GetApiData();

            MainTableView.Source = new MainTableSource(mainTableModel);

        }


        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            MainTableSource.RowClicked += (object sender, ModelFields data) =>
            {
                MainTableView.ScrollEnabled = false;

                var bounds = View.Bounds;
                rowData = data as ModelFields;
                activeSourceInstance = sender as MainTableSource;

                modal = new ModalBackground(bounds);
                modal.qField.Text = rowData.question;
                modal.aField.Text = rowData.answer;
                View.AddSubview(modal);

                modal.submitButton.TouchUpInside += SubmitEditedFields;
                modal.cancelButton.TouchUpInside += cancelEditing;
            };

            MainTableSource.DeleteRowClicked += (object sender, ModelFields data) =>
            {

                bool deleted = DeleteRow(data.entryid);

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
            };
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
                var jsonData = new StringContent(JsonConvert.SerializeObject(sendData).ToString(), Encoding.UTF8, "application/json");


                var response = httpClient.PostAsync("api/editRow/" + rowData.entryid, jsonData).Result;

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = response.Content.ReadAsStringAsync().Result;
                    var updatedData = JsonConvert.DeserializeObject<MainTableModel>(apiResponse);

                    if (updatedData.success == true)
                    {
                        modal.Hide();
                        activeSourceInstance.TableData[activeSourceInstance.currentInexPath.Row] = updatedData.apiData[0];
                        MainTableView.ReloadData();
                        MainTableView.ScrollEnabled = true;
                    }
                    else
                    {
                        new UIAlertView("Error", updatedData.message, null, "Back", null).Show();
                    }


                }
            }
        }



        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }



        private void GetApiData()
        {
            string queryString = GetQueryString("0", "100");

            var apiResponse = httpClient.GetAsync("api/userEntries" + "?" + queryString).Result;//"?token=" + _token.token);

            if (apiResponse.IsSuccessStatusCode)
            {
                var responseContent = apiResponse.Content;
                string jsonString = responseContent.ReadAsStringAsync().Result;
                mainTableModel = JsonConvert.DeserializeObject<MainTableModel>(jsonString);
            }
        }


        private void cancelEditing(object sender, EventArgs e)
        {
            modal.Hide();
            MainTableView.ScrollEnabled = true;
        }



        private bool DeleteRow(int id)
        {
            //Console.WriteLine("row id is: " + id);

            var apiResponse = httpClient.DeleteAsync("api/deleteRow/" + id).Result;

            if (apiResponse.IsSuccessStatusCode)
            {
                var responseContent = apiResponse.Content.ReadAsStringAsync().Result;
                var convertedData = JsonConvert.DeserializeObject<MainTableModel>((string)responseContent);
                return convertedData.success;
            }
            return false;
        }


        private string GetQueryString(string offsetValue, string limitValue)
        {
            NameValueCollection queryString = HttpUtility.ParseQueryString(string.Empty);

            queryString["offset"] = offsetValue;
            queryString["limit"] = limitValue;
            //queryString["token"] = token;

            return queryString.ToString();
        }
    }
}