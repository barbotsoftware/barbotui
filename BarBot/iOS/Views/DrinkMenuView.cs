using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using BarBot.Core.ViewModels;
using UIKit;
using CoreGraphics;

namespace BarBot.iOS.Views
{
	public partial class DrinkMenuView : MvxViewController<DrinkMenuViewModel>
	{
		public DrinkMenuView() : base("DrinkMenuView", null)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			Title = "DRINK MENU";
			NavBarStyle(NavigationController.NavigationBar);
			NavigationItem.BackBarButtonItem = new UIBarButtonItem("Back", UIBarButtonItemStyle.Plain, null);
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}

		void NavBarStyle(UINavigationBar NavBar)
		{
			NavBar.BarTintColor = Color.BackgroundGray;
			NavBar.TitleTextAttributes = new UIStringAttributes
			{
				ForegroundColor = UIColor.White,
				Font = UIFont.FromName("Microsoft-Yi-Baiti", 26f)
			};
			var NavBorder = new UIView(new CGRect(0,
												  NavBar.Frame.Size.Height - 1,
												  NavBar.Frame.Size.Width,
												  4));
			NavBorder.BackgroundColor = Color.BarBotBlue;
			NavBorder.Opaque = true;
			NavBar.AddSubview(NavBorder);
		}
	}
}

