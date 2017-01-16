using System.Linq;
using UIKit;
using Foundation;
using CoreGraphics;
using BarBot.Core.Model;
using BarBot.Core.ViewModel;
using BarBot.iOS.Style;
using BarBot.iOS.View.Detail.IngredientTable.Picker;

namespace BarBot.iOS.View.Detail.IngredientTable
{
	public class IngredientTableView : UITableView
	{
		DetailViewModel ViewModel => Application.Locator.Detail;

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

		public override void SetEditing(bool editing, bool animated)
		{
			base.SetEditing(editing, animated);
			if (editing)
			{
				ShowAddNewIngredientRow();
			}
			else
			{
				if (AddIngredientPickerIsShown())
				{
					BeginUpdates();
					HideExistingPicker();
					EndUpdates();
				}
				HideAddNewIngredientRow();
			}
			base.SetEditing(editing, animated);
		}

		// returns if the Add Ingredient Picker Cell is Shown
		public bool AddIngredientPickerIsShown()
		{
			return AddIngredientPickerIndexPath != null;
		}

		// returns if the passed row is an Add Ingredient Cell
		public bool RowIsAddIngredientCell(int row)
		{
			return row == ViewModel.Ingredients.Count - 1;
		}

		// Show new Ingredient Picker
		public void ShowNewPickerAtIndex(NSIndexPath indexPath)
		{
			NSIndexPath[] indexPaths = { NSIndexPath.FromRowSection(indexPath.Row + 1, 0) };

			InsertRows(indexPaths, UITableViewRowAnimation.Middle);

			if (RowIsAddIngredientCell(indexPath.Row))
			{
				// initialize some fields: transition from new ingredient to added ingredient
				ViewModel.Ingredients[indexPath.Row].IngredientId = ViewModel.IngredientsInBarBot[0].IngredientId;
				ViewModel.Ingredients[indexPath.Row].Name = ViewModel.IngredientsInBarBot[0].Name;
				ViewModel.Ingredients[indexPath.Row].Quantity = 0.5;

				var cell = CellAt(indexPath);
				ConfigureIngredientCell((IngredientTableViewCell)cell, indexPath);
			}
		}

		// Hides Ingredient Picker
		public void HideExistingPicker()
		{
			DeleteRows(new NSIndexPath[] { NSIndexPath.FromRowSection(AddIngredientPickerIndexPath.Row, 0) },
					   UITableViewRowAnimation.Middle);
			AddIngredientPickerIndexPath = null;
		}

		// Adds a 'Add Ingredient' cell to UITableView and Ingredients array
		public void ShowAddNewIngredientRow()
		{
			ViewModel.Ingredients.Add(new Ingredient("add_ingredient", "Add Ingredient", 0.0));

			InsertRows(new NSIndexPath[] { NSIndexPath.FromRowSection(ViewModel.Ingredients.Count - 1, 0) }, UITableViewRowAnimation.Fade);
		}

		// Hides 'Add Ingredient' cell from UITableView and removes from Steps array
		public void HideAddNewIngredientRow()
		{
			var ingredientNumber = ViewModel.Ingredients.Count;
			ViewModel.Ingredients.RemoveAt(ingredientNumber - 1);

			DeleteRows(new NSIndexPath[] { NSIndexPath.FromRowSection(ingredientNumber - 1, 0) }, UITableViewRowAnimation.Fade);
		}

		// Calculate Index for Add Ingredient Picker
		public NSIndexPath CalculateIndexPathForNewPicker(NSIndexPath indexPath)
		{
			NSIndexPath newIndexPath;
			if (AddIngredientPickerIsShown() && AddIngredientPickerIndexPath.Row < indexPath.Row)
			{
				newIndexPath = NSIndexPath.FromRowSection(indexPath.Row - 1, 0);
			}
			else
			{
				newIndexPath = NSIndexPath.FromRowSection(indexPath.Row, 0);
			}

			return newIndexPath;
		}

		// Configure Ingredient Cell
		public UITableViewCell ConfigureIngredientCell(IngredientTableViewCell cell, NSIndexPath indexPath)
		{
			Ingredient row;

			row = ViewModel.Ingredients[indexPath.Row];

			SharedStyles.StyleCell(cell);
			ConfigureDisplayStringForCell(cell, row);

			return cell;
		}

		// Configure Picker Cell
		public UITableViewCell ConfigurePickerCell(AddIngredientPickerCell cell, NSIndexPath indexPath)
		{
			var ingredient = ViewModel.Ingredients[indexPath.Row - 1];

			var pickerView = new UIPickerView();
			pickerView.DataSource = new IngredientPickerViewDataSource();
			pickerView.Delegate = new IngredientPickerViewDelegate(this);
			pickerView.Frame = new CGRect(0, 0, cell.Frame.Width, 216);

			var quantity = ingredient.Quantity;

			// select default quantity (volume) value for new picker view
			if (double.IsNaN(quantity))
			{
				pickerView.Select(ViewModel.Quantities.IndexOf(quantity), 0, true);
			}
			else
			{
				pickerView.Select(0, 0, true);
			}

			if (ingredient.Name != null)
			{
				var fetchedIngredient = ViewModel.IngredientsInBarBot.FirstOrDefault(i => i.Name.Equals(ingredient.Name));

				var indexOfIngredient = ViewModel.IngredientsInBarBot.IndexOf(fetchedIngredient);

				pickerView.Select(indexOfIngredient, 1, true);
			}
			else
			{
				pickerView.Select(0, 1, true);
			}

			SharedStyles.StylePickerView(pickerView);
			cell.AddSubview(pickerView);

			return cell;
		}

		void ConfigureDisplayStringForCell(UITableViewCell cell, Ingredient element)
		{
			if (element.IngredientId.Equals("add_ingredient"))
			{
				cell.TextLabel.Text = element.Name;
			}
			else
			{
				cell.TextLabel.Text = element.Quantity + " oz " + element.Name;
			}
		}
	}
}
