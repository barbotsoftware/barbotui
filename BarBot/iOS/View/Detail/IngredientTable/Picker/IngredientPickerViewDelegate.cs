using System;
using UIKit;
using Foundation;
using BarBot.Core.ViewModel;

namespace BarBot.iOS.View.Detail.IngredientTable.Picker
{
	public class IngredientPickerViewDelegate : UIPickerViewDelegate
	{
		DetailViewModel ViewModel => Application.Locator.Detail;
		IngredientTableView TableView;

		public IngredientPickerViewDelegate(IngredientTableView tableView)
		{
			TableView = tableView;
		}

		public override NSAttributedString GetAttributedTitle(UIPickerView pickerView, nint row, nint component)
		{
			string title = "";
			if (component == 0)
			{
				title = ViewModel.Quantities[(int)row].ToString();
			}
			else
			{
				title = ViewModel.IngredientsInBarBot[(int)row].Name;
			}

			var attributedString = new NSAttributedString(title, new UIStringAttributes()
			{
				ForegroundColor = UIColor.White,
				Font = UIFont.FromName("Microsoft-Yi-Baiti", 26f)
			});

			return attributedString;
		}

		public override void Selected(UIPickerView pickerView, nint row, nint component)
		{
			NSIndexPath parentCellIndexPath;

			if (TableView.AddIngredientPickerIsShown()) 
			{
				parentCellIndexPath = NSIndexPath.FromRowSection(TableView.AddIngredientPickerIndexPath.Row - 1, 0);
			}
			else
			{
				return;
			}

			var cell = (IngredientTableViewCell)TableView.CellAt(parentCellIndexPath);

			if (component == 0)
			{
				ViewModel.Ingredients[parentCellIndexPath.Row].Quantity = ViewModel.Quantities[(int)row];
			}
			else if (component == 1)
			{
				ViewModel.Ingredients[parentCellIndexPath.Row].IngredientId = ViewModel.IngredientsInBarBot[(int)row].IngredientId;
				ViewModel.Ingredients[parentCellIndexPath.Row].Name = ViewModel.IngredientsInBarBot[(int)row].Name;
			}

			//	self.recipe.custom = true

			TableView.ConfigureIngredientCell(cell, parentCellIndexPath);
		}
	}
}
