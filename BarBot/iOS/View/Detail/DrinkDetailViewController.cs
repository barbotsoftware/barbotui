using Foundation;
using System;
using UIKit;
using BarBot.Core.ViewModel;

namespace BarBot.iOS.View.Detail
{
	public class DrinkDetailViewController : UIViewController
    {
		private DetailViewModel ViewModel => Application.Locator.Detail;
        
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