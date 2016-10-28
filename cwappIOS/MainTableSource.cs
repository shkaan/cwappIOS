using System;
using System.Collections.Generic;
using Foundation;
using UIKit;
using System.Threading.Tasks;
using System.Linq;

namespace cwappIOS
{
    class MainTableSource : UITableViewSource
    {
        public NSIndexPath currentIndexPath { get; set; }
        public List<ModelFields> tableData { get; set; }
        public static event EventHandler<ModelFields> RowClicked = delegate { };
        public static event EventHandler<ModelFields> DeleteRowClicked = delegate { };
        public static bool successIndicator = false;

        //infinite scroll fields
        private readonly HttpClientAndApiMethods httpClient;
        private UITableView _tableView;
        private int offset = 0;
        private int limit = 100;
        private int totalRowCount;
        //private int totalTableCount = 0;
        private bool isFetching;

        public MainTableSource(MainTableModel items, HttpClientAndApiMethods httpClient)
        {
            tableData = items.apiData;
            totalRowCount = items.totalRows;
            this.httpClient = httpClient;
        }


        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            MainCell cell = (MainCell)tableView.DequeueReusableCell(MainTableController.cellIdentifier, indexPath);

            cell.UpdateCell(tableData[indexPath.Row].question, tableData[indexPath.Row].answer);

            int index = indexPath.Row;
            int totalTableCount = tableData.Count;

            if (_tableView == null)
            _tableView = tableView;

            if(!isFetching && index >= totalTableCount * 0.8 && totalRowCount >= 0 && totalRowCount - totalTableCount >= limit)
            {
                isFetching = true;
                Task.Factory.StartNew(LoadMore);

            } else if (!isFetching && index >= totalTableCount * 0.8 && totalRowCount > 0 && totalRowCount - totalTableCount < limit)
            {
                isFetching = true;
                Task.Factory.StartNew(LastCall);
            }
            return cell;
        }


        private async void LoadMore()
        {
            //this.pageIndex++;
            offset = tableData.Count;
            var moreRows = await httpClient.GetApiData(offset, limit);
            tableData.AddRange(moreRows.apiData);
            totalRowCount = tableData.Last().totalRows;
            InvokeOnMainThread(() => _tableView.ReloadData());
            isFetching = false;
        }


        private async void LastCall()
        {
            limit = totalRowCount - tableData.Count;
            offset = tableData.Count;
            var moreRows = await httpClient.GetApiData(offset, limit);
            tableData.AddRange(moreRows.apiData);
            totalRowCount = tableData.Last().totalRows;
            InvokeOnMainThread(() => _tableView.ReloadData());
        }


        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return tableData.Count;
        }


        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            currentIndexPath = indexPath;
            RowClicked(this, tableData[indexPath.Row]);
            tableView.DeselectRow(indexPath, true);
        }


        public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
        {
            switch (editingStyle)
            {
                case UITableViewCellEditingStyle.Delete:

                    //Console.WriteLine("id is {0}", tableData[indexPath.Row].entryid);
                    DeleteRowClicked(this, tableData[indexPath.Row]);
                    if (successIndicator == true)
                    {
                        tableData.RemoveAt(indexPath.Row);
                        tableView.DeleteRows(new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Fade);
                    }
                    else
                    {
                        new UIAlertView("Error", "Can not delete selected row.", null, "Ok", null).Show();
                    }
                    break;
                case UITableViewCellEditingStyle.None:
                    Console.WriteLine("CommitEditingStyle:None called");
                    break;
            }
        }


        public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
        {
            return true; // return false if you wish to disable editing for a specific indexPath or for all rows
        }


        public override bool CanMoveRow(UITableView tableView, NSIndexPath indexPath)
        {
            return false; // return true to enable row moving
        }

        //public override string TitleForDeleteConfirmation(UITableView tableView, NSIndexPath indexPath)
        //{   // Optional - default text is 'Delete'
        //    return "Trash (" + tableData[indexPath.Row].question + ")";
        //}

    }
}
