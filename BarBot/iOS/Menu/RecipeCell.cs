﻿using System.Net.Http;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using UIKit;
using BarBot.Model;

namespace BarBot.iOS.Menu
{
	public class RecipeCell : UICollectionViewCell
	{
		public static NSString CellID = new NSString("RecipeCell");
		public UILabel LabelView;
		public UIImageView ImageView;

		[Export("initWithFrame:")]
		public RecipeCell(CGRect Frame) : base(Frame)
		{
			ContentView.Layer.BorderColor = UIColor.LightGray.CGColor;
			ContentView.Layer.BorderWidth = 1.0f / UIScreen.MainScreen.NativeScale;
			ContentView.BackgroundColor = UIColor.White;
			//ContentView.Transform = CGAffineTransform.MakeScale(0.8f, 0.8f);

			ImageView = new UIImageView();
			ContentView.AddSubview(ImageView);

			LabelView = new UILabel
			{
				TextColor = Color.BarBotBlue,
				TextAlignment = UITextAlignment.Center,
				AdjustsFontSizeToFitWidth = true
			};
			ContentView.AddSubview(LabelView);
		}

		public async void UpdateRow(Recipe element)
		{
			LabelView.Text = element.Name;
			ImageView.Image = await LoadImage(element.Img);

			//LabelView.Font = UIFont.FromName("HelveticaNeue-Bold", fontSize);

			CGPoint point = new CGPoint(ContentView.Frame.X, ContentView.Frame.Y);
			CGSize size = new CGSize(ContentView.Frame.Width, ContentView.Frame.Width * (3.0 / 4.0));

			ImageView.Frame = new CGRect(point, size);
			ImageView.Center = new CGPoint(ContentView.Center.X, ContentView.Center.Y);
			LabelView.Frame = new CGRect(ContentView.Frame.X,
			                             ContentView.Frame.Bottom - 35.0f,
			                             ContentView.Frame.Width,
			                             24.0f);
		}

		public async Task<UIImage> LoadImage(string imageUrl)
		{
			var httpClient = new HttpClient();

			// await! control returns to the caller and the task continues to run on another thread
			var contents = await httpClient.GetByteArrayAsync(imageUrl);

			// load from bytes
			return UIImage.LoadFromData(NSData.FromArray(contents));
		}
	}
}