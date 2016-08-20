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
				Frame = new CGRect(15, 82, w - 30, h)
			};

			passwordField = new UITextField
			{
				Placeholder = "Password",
				BorderStyle = UITextBorderStyle.RoundedRect,
				Frame = new CGRect(15, 116, w - 30, h),
				SecureTextEntry = true
			};

			submitButton = createUIButton("Submit", 15, 148);

			submitButton.TouchUpInside += (sender, e) =>
			{
				Console.WriteLine("Submit button pressed");
			};

			View.AddSubview(usernameField);
			View.AddSubview(passwordField);
			View.AddSubview(submitButton);
		}
	}
}

