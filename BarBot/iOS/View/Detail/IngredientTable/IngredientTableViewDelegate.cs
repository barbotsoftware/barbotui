using System;
using UIKit;
using Foundation;

namespace BarBot.iOS.View.Detail.IngredientTable
{
	public class IngredientTableViewDelegate : UITableViewDelegate
	{
		
		public IngredientTableViewDelegate()
		{
		}

		// Called when a Row is Selected
		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			var ingredientTableView = tableView as IngredientTableView;

			// edit ingredient, quantity
			ingredientTableView.BeginUpdates();

			if (ingredientTableView.AddIngredientPickerIsShown()
			    && ingredientTableView.AddIngredientPickerIndexPath.Row - 1 == indexPath.Row) 
			{
				if (ingredientTableView.RowIsAddIngredientCell(indexPath.Row)) {
					ingredientTableView.ShowAddNewIngredientRow();
				}

				ingredientTableView.HideExistingPicker();
			}
			else
			{
				var newPickerIndexPath = ingredientTableView.CalculateIndexPathForNewPicker(indexPath);
				if (ingredientTableView.AddIngredientPickerIsShown()) 
				{
					ingredientTableView.HideExistingPicker();
				}

				ingredientTableView.ShowNewPickerAtIndex(newPickerIndexPath);

				ingredientTableView.AddIngredientPickerIndexPath = NSIndexPath.FromRowSection(newPickerIndexPath.Row + 1, 0);
			}

			ingredientTableView.DeselectRow(indexPath, true);

			ingredientTableView.EndUpdates();

			// scroll row with open picker view to top of table view
			ingredientTableView.ScrollToRow(indexPath, UITableViewScrollPosition.Top, true);
		}

		// Called to set editing icons for each row
		public override UITableViewCellEditingStyle EditingStyleForRow(UITableView tableView, NSIndexPath indexPath)
		{
			var lastIndexPath = NSIndexPath.FromRowSection(tableView.NumberOfRowsInSection(0) - 1, 0);

			if (IndexPathIsIngredientPicker(tableView, indexPath))
			{
				return UITableViewCellEditingStyle.None;
			}
			else if (indexPath.Row == lastIndexPath.Row) 
			{
				return UITableViewCellEditingStyle.Insert;
			}
			else
			{
				return UITableViewCellEditingStyle.Delete;
			}
		}

		// Called to set editing indent for each row
		public override bool ShouldIndentWhileEditing(UITableView tableView, NSIndexPath indexPath)
		{
			if (IndexPathIsIngredientPicker(tableView, indexPath))
			{
				return false;
			}
			return true;
		}

		// Called to set height for each row
		public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
		{
			var ingredientTableView = tableView as IngredientTableView;

			var rowHeight = ingredientTableView.RowHeight;

			if (ingredientTableView.AddIngredientPickerIsShown() && 
			    ingredientTableView.AddIngredientPickerIndexPath.Row == indexPath.Row) {

				// get Picker View Cell Height
				//var pickerViewCellToCheck = tableView.DequeueReusableCell(AddIngredientPickerCell.CellID);
				rowHeight = 216;//pickerViewCellToCheck.Frame.Height;
			}

			return rowHeight;
		}

		// Called to determine if the passed indexPath is a Picker Cell
		bool IndexPathIsIngredientPicker(UITableView tableView, NSIndexPath indexPath)
		{
			var ingredientTableView = tableView as IngredientTableView;

			return ingredientTableView.AddIngredientPickerIsShown() &&
				(ingredientTableView.AddIngredientPickerIndexPath.Row == indexPath.Row ||
				 ingredientTableView.AddIngredientPickerIndexPath.Row - 1 == indexPath.Row);
		}
	}
}
