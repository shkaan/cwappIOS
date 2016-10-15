using CoreGraphics;
using System;
using UIKit;

namespace cwappIOS
{
    class ModalBackground : UIView

    {
        UIView modal;
        UILabel editLabel;
        public UITextField qField;
        public UITextField aField;
        public UIButton submitButton;
        public UIButton cancelButton;

        public ModalBackground(CGRect frame) : base(frame)
        {
            BackgroundColor = UIColor.Black.ColorWithAlpha(0.7f);
            AutoresizingMask = UIViewAutoresizing.All;

            nfloat centerX = Frame.Width / 2;
            nfloat centerY = Frame.Height / 2;

            nfloat modalWidth = 260;
            nfloat modalHeight = 200;


            //modal window

            modal = new UIView();
            modal.Frame = new CGRect(
                centerX - (modalWidth / 2),
                centerY - (modalHeight / 2) - 50,
                modalWidth,
                modalHeight);
            modal.AutoresizingMask = UIViewAutoresizing.All;
            modal.BackgroundColor = UIColor.White;
            modal.Layer.CornerRadius = 5;
            AddSubview(modal);


            //modal elements

            nfloat labelHeight = 22;
            nfloat textFieldHeight = labelHeight + 4;
            nfloat buttonHeight = labelHeight + 2;


            //header label

            editLabel = new UILabel(new CGRect(
                10f,//centerX - (labelWidth / 2),
                10f,//centerY + 20,
                modalWidth - 20,//labelWidth,
                labelHeight
                ));
            editLabel.BackgroundColor = UIColor.Clear;
            editLabel.TextColor = UIColor.Black;
            editLabel.Text = "Edit Fields";
            editLabel.Font = UIFont.FromName("Helvetica", 20f);
            editLabel.TextAlignment = UITextAlignment.Center;
            editLabel.AutoresizingMask = UIViewAutoresizing.All;
            modal.AddSubview(editLabel);


            //question input field

            qField = new UITextField(new CGRect(
                10f, //horizontal position
                10f + labelHeight + 28f, //vertical position
                modalWidth - 20f,
                textFieldHeight
                ));
            qField.AutoresizingMask = UIViewAutoresizing.All;
            qField.AutocorrectionType = UITextAutocorrectionType.No;
            qField.SpellCheckingType = UITextSpellCheckingType.No;
            qField.Font = UIFont.FromName("Helvetica", 14f);
            qField.AdjustsFontSizeToFitWidth = true;
            qField.BorderStyle = UITextBorderStyle.RoundedRect;
            qField.BecomeFirstResponder();
            modal.AddSubview(qField);
            //qField.Layer.BorderWidth = 0.5f;
            //qField.Layer.BorderColor = UIColor.Blue.CGColor;


            //answer input field

            aField = new UITextField(new CGRect(
                10f,
                40f + (labelHeight * 2) + 14,
                modalWidth - 20f,
                textFieldHeight
                ));
            aField.AutoresizingMask = UIViewAutoresizing.All;
            aField.AutocorrectionType = UITextAutocorrectionType.No;
            aField.SpellCheckingType = UITextSpellCheckingType.No;
            aField.Font = UIFont.FromName("Helvetica", 14f);
            aField.AdjustsFontSizeToFitWidth = true;
            aField.BorderStyle = UITextBorderStyle.RoundedRect;
            modal.AddSubview(aField);


            //submit button

            submitButton = new UIButton(UIButtonType.System);
            submitButton.Frame = new CGRect(
                40f,
                80f + (labelHeight * 2) + 16,
                modalWidth - 80f,
                buttonHeight
                );
            submitButton.SetTitle("Submit", UIControlState.Normal);
            submitButton.AutoresizingMask = UIViewAutoresizing.All;
            modal.AddSubview(submitButton);


            //cancel button

            cancelButton = new UIButton(UIButtonType.System);
            cancelButton.Frame = new CGRect(
                40f,
                80f + (labelHeight * 3) + 20f,
                modalWidth - 80f,
                buttonHeight
                );
            cancelButton.SetTitle("Cancel", UIControlState.Normal);
            cancelButton.AutoresizingMask = UIViewAutoresizing.All;
            modal.AddSubview(cancelButton);

        }

        public void Hide()
        {
            UIView.Animate(
                0.1, // duration
                () => { Alpha = 0; },
                () => { RemoveFromSuperview(); }
            );
        }
    }
}
