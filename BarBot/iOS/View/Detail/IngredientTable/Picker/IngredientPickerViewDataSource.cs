using System;
using UIKit;
using BarBot.Core.ViewModel;

namespace BarBot.iOS.View.Detail.IngredientTable.Picker
{
	public class IngredientPickerViewDataSource : UIPickerViewDataSource
	{
		private RecipeDetailViewModel ViewModel => Application.Locator.Detail;

		public IngredientPickerViewDataSource()
		{
		}

		// Components: Quantity and Ingredient
		public override nint GetComponentCount(UIPickerView pickerView)
		{
			return 2;
		}

		// Rows for each Component
		public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
		{
			if (component == 0)
			{
				return ViewModel.Quantities.Count;
			}
			else
			{
				return ViewModel.AvailableIngredients.Count;
			}
		}
	}
}
