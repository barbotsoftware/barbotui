using System.Collections.Generic;
using UIKit;
using BarBot.Core.Model;
using BarBot.Core.ViewModel;

namespace BarBot.iOS.View.Detail
{
	public class IngredientTableDataSource : UITableViewDataSource
	{
		public List<Ingredient> Rows { get; private set; }

		private DetailViewModel ViewModel => Application.Locator.Detail;

		AppDelegate Delegate;

		public IngredientTableDataSource()
		{
			Rows = ViewModel.Ingredients;
			Delegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
		}

		public override System.nint RowsInSection(UITableView tableView, System.nint section)
		{
			var numberOfRows = Rows.Count;
			return numberOfRows;
		}

		public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			var cell = (IngredientTableViewCell)tableView.DequeueReusableCell(IngredientTableViewCell.CellID, indexPath);

			Ingredient row;

			if (Delegate.IngredientsInBarBot.Ingredients.Count > 0)
			{
				row = Delegate.IngredientsInBarBot.GetIngredient(Rows[indexPath.Row].IngredientId);
			}
			else
			{
				row = Rows[indexPath.Row];
			}
			
			cell.StyleCell();		// TODO: move to cell constructor
			cell.UpdateRow(row);

			return cell;
		}
	}
}
