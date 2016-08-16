using System;

using UIKit;

namespace BarBot.iOS
{
	public partial class HomeViewController : UIViewController
	{

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
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}


