using System;
using CoreGraphics;
using UIKit;

namespace BarBot.iOS
{
	public class HomeViewController : BaseViewController
	{
		UILabel titleLabel;
		UIImageView logoImageView;
		UIButton signInButton, registerButton;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			View.BackgroundColor = UIColor.White;

			configureTitleLabel();
			configureLogoImage();
			configureSignInButton();
			configureRegisterButton();
			View.AddSubviews(new UIView[] { titleLabel, logoImageView, signInButton, registerButton });
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
			titleLabel.Font = UIFont.SystemFontOfSize(75, UIFontWeight.Bold);
			titleLabel.TextAlignment = UITextAlignment.Center;
			titleLabel.Frame = new CGRect(20, 70, View.Bounds.Width - 40, 75);
		}

		void configureLogoImage()
		{
			nfloat sq = View.Bounds.Width - 60;
			UIImage logoImage = UIImage.FromBundle("Images/180iphoneapp.png");
			logoImageView = new UIImageView();
			logoImageView.Image = logoImage;
			logoImageView.Frame = new CGRect(30, 175, sq, sq);
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

