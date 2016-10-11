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
        //private string cellIdentifier = "MainCell";
        


        public MainTableSource(MainTableModel items)
        {
            tableData = items.apiData;

        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            var cellTag = tableView.Tag;
            new UIAlertView("Row selected: ", tableData[indexPath.Row].entryid.ToString(), null, "OK", null).Show();
            tableView.DeselectRow(indexPath, true);

        }


        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            //tableView.RegisterClassForCellReuse(typeof(MainCell), new NSString(cellIdentifier));
            //MainCell cell = tableView.DequeueReusableCell(cellIdentifier) as MainCell;
            var cell = (MainCell)tableView.DequeueReusableCell(TestTableController.cellIdentifier, indexPath);
            
            
            
            //var cell = tableView.DequeueReusableCell(MainCell.Key, indexPath);
            
            //string question = tableData[indexPath.Row].question;

            //if (cell == null)
            //{
                //cell = new MainCell(new NSString(cellIdentifier));
                //cell = new MainCell(cellIdentifier);
                //cell = new MainCell(new NSString(cellIdentifier));

                //cell.TextLabel.Text = tableData[indexPath.Row].question.ToString();
            //}

            //cell.UpdateCell(tableData[indexPath.Row].question, tableData[indexPath.Row].answer);
            cell.TextLabel.Text = tableData[indexPath.Row].question.ToString();
            //cell.UpdateCell(tableData[indexPath.Row].question, tableData[indexPath.Row].answer);


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

        //public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
        //{
        //    return true;
        //}

    }
}
