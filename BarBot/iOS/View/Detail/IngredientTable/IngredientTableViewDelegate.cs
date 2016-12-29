using UIKit;
using Foundation;

namespace BarBot.iOS.View.Detail.IngredientTable
{
	public class IngredientTableViewDelegate : UITableViewDelegate
	{

		public IngredientTableViewDelegate()
		{
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			var ingredientTableView = tableView as IngredientTableView;

			// edit ingredient, quantity
			ingredientTableView.BeginUpdates();

			if (ingredientTableView.AddIngredientPickerIsShown() 
			    && ingredientTableView.AddIngredientPickerIndexPath.Row - 1 == indexPath.Row) 
			{
				//if rowIsAddIngredientCell(indexPath.Row) {
				//	ingredientTableView.ShowAddNewIngredientRow();
				//}
			
				//ingredientTableView.hideExistingPicker()
			} 
			else 
			{
				//var newPickerIndexPath = CalculateIndexPathForNewPicker(indexPath);
				//	if self.addIngredientPickerIsShown() {
				//			self.hideExistingPicker()

				//}

				//self.showNewPickerAtIndex(newPickerIndexPath)


				//ingredientTableView.AddIngredientPickerIndexPath = NSIndexPath.FromRowSection(newPickerIndexPath.row + 1, 0);

			}

			ingredientTableView.DeselectRow(indexPath, true);

			ingredientTableView.EndUpdates();

			// scroll row with open picker view to top of table view
			ingredientTableView.ScrollToRow(indexPath, UITableViewScrollPosition.Top, true);
		}

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

		public override bool ShouldIndentWhileEditing(UITableView tableView, NSIndexPath indexPath)
		{
			if (IndexPathIsIngredientPicker(tableView, indexPath)) 
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		bool IndexPathIsIngredientPicker(UITableView tableView, NSIndexPath indexPath)
		{
			var ingredientTableView = tableView as IngredientTableView;

			return ingredientTableView.AddIngredientPickerIsShown() &&
				(ingredientTableView.AddIngredientPickerIndexPath.Row == indexPath.Row ||
				 ingredientTableView.AddIngredientPickerIndexPath.Row - 1 == indexPath.Row);
		}
	}
}
