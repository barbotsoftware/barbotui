using UIKit;

using BarBot.Core.ViewModel;

namespace BarBot.iOS.View.Home
{
    public class HomeViewController : UIViewController
    {
        HomeViewModel Vm => Application.Locator.Home;

        public HomeViewController()
        {
        }

		public override void LoadView()
		{
			base.LoadView();
			var frame = View.Frame;

			// Pass base frame to Custom View
			View = new HomeView(frame);
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }
    }
}
