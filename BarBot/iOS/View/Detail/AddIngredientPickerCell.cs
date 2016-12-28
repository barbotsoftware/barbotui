using Foundation;
using UIKit;

namespace BarBot.iOS.View.Detail
{
	public class AddIngredientPickerCell : UITableViewCell
	{
		public static NSString CellID = new NSString("PickerCell");

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
