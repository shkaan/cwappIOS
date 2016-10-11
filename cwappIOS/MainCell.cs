using Foundation;
using System;
using UIKit;

namespace cwappIOS
{
    public partial class MainCell : UITableViewCell
    {
        public MainCell(IntPtr handle) : base(handle)
        {
        }

        public void UpdateCell(string question, string answer)
        {
            qLabel.Text = "Q: " + question;
            aLabel.Text = "A: " + answer;
        }
    }


}