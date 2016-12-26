using UIKit;
using BarBot.iOS.Util;

namespace BarBot.iOS.View.Menu
{
	public class DrinkSearchResultsViewController : UITableViewController
	{
		public DrinkSearchResultsViewController()
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			TableView.BackgroundColor = Color.BackgroundGray;
			TableView.SeparatorColor = Color.NavBarGray;
		}

		public void Search(string forSearchString)
		{
			
		}
	}
}
