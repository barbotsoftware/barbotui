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
			// Perform any additional setup after loading the view, typically from a nib.
			//NavBarStyle();
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}

		//void NavBarStyle()
		//{
		//	NavigationBar.TintColor = UIColor.White;
		//	NavigationBar.BarTintColor = Color.BackgroundGray;
		//	var NavBorder = new UIView(new CGRect(0,
		//										  NavigationBar.Frame.Size.Height - 1,
		//										  NavigationBar.Frame.Size.Width,
		//										  4));
		//	NavBorder.BackgroundColor = Color.BarBotBlue;
		//	NavBorder.Opaque = true;
		//	NavigationBar.AddSubview(NavBorder);
		//	NavigationBar.TitleTextAttributes = new UIStringAttributes
		//	{
		//		ForegroundColor = UIColor.White,
		//		Font = UIFont.FromName("Microsoft-Yi-Baiti", 26f)
		//	};
		//}
	}
}

