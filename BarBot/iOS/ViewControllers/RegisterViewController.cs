using System;
using UIKit;
using CoreGraphics;

namespace BarBot.iOS
{
	public class RegisterViewController : BaseUserValidationViewController
	{
		UISwitch privacySwitch;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			Title = "Register";
			configureSubmitButton("Create Account");
			configurePrivacySwitch();

			View.AddSubviews(new UIView[] { submitButton, privacySwitch });
		}

		void configurePrivacySwitch()
		{
			privacySwitch = new UISwitch
			{
				Frame = new CGRect(View.Bounds.Width - 70, 260, 0, 0)
			};
		}
	}
}

