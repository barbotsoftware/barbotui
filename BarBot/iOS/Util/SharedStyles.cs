using UIKit;
using CoreGraphics;

namespace BarBot.iOS.Util
{
	public static class SharedStyles
	{
		// Style Navigation Bar
		public static void NavBarStyle(UINavigationBar NavBar)
		{
			NavBar.TintColor = UIColor.White;
			NavBar.BarTintColor = Color.NavBarGray;
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
