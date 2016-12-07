using System;
using UIKit;
using BarBot.ViewModel;

namespace BarBot.iOS.View.Order
{
	public class IngredientTableViewDataSource : UITableViewDataSource
	{
		public RecipeViewModel ViewModel { get; set; }

		public IngredientTableViewDataSource(RecipeViewModel viewModel)
		{
			ViewModel = viewModel;
		}

		public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			throw new NotImplementedException();
		}

		public override nint RowsInSection(UITableView tableView, nint section)
		{
			var NumberOfRows = 0;
			if (ViewModel.Recipe.Steps != null)
			{
				NumberOfRows = ViewModel.Recipe.Steps.Length;
			}

			return NumberOfRows;
		}
	}
}
