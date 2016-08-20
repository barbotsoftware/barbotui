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

		protected UIButton createUIButton(string title, int x, int y)
		{
			var btn = UIButton.FromType(UIButtonType.System);
			btn.Frame = new CGRect(x, y, View.Bounds.Width - 30, 44);
			btn.SetTitle(title, UIControlState.Normal);
			btn.SetTitleColor(UIColor.White, UIControlState.Normal);
			btn.BackgroundColor = styles.BarBotBlue;
			btn.Layer.CornerRadius = new nfloat(5.0);
			btn.Layer.BorderWidth = new nfloat(0.9);
			btn.Layer.BorderColor = styles.BarBotBlue.CGColor;
			var insets = new UIEdgeInsets(new nfloat(2.0), new nfloat(2.0), new nfloat(2.0),
								 new nfloat(2.0));
			btn.TitleEdgeInsets = insets;

			return btn;
		}
	}
}

