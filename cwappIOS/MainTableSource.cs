using System;
using System.Collections.Generic;
using System.Text;
using Foundation;
using UIKit;

namespace cwappIOS
{
    class MainTableSource : UITableViewSource
    {
        List<ModelFields> tableData;
        public static event EventHandler RowClicked = delegate { };

        public MainTableSource(MainTableModel items)
        {
            tableData = items.apiData;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {

            RowClicked(null, EventArgs.Empty);

            //new UIAlertView("Row selected: ", tableData[indexPath.Row].entryid.ToString(), null, "OK", null).Show();
            tableView.DeselectRow(indexPath, true);
        }


        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = (MainCell)tableView.DequeueReusableCell(TestTableController.cellIdentifier, indexPath);

            //cell.TextLabel.Text = tableData[indexPath.Row].question.ToString();
            cell.UpdateCell(tableData[indexPath.Row].question, tableData[indexPath.Row].answer);

            return cell;
        }

        //public override nint NumberOfSections(UITableView tableView)
        //{
        //    return 1;
        //}


        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return tableData.Count;
        }

        public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, Foundation.NSIndexPath indexPath)
        {
            switch (editingStyle)
            {
                case UITableViewCellEditingStyle.Delete:
                    // remove the item from the underlying data source
                    tableData.RemoveAt(indexPath.Row);
                    // delete the row from the table
                    tableView.DeleteRows(new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Fade);
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
