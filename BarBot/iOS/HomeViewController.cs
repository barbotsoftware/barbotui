using System;
using CoreGraphics;
using UIKit;

namespace BarBot.iOS
{
	public class HomeViewController : UIViewController
	{
		private CustomStyles styles;

		public HomeViewController()
		{
			styles = new CustomStyles();
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			View.BackgroundColor = UIColor.White;

			View.AddSubview(TitleLabel());
			View.AddSubview(SignInButton());
		}

		private UILabel TitleLabel()
		{
			var titleLabel = new UILabel();
			titleLabel.Text = "BarBot";
			titleLabel.TextColor = styles.BarBotBlue;
			return titleLabel;
		}

		private UIButton SignInButton()
		{
			var signInButton = UIButton.FromType(UIButtonType.System);
			signInButton.Frame = new CGRect(20, 200, 280, 44);
			signInButton.SetTitle("Sign In", UIControlState.Normal);

			signInButton.TouchUpInside += (sender, e) =>
			{
				//this.NavigationController.PushViewController(signInViewController, true);
			};

			return signInButton;
		}
			
	}
}

