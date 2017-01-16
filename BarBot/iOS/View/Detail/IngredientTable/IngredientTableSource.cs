﻿using System;
using System.Collections.Generic;
using UIKit;
using Foundation;
using BarBot.Core.Model;
using BarBot.Core.ViewModel;
using BarBot.iOS.View.Detail.IngredientTable.Picker;

namespace BarBot.iOS.View.Detail.IngredientTable
{
	public class IngredientTableSource : UITableViewSource
	{
		public List<Ingredient> Rows { get; private set; }

		private DetailViewModel ViewModel => Application.Locator.Detail;

		public IngredientTableSource()
		{
			Rows = ViewModel.Ingredients;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
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
				cell = ingredientTableView.ConfigurePickerCell((AddIngredientPickerCell)cell, indexPath);
			}
			else
			{
				// Configure Regular Cell
				cell = tableView.DequeueReusableCell(IngredientTableViewCell.CellID, indexPath);
				cell = ingredientTableView.ConfigureIngredientCell((IngredientTableViewCell)cell, indexPath);
			}

			return cell;
		}



		public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
		{
			if (editingStyle == UITableViewCellEditingStyle.Delete) {
				// Delete the row from the data source

				// Remove Ingredient from Recipe
				ViewModel.Ingredients.RemoveAt(indexPath.Row);

				tableView.DeleteRows(new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Top);
				tableView.ReloadSections(NSIndexSet.FromIndex(0), UITableViewRowAnimation.Fade);

				// Set Available Ingredients
				ViewModel.RefreshAvailableIngredients();
			}
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
