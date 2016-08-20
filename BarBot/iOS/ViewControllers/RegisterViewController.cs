using System;

namespace BarBot.iOS
{
	public class RegisterViewController : BaseUserValidationViewController
	{
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			Title = "Register";
			configureSubmitButton("Create Account");
			View.AddSubview(submitButton);
		}
	}
}

