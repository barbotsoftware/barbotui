using System;
using UIKit;

namespace BarBot.iOS.View.Menu.Search
{
	public class SearchControllerDelegate : UISearchControllerDelegate
	{
		Action dismissSearchController;

		public SearchControllerDelegate(Action dismissSearchController)
		{
			this.dismissSearchController = dismissSearchController;
		}

		public override void WillDismissSearchController(UISearchController searchController)
		{
			dismissSearchController();
		}
	}
}
