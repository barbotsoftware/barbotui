using System;
using Foundation;
using UIKit;

namespace BarBot.iOS.View.Menu.Search
{
	public class DrinkSearchController : UISearchController
	{
		DrinkSearchResultsViewController resultsController;

		public DrinkSearchController(UIViewController searchResultsController) : base(searchResultsController)
		{
			resultsController = (DrinkSearchResultsViewController)searchResultsController;
			Configure();
		}

		public void Configure()
		{
			//Creates a search controller updater
			var searchUpdater = new DrinkSearchResultsUpdater();
			searchUpdater.UpdateSearchResults += resultsController.PerformSearch;
			SearchResultsUpdater = searchUpdater;

			//format the search bar
			SearchBar.SizeToFit();
			SearchBar.SearchBarStyle = UISearchBarStyle.Minimal;
			SearchBar.KeyboardAppearance = UIKeyboardAppearance.Dark;
			SearchBar.Placeholder = "Search";
			SearchBar.ShowsCancelButton = true;

			// Configure SearchBar TextField
			foreach (UIView subView in SearchBar.Subviews)
			{
				foreach (UIView subsubView in subView.Subviews)
				{
					if (subsubView is UITextField)
					{
						var textField = subsubView as UITextField;
						textField.AttributedPlaceholder = new NSAttributedString("Search", 
						                                                         new UIStringAttributes { 
																					ForegroundColor = UIColor.White, 
																					Font = UIFont.FromName("Microsoft-Yi-Baiti", 13f)
																				});
						textField.TextColor = UIColor.White;
						textField.AutocapitalizationType = UITextAutocapitalizationType.None;
					}
				}
			}

			//the search bar is contained in the navigation bar, so it should be visible
			HidesNavigationBarDuringPresentation = false;
		}

		public void ShowSearchBar(UINavigationItem NavigationItem)
		{
			NavigationItem.TitleView = SearchBar;
			NavigationItem.SetRightBarButtonItem(null, true);
			SearchBar.BecomeFirstResponder();
		}

		public class DrinkSearchResultsUpdater : UISearchResultsUpdating
		{
			public event Action<string> UpdateSearchResults = delegate { };

			public override void UpdateSearchResultsForSearchController(UISearchController searchController)
			{
				UpdateSearchResults(searchController.SearchBar.Text);
			}
		}
	}
}
