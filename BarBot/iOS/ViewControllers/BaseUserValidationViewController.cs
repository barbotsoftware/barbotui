using System;
using CoreGraphics;
using UIKit;

namespace BarBot.iOS
{
	public class BaseUserValidationViewController: BaseViewController
	{
		UITextField usernameField, passwordField;
		protected UIButton submitButton;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			View.BackgroundColor = UIColor.White;

			configureTextFields();
			View.AddSubviews(new UIView[] { usernameField, passwordField });
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			NavigationController.SetNavigationBarHidden(false, true);
		}

		void configureTextFields()
		{
			nfloat h = 40.0f;
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
				Frame = new CGRect(20, 142, w - 40, h),
				SecureTextEntry = true
			};
		}

		protected void configureSubmitButton(string title)
		{
			submitButton = createUIButton(title, 20, 202, View.Bounds.Width - 40);
		}
	}
}

