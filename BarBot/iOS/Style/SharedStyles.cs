using System;

using UIKit;
using CoreGraphics;

namespace BarBot.iOS.Style
{
	public static class SharedStyles
	{
        const int TEXT_SIZE = 28;
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

        // UIButton

        public static void StyleUIButton(UIButton Button)
        {
			StyleButtonText(Button, TEXT_SIZE);
			Button.BackgroundColor = Color.BarBotBlue;
			Button.Layer.CornerRadius = new nfloat(2.0);
			Button.Layer.BorderWidth = new nfloat(0.9);
			Button.Layer.BorderColor = Color.BarBotBlue.CGColor;
			var insets = new UIEdgeInsets(new nfloat(2.0), new nfloat(2.0), new nfloat(2.0),
								 new nfloat(2.0));
			Button.TitleEdgeInsets = insets;
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
