using System;
using UIKit;
using Foundation;
using BarBot.Core.Model;
using BarBot.iOS.Util;

namespace BarBot.iOS.View.Detail
{
	public class IngredientTableViewCell : UITableViewCell
	{
		public static NSString CellID = new NSString("IngredientCell");

		public IngredientTableViewCell(IntPtr handle) : base(handle)
		{
		}

		public void UpdateRow(Ingredient element)
		{
			TextLabel.Text = element.Quantity + " oz " + element.Name;
		}
	}
}
