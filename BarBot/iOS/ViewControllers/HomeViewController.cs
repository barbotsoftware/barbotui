using System;
using CoreGraphics;
using UIKit;

namespace BarBot.iOS
{
	public class HomeViewController : UIViewController
	{
		private StyleUtil styles;

		public HomeViewController()
		{
			styles = new StyleUtil();
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			View.BackgroundColor = UIColor.White;

			View.AddSubview(TitleLabel());
			View.AddSubview(configureSignInButton());
			View.AddSubview(configureRegisterButton());
		}

		UILabel TitleLabel()
		{
			var titleLabel = new UILabel();
			titleLabel.Text = "BarBot";
			titleLabel.TextColor = styles.BarBotBlue;
			return titleLabel;
		}

		UIButton configureSignInButton()
		{
			var signInButton = createUIButton("Sign In", 20, 200);
			var signInViewController = new SignInViewController();

			signInButton.TouchUpInside += (sender, e) =>
			{
				NavigationController.PushViewController(signInViewController, true);
			};
			return signInButton;
		}

		UIButton configureRegisterButton()
		{
			var registerButton = createUIButton("Register", 20, 300);
			var registerViewController = new RegisterViewController();

			registerButton.TouchUpInside += (sender, e) =>
			{
				NavigationController.PushViewController(registerViewController, true);
			};
			return registerButton;
		}

		UIButton createUIButton(string title, int x, int y)
		{
			var btn = UIButton.FromType(UIButtonType.System);
			btn.Frame = new CGRect(x, y, 280, 44);
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

