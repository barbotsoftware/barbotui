﻿using System;

namespace BarBot.iOS
{
	public class SignInViewController : BaseUserValidationViewController
	{
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			Title = "Sign In";
			configureSubmitButton("Sign In");
			View.AddSubview(submitButton);
		}
	}
}

