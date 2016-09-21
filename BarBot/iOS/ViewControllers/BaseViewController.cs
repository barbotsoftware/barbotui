using System;
using CoreGraphics;
using UIKit;

namespace BarBot.iOS
{
	public class BaseViewController : UIViewController
	{

		public BaseViewController()
		{
		}

		protected UIButton createUIButton(string title, nfloat x, nfloat y, nfloat width)
		{
			var button = UIButton.FromType(UIButtonType.System);
			button.Frame = new CGRect(x, y, width, 45);
			button.SetTitle(title, UIControlState.Normal);
			button.SetTitleColor(UIColor.White, UIControlState.Normal);
			button.BackgroundColor = Color.BarBotBlue;
			button.Layer.CornerRadius = new nfloat(2.0);
			button.Layer.BorderWidth = new nfloat(0.9);
			button.Layer.BorderColor = Color.BarBotBlue.CGColor;
			var insets = new UIEdgeInsets(new nfloat(2.0), new nfloat(2.0), new nfloat(2.0),
								 new nfloat(2.0));
			button.TitleEdgeInsets = insets;

			return button;
		}
	}
}

