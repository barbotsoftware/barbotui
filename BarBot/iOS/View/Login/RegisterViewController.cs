using System;
using UIKit;
using CoreGraphics;

namespace BarBot.iOS
{
	public class RegisterViewController : BaseUserValidationViewController
	{
		UISwitch privacySwitch;
		UILabel privacyLabel;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			Title = "Register";
			configureSubmitButton("Create Account");
			configurePrivacySwitch();
			configurePrivacyLabel();

			View.AddSubviews(new UIView[] { submitButton, privacySwitch, privacyLabel });
		}

		void configurePrivacySwitch()
		{
			privacySwitch = new UISwitch
			{
				Frame = new CGRect(View.Bounds.Width - 70, 260, 0, 0),
				//Action = privacySwitchChanged()
			};
		}

		void configurePrivacyLabel()
		{
			privacyLabel = new UILabel
			{
				Frame = new CGRect(22, 268, View.Bounds.Width - 100, 15),
				Text = "I agree to the Privacy Policy and Terms of Use",
				Font = UIFont.SystemFontOfSize(13, UIFontWeight.Thin)
			};
		}

		void privacySwitchChanged()
		{
			
		}
	}
}

