using System;
using CoreGraphics;
using UIKit;

namespace BarBot.iOS
{
	public class BaseUserValidationViewController: BaseViewController
	{
		UITextField usernameField, passwordField;
		UIButton submitButton;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			View.BackgroundColor = UIColor.White;

			nfloat h = 31.0f;
			nfloat w = View.Bounds.Width;

			usernameField = new UITextField
			{
				Placeholder = "Email",
				BorderStyle = UITextBorderStyle.RoundedRect,
				Frame = new CGRect(20, 82, w - 40, h)
			};

			passwordField = new UITextField
			{
				Placeholder = "Password",
				BorderStyle = UITextBorderStyle.RoundedRect,
				Frame = new CGRect(20, 116, w - 40, h),
				SecureTextEntry = true
			};

			submitButton = createUIButton("Submit", 20, 148, w - 40);

			submitButton.TouchUpInside += (sender, e) =>
			{
				Console.WriteLine("Submit button pressed");
			};

			View.AddSubviews(new UIView[] { usernameField, passwordField, submitButton });
		}

		public override void ViewWillAppear(Boolean animated)
		{
			base.ViewWillAppear(animated);
			NavigationController.SetNavigationBarHidden(false, true);
		}
	}
}

