using System.Collections.Generic;
using UIKit;
using BarBot.Core.Model;
using BarBot.Core.ViewModel;
using BarBot.iOS.Util;
using System;
using System.Linq;

namespace BarBot.iOS.View.Menu.Search
{
	public class DrinkSearchResultsViewController : UITableViewController
	{
		private MenuViewModel ViewModel => Application.Locator.Menu;

		public List<Recipe> Recipes { get; set; }
		public List<Recipe> SearchResults { get; set; }

		public DrinkSearchResultsViewController(List<Recipe> recipes)
		{
			Recipes = recipes;
			SearchResults = new List<Recipe>();
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			TableView.BackgroundColor = Color.BackgroundGray;
			TableView.SeparatorColor = Color.NavBarGray;
			TableView.RegisterClassForCellReuse(typeof(SearchResultCell), SearchResultCell.CellID);

			if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
			{
				AutomaticallyAdjustsScrollViewInsets = false;
				TableView.ContentInset = new UIEdgeInsets(64, 0, 44, 0);
			}
		}

		public override void ViewDidAppear(bool animated)
		{
			TableView.ReloadData();
		}

		public override nint RowsInSection(UITableView tableView, nint section)
		{
			return SearchResults.Count;
		}

		public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			var cell = (SearchResultCell)tableView.DequeueReusableCell(SearchResultCell.CellID, indexPath);

			SharedStyles.StyleCell(cell);
			cell.UpdateRow(SearchResults[indexPath.Row]);
			return cell;
		}

		public override void RowSelected(UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			tableView.DeselectRow(indexPath, true);
			var cell = (SearchResultCell)GetCell(tableView, indexPath);
			ViewModel.ShowDrinkDetailsCommand(cell.RecipeId, cell.ImageContents);
		}

		public void PerformSearch(string searchString)
		{
			SearchResults.Clear();
			searchString = searchString.Trim().ToLower();

			IEnumerable<Recipe> results = Recipes.Where(r => r.Name.ToLower().Contains(searchString));

			SearchResults.AddRange(results);

			TableView.ReloadData();
		}
	}
}
