using System.Collections.Generic;
using UIKit;
using BarBot.Core.Model;
using BarBot.Core.ViewModel;

namespace BarBot.iOS.View.Detail
{
	public class IngredientTableDataSource : UITableViewDataSource
	{
		public List<Ingredient> Rows { get; private set; }

		private DetailViewModel ViewModel
		{
			get
			{
				return Application.Locator.Detail;
			}
		}

		public IngredientTableDataSource()
		{
			Rows = ViewModel.Ingredients;
		}

		public override System.nint RowsInSection(UITableView tableView, System.nint section)
		{
			var numberOfRows = Rows.Count;
			return numberOfRows;
		}

		public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			var cell = (IngredientTableViewCell)tableView.DequeueReusableCell(IngredientTableViewCell.CellID, indexPath);

			Ingredient row = Rows[indexPath.Row];
			
			cell.StyleCell();		// TODO: move to cell constructor
			cell.UpdateRow(row);

			return cell;
		}
	}
}
