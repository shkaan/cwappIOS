using System;
using System.Collections.Generic;
using System.Text;
using Foundation;
using UIKit;

namespace cwappIOS
{
    class MainTableSource : UITableViewSource
    {
        public NSIndexPath currentInexPath { get; set; }

        List<ModelFields> tableData;

        public List<ModelFields> TableData
        {
            get
            {
                return tableData;
            }

            set
            {
                tableData = value;
            }
        }

        public static event EventHandler<ModelFields> RowClicked = delegate { };
        public static event EventHandler<ModelFields> DeleteRowClicked = delegate { };
        public static bool successIndicator = false;

        public MainTableSource(MainTableModel items)
        {
            TableData = items.apiData;
        }



        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = (MainCell)tableView.DequeueReusableCell(MainTableController.cellIdentifier, indexPath);

            cell.UpdateCell(TableData[indexPath.Row].question, TableData[indexPath.Row].answer);

            return cell;
        }


        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            currentInexPath = indexPath;
            RowClicked(this, TableData[indexPath.Row]);
            tableView.DeselectRow(indexPath, true);
        }

        //public void UpdateModel(UpdatedDataModel updatedData)
        //{
        //    var forUpdate = updatedData.apiData;

        //    TableData[currentInexPath.Row] = updatedData.apiData;


        //}


        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return TableData.Count;
        }


        public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, Foundation.NSIndexPath indexPath)
        {
            switch (editingStyle)
            {
                case UITableViewCellEditingStyle.Delete:

                    //Console.WriteLine("id is {0}", tableData[indexPath.Row].entryid);
                    DeleteRowClicked(this, TableData[indexPath.Row]);
                    if (successIndicator == true)
                    {
                        TableData.RemoveAt(indexPath.Row);
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
