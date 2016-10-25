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
            var firstAttributes = new UIStringAttributes
            {
                ForegroundColor = UIColor.FromRGB(0, 8, 120),
                BackgroundColor = UIColor.Clear,
                //Font = UIFont.FromName("Courier", 18f)
                Font = UIFont.BoldSystemFontOfSize(14)

            };

            var questionString = new NSMutableAttributedString(" Q:  " + question);
            var answerString = new NSMutableAttributedString(" A:  " + answer);

            questionString.SetAttributes(firstAttributes.Dictionary, new NSRange(0, 2));
            answerString.SetAttributes(firstAttributes.Dictionary, new NSRange(0, 2));

            qLabel.AttributedText = questionString;
            aLabel.AttributedText = answerString;
        }
    }


}