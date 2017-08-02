using System;
using UIKit;
using Foundation;
using BarBot.Core.Model;
using BarBot.Core.ViewModel;

namespace BarBot.iOS.View.Menu.Search
{
	public class SearchResultCell : UITableViewCell
	{
		MenuViewModel ViewModel => Application.Locator.Menu;
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

			if (!ViewModel.ImageCache.ContainsKey(element.Name))
			{
				// Load new Image
				ImageContents = await appDelegate.RestService.LoadImage(element.Img);
				ViewModel.ImageCache.Add(element.Name, ImageContents);
			}
			else
			{
				// find in Image Cache
				ImageContents = ViewModel.ImageCache[element.Name];
			}
		}
	}
}
