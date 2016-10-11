// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace cwappIOS
{
    [Register ("MainCell")]
    partial class MainCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel aLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel qLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (aLabel != null) {
                aLabel.Dispose ();
                aLabel = null;
            }

            if (qLabel != null) {
                qLabel.Dispose ();
                qLabel = null;
            }
        }
    }
}