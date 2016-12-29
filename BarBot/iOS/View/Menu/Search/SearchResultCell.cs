using System;
using UIKit;
using Foundation;
using BarBot.Core.Model;
using BarBot.iOS.Util;

namespace BarBot.iOS.View.Menu.Search
{
	public class SearchResultCell : UITableViewCell
	{
		public static NSString CellID = new NSString("SearchResultCell");

		public string RecipeId { get; set; }
		public byte[] ImageContents { get; set; }

		public SearchResultCell(IntPtr handle) : base(handle)
		{
		}

		public async void UpdateRow(Recipe element)
		{
			RecipeId = element.RecipeId;
			TextLabel.Text = element.Name;
			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
			ImageContents = await appDelegate.AsyncUtil.LoadImage(element.Img);
		}
	}
}
