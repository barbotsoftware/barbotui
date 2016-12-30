using System;
using UIKit;
using Foundation;
using BarBot.Core.Model;

namespace BarBot.iOS.View.Detail.IngredientTable
{
	public class IngredientTableViewCell : UITableViewCell
	{
		public static NSString CellID = new NSString("IngredientCell");

		public IngredientTableViewCell(IntPtr handle) : base(handle)
		{
		}
	}
}
