using System;
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
		}

		void configureTitleLabel()
		{
			titleLabel = new UILabel();
			titleLabel.Text = "BarBot";
			titleLabel.TextColor = styles.BarBotBlue;
			View.AddSubview(titleLabel);
		}

		void configureSignInButton()
		{
			signInButton = createUIButton("Sign In", 15, 200);
			var signInViewController = new SignInViewController();

			signInButton.TouchUpInside += (sender, e) =>
			{
				NavigationController.PushViewController(signInViewController, true);
			};
			View.AddSubview(signInButton);
		}

		void configureRegisterButton()
		{
			registerButton = createUIButton("Register", 15, 255);
			var registerViewController = new RegisterViewController();

			registerButton.TouchUpInside += (sender, e) =>
			{
				NavigationController.PushViewController(registerViewController, true);
			};
			View.AddSubview(registerButton);
		}

	}
}

