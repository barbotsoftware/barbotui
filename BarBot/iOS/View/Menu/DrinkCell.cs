using System;
//using System.Net.Http;
//using System.Threading.Tasks;
using Foundation;
using UIKit;
using CoreGraphics;
using BarBot.Core.Model;

namespace BarBot.iOS.View.Menu
{
    public class RecipeCollectionViewCell : UICollectionViewCell
    {
		public static NSString CellID = new NSString("RecipeCell");
		public UILabel LabelView;
		public UIImageView ImageView;

        public RecipeCollectionViewCell (IntPtr handle) : base (handle)
        {
        }

		[Export("initWithFrame:")]
		public RecipeCollectionViewCell(CGRect Frame) : base(Frame)
		{
			ImageView = new UIImageView();
			ImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
			ContentView.AddSubview(ImageView);

			LabelView = new UILabel
			{
				TextColor = UIColor.White,
				TextAlignment = UITextAlignment.Center,
				AdjustsFontSizeToFitWidth = true,
				Font = UIFont.FromName("Microsoft-Yi-Baiti", 26f)
			};
			ContentView.AddSubview(LabelView);
		}

		public void UpdateRow(Recipe element)
		{
			LabelView.Text = element.Name;
			ImageView.Image = UIImage.FromFile("Images/HexagonTile.png");

			var point = new CGPoint(ContentView.Frame.X, ContentView.Frame.Y);
			var size = new CGSize(ContentView.Frame.Width, ContentView.Frame.Height);

			ImageView.Frame = new CGRect(point, size);
			ImageView.Center = new CGPoint(ContentView.Center.X, ContentView.Center.Y);
			LabelView.Frame = new CGRect(ContentView.Frame.X,
										 ContentView.Frame.Y,
										 ContentView.Frame.Width,
										 ContentView.Frame.Height);
		}

		//public async Task<UIImage> LoadImage(string imageUrl)
		//{
		//	var httpClient = new HttpClient();

		//	// await! control returns to the caller and the task continues to run on another thread
		//	var contents = await httpClient.GetByteArrayAsync(imageUrl);

		//	// load from bytes
		//	return UIImage.LoadFromData(NSData.FromArray(contents));
		//}
	}
}