using System;
using UIKit;
using CoreGraphics;

namespace BarBot.iOS
{
	public partial class BarBotNavigationController : UINavigationController
	{
		public BarBotNavigationController(UIViewController controller) : base(controller)
		{
		}

		public BarBotNavigationController(IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			NavBarStyle();
		}

		void NavBarStyle()
		{
			NavigationBar.TintColor = UIColor.White;
			NavigationBar.BarTintColor = Color.BackgroundGray;
			var NavBorder = new UIView(new CGRect(0,
												  NavigationBar.Frame.Size.Height - 1,
												  NavigationBar.Frame.Size.Width,
												  4));
			NavBorder.BackgroundColor = Color.BarBotBlue;
			NavBorder.Opaque = true;
			NavigationBar.AddSubview(NavBorder);
			NavigationBar.TitleTextAttributes = new UIStringAttributes
			{
				ForegroundColor = UIColor.White,
				Font = UIFont.FromName("Microsoft-Yi-Baiti", 26f)
			};
		}

		public override UIStatusBarStyle PreferredStatusBarStyle()
		{
			return UIStatusBarStyle.LightContent;
		}
	}
}
