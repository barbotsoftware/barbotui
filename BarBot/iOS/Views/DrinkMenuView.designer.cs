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

namespace BarBot.iOS.Views
{
    [Register ("DrinkMenuView")]
    partial class DrinkMenuView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UICollectionView DrinkMenuCollectionView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UINavigationBar NavigationBar { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (DrinkMenuCollectionView != null) {
                DrinkMenuCollectionView.Dispose ();
                DrinkMenuCollectionView = null;
            }

            if (NavigationBar != null) {
                NavigationBar.Dispose ();
                NavigationBar = null;
            }
        }
    }
}