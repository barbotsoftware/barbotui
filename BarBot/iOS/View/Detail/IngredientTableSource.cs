using System.Collections.Generic;
using UIKit;
using Foundation;
using BarBot.Core.Model;
using BarBot.Core.ViewModel;
using BarBot.iOS.Util;

namespace BarBot.iOS.View.Detail
{
	public class IngredientTableSource : UITableViewSource
	{
		public List<Ingredient> Rows { get; private set; }

		private DetailViewModel ViewModel => Application.Locator.Detail;

		public IngredientTableSource()
		{
			Rows = ViewModel.Ingredients;
		}

		public override System.nint RowsInSection(UITableView tableview, System.nint section)
		{
			var numberOfRows = Rows.Count;
			var ingredientTableView = tableview as IngredientTableView;

			if (ingredientTableView.AddIngredientPickerIsShown()) {
				numberOfRows += 1;
			}

			return numberOfRows;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var cell = new UITableViewCell();
			var ingredientTableView = tableView as IngredientTableView;

			// Configure Ingredient Picker Cell
			if (ingredientTableView.AddIngredientPickerIsShown() && ingredientTableView.AddIngredientPickerIndexPath.Row == indexPath.Row)
			{
				cell = tableView.DequeueReusableCell(AddIngredientPickerCell.CellID, indexPath);
				cell = ConfigurePickerCell((AddIngredientPickerCell)cell, indexPath);
			}
			else
			{
				// Configure Regular Cell
				cell = tableView.DequeueReusableCell(IngredientTableViewCell.CellID, indexPath);
				cell = ConfigureIngredientCell((IngredientTableViewCell)cell, indexPath);
			}


			return cell;
		}

		UITableViewCell ConfigureIngredientCell(IngredientTableViewCell cell, NSIndexPath indexPath)
		{
			Ingredient row;

			row = Rows[indexPath.Row];

			SharedStyles.StyleCell(cell);
			cell.UpdateRow(row);

			return cell;
		}

		UITableViewCell ConfigurePickerCell(AddIngredientPickerCell cell, NSIndexPath indexPath)
		{
			SharedStyles.StyleCell(cell);
			return cell;
		}

		public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
		{
			base.CommitEditingStyle(tableView, editingStyle, indexPath);
		}

		public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
		{
			return true;
		}

		public override bool CanMoveRow(UITableView tableView, NSIndexPath indexPath)
		{
			return false;
		}

		public override UITableViewCellEditingStyle EditingStyleForRow(UITableView tableView, NSIndexPath indexPath)
		{
			return UITableViewCellEditingStyle.Delete;
		}
	}
}
