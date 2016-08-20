using System;
using CoreGraphics;
using UIKit;

namespace BarBot.iOS
{
	public class HomeViewController : BaseViewController
	{
		UIButton signInButton, registerButton;
		UILabel titleLabel;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			View.BackgroundColor = UIColor.White;

			configureTitleLabel();
			configureSignInButton();
			configureRegisterButton();
			View.AddSubviews(new UIView[] { signInButton, registerButton, titleLabel });
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			NavigationController.SetNavigationBarHidden(true, true);
		}

		void configureTitleLabel()
		{
			titleLabel = new UILabel();
			titleLabel.Text = "BarBot";
			titleLabel.TextColor = styles.BarBotBlue;
			titleLabel.Font = UIFont.SystemFontOfSize(50, UIFontWeight.Bold);
			titleLabel.TextAlignment = UITextAlignment.Center;
			titleLabel.Frame = new CGRect(20, 50, View.Bounds.Width - 40, 50);
		}

		void configureSignInButton()
		{
			signInButton = createUIButton("Sign In", 20, View.Bounds.Bottom - 130, View.Bounds.Width - 40);
			var signInViewController = new SignInViewController();

			signInButton.TouchUpInside += (sender, e) =>
			{
				NavigationController.PushViewController(signInViewController, true);
			};
		}

		void configureRegisterButton()
		{
			registerButton = createUIButton("Register", 20, View.Bounds.Bottom - 70, View.Bounds.Width - 40);
			var registerViewController = new RegisterViewController();

			registerButton.TouchUpInside += (sender, e) =>
			{
				NavigationController.PushViewController(registerViewController, true);
			};
		}

	}
}

