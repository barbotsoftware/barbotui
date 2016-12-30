using System;
using Foundation;
using UIKit;

namespace BarBot.iOS.View.Detail.IngredientTable.Picker
{
	public class AddIngredientPickerCell : UITableViewCell
	{
		public static NSString CellID = new NSString("PickerCell");

		public AddIngredientPickerCell(IntPtr handle) : base(handle)
		{
		}

		public AddIngredientPickerCell()
		{
			ConfigureCell();
		}

		void ConfigureCell()
		{
			TextLabel.Text = "Add Ingredient";
		}
	}
}
