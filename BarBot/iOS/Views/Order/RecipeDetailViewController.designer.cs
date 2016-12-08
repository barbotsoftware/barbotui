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

namespace BarBot.iOS.View.Order
{
    [Register ("RecipeDetailViewController")]
    partial class RecipeDetailViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView DrinkImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView IngredientTable { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton OrderButton { get; set; }

        [Action ("OrderButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OrderButton_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (DrinkImage != null) {
                DrinkImage.Dispose ();
                DrinkImage = null;
            }

            if (IngredientTable != null) {
                IngredientTable.Dispose ();
                IngredientTable = null;
            }

            if (OrderButton != null) {
                OrderButton.Dispose ();
                OrderButton = null;
            }
        }
    }
}