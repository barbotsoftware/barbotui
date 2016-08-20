using System;
using CoreGraphics;
using UIKit;

namespace BarBot.iOS
{
	public class BaseViewController : UIViewController
	{
		protected StyleUtil styles;

		public BaseViewController()
		{
			styles = new StyleUtil();
		}

		protected UIButton createUIButton(string title, nfloat x, nfloat y, nfloat width)
		{
			var button = UIButton.FromType(UIButtonType.System);
			button.Frame = new CGRect(x, y, width, 40);
			button.SetTitle(title, UIControlState.Normal);
			button.SetTitleColor(UIColor.White, UIControlState.Normal);
			button.BackgroundColor = styles.BarBotBlue;
			button.Layer.CornerRadius = new nfloat(5.0);
			button.Layer.BorderWidth = new nfloat(0.9);
			button.Layer.BorderColor = styles.BarBotBlue.CGColor;
			var insets = new UIEdgeInsets(new nfloat(2.0), new nfloat(2.0), new nfloat(2.0),
								 new nfloat(2.0));
			button.TitleEdgeInsets = insets;

			return button;
		}
	}
}

