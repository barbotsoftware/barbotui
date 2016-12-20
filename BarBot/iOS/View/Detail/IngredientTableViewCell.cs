﻿using System;
using UIKit;
using Foundation;
using BarBot.Core.Model;

namespace BarBot.iOS.View.Detail
{
	public class IngredientTableViewCell : UITableViewCell
	{
		public static NSString CellID = new NSString("IngredientCell");

		public IngredientTableViewCell(IntPtr handle) : base(handle)
		{
		}

		public void StyleCell()
		{
			BackgroundColor = Color.BackgroundGray;
			TextLabel.TextColor = UIColor.White;
			TextLabel.Font = UIFont.FromName("Microsoft-Yi-Baiti", 26f);
		}

		public void UpdateRow(Ingredient element)
		{
			TextLabel.Text = element.IngredientId;
		}
	}
}
