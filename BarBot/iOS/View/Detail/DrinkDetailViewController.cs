using Foundation;
using System;
using UIKit;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;

namespace BarBot.iOS.View.Detail
{
	public class DrinkDetailViewController : UIViewController
    {
        public DrinkDetailViewController()
        {
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			Title = "Drink Detail";
		}
    }
}