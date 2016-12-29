using UIKit;
using Foundation;
using BarBot.Core.Model;
using BarBot.Core.ViewModel;
using BarBot.iOS.Util;

namespace BarBot.iOS.View.Detail.IngredientTable
{
	public class IngredientTableView : UITableView
	{
		private DetailViewModel ViewModel => Application.Locator.Detail;

		public NSIndexPath AddIngredientPickerIndexPath { get; set; }

		public IngredientTableView()
		{
			Configure();
		}

		void Configure()
		{
			BackgroundColor = Color.BackgroundGray;
			RowHeight = 45;
			ScrollEnabled = true;
			ShowsVerticalScrollIndicator = true;
			AllowsSelection = false;
			AllowsSelectionDuringEditing = true;
			Bounces = true;
			SeparatorColor = Color.NavBarGray;
		}

		public bool AddIngredientPickerIsShown()
		{
			return false;//AddIngredientPickerIndexPath != null;
		}

		public override void SetEditing(bool editing, bool animated)
		{
			base.SetEditing(editing, animated);
			if (editing)
			{
				ShowAddNewIngredientRow();
			}
			else
			{
				HideAddNewIngredientRow();
			}
		}

		// Adds a 'Add Ingredient' cell to UITableView and Ingredients array
		public void ShowAddNewIngredientRow()
		{
			ViewModel.Ingredients.Add(new Ingredient("add_ingredient", "Add Ingredient", 0.0));

			InsertRows(new NSIndexPath[] { NSIndexPath.FromRowSection(ViewModel.Ingredients.Count - 1, 0)}, UITableViewRowAnimation.Fade);
    	}

		// Hides 'Add Ingredient' cell from UITableView and removes from Steps array
		public void HideAddNewIngredientRow()
		{
			ViewModel.Ingredients.RemoveAt(ViewModel.Ingredients.Count - 1);

			DeleteRows(new NSIndexPath[] { NSIndexPath.FromRowSection(ViewModel.Ingredients.Count, 0) }, UITableViewRowAnimation.Fade);
    	}
	}
}
