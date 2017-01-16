using UIKit;
using CoreGraphics;

namespace BarBot.iOS.Style
{
	public static class SharedStyles
	{
		static UIColor WHITE = UIColor.White;
		static UIFont MSYIBAITI = UIFont.FromName("Microsoft-Yi-Baiti", 26f);

		// Style Navigation Bar
		public static void NavBarStyle(UINavigationBar NavBar)
		{
			NavBar.TintColor = WHITE;
			NavBar.BarTintColor = Color.NavBarGray;
			NavBar.TitleTextAttributes = new UIStringAttributes
			{
				ForegroundColor = WHITE,
				Font = MSYIBAITI
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
			Cell.TextLabel.TextColor = WHITE;
			Cell.TextLabel.Font = MSYIBAITI;
		}

		public static void StyleButtonText(UIButton Button, int textSize)
		{
			Button.TitleLabel.Font = UIFont.FromName("Microsoft-Yi-Baiti", textSize);
			Button.SetTitleColor(WHITE, UIControlState.Normal);
		}

		public static void StylePickerView(UIPickerView Picker)
		{
			Picker.BackgroundColor = Color.BackgroundGray;
		}
	}
}
