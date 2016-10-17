using UIKit;

namespace BarBot.iOS
{
	public class BarBotNavigationController : UINavigationController
	{
		public BarBotNavigationController(UIViewController controller) : base(controller)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			NavBarStyle();
		}

		void NavBarStyle()
		{
			NavigationBar.TintColor = UIColor.White;
			NavigationBar.BarTintColor = Color.BarBotBlue;
			NavigationBar.TitleTextAttributes = new UIStringAttributes
			{
				ForegroundColor = UIColor.White
			};
		}

		public override UIStatusBarStyle PreferredStatusBarStyle()
		{
			return UIStatusBarStyle.LightContent;
		}
	}
}
