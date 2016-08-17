using System;

using UIKit;

namespace BarBot.iOS
{
	public partial class HomeViewController : UIViewController
	{

		public UIColor BarBotBlue = new UIColor(
			new nfloat(4.0/255.0), 
			new nfloat(75.0/255.0), 
			new nfloat(154.0/255.0),
			new nfloat(1.0));
		
		partial void SignInButton_TouchUpInside(UIButton sender)
		{
			Console.WriteLine("sign in segue");
		}

		public HomeViewController() : base("HomeViewController", null)
		{
		}

		public HomeViewController(IntPtr handle) : base (handle) 
		{
		}

		public override void AwakeFromNib()
		{
			base.AwakeFromNib();
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib
			ConfigureView();
		}

		public void ConfigureView()
		{
			SignInButton.Layer.CornerRadius = RegisterButton.Layer.CornerRadius = new nfloat(5.0);
			SignInButton.Layer.BorderWidth = RegisterButton.Layer.BorderWidth = new nfloat(0.9);
			SignInButton.Layer.BorderColor = RegisterButton.Layer.BorderColor = BarBotBlue.CGColor;
			var insets = new UIEdgeInsets(new nfloat(2.0), new nfloat(2.0), new nfloat(2.0),
												   new nfloat(2.0));
			SignInButton.TitleEdgeInsets = RegisterButton.TitleEdgeInsets = insets;
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}


