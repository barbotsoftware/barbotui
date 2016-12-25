using System;
using UIKit;
namespace BarBot.iOS.View.Menu
{
	public class DrinkSearchController : UISearchController
	{
		public DrinkSearchController(UIViewController searchResultsController) : base(searchResultsController)
		{
			Configure();
		}

		public void Configure()
		{
			

			//Creates a search controller updater
			var searchUpdater = new DrinkSearchResultsUpdater();
			//searchUpdater.UpdateSearchResults += SearchResultsController.Search;
			SearchResultsUpdater = searchUpdater;

			//format the search bar
			SearchBar.SizeToFit();
			SearchBar.SearchBarStyle = UISearchBarStyle.Minimal;
			SearchBar.Placeholder = "Search";
			SearchBar.ShowsCancelButton = true;

			//the search bar is contained in the navigation bar, so it should be visible
			HidesNavigationBarDuringPresentation = false;
		}

		public class DrinkSearchResultsUpdater : UISearchResultsUpdating
		{
			public event Action<string> UpdateSearchResults = delegate { };

			public override void UpdateSearchResultsForSearchController(UISearchController searchController)
			{
				this.UpdateSearchResults(searchController.SearchBar.Text);
			}
		}
	}
}
