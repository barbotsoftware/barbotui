using UIKit;
using CoreAnimation;
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

		// Style TableViewCell
		public static void StyleCell(UITableViewCell Cell)
		{
			Cell.BackgroundColor = Color.BackgroundGray;
			Cell.TextLabel.TextColor = UIColor.White;
			Cell.TextLabel.Font = UIFont.FromName("Microsoft-Yi-Baiti", 26f);
		}
	}
}
