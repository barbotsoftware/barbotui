using System;
using System.Net.Http;
using System.Threading.Tasks;
using Foundation;
using UIKit;
using CoreGraphics;
using BarBot.Core.Model;
using BarBot.Core.ViewModel;

namespace BarBot.iOS.View.Menu
{
	public class RecipeCollectionViewCell : UICollectionViewCell
	{
		public static NSString CellID = new NSString("RecipeCell");
		public UILabel NameLabel;
		public UIImageView DrinkImageView;
		public UIImageView HexagonImageView;
		MenuViewModel ViewModel => Application.Locator.Menu;
		string _recipeId;

		public RecipeCollectionViewCell(IntPtr handle) : base(handle)
		{
		}

		[Export("initWithFrame:")]
		public RecipeCollectionViewCell(CGRect Frame) : base(Frame)
		{
			ConfigureBackgroundView();
			ConfigureDrinkImage();
			ConfigureNameLabel();
		}

		void ConfigureBackgroundView()
		{
			var point = new CGPoint(ContentView.Frame.X, ContentView.Frame.Y);
			var size = new CGSize(ContentView.Frame.Width, ContentView.Frame.Height);

			HexagonImageView = new Hexagon("Images/HexagonTile.png",
										   point,
										   size);
			HexagonImageView.UserInteractionEnabled = true;
			HexagonImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
			HexagonImageView.Center = ContentView.Center;

			var tapGesture = new UITapGestureRecognizer(() => ViewModel.ShowDrinkDetailsCommand(_recipeId));
			HexagonImageView.AddGestureRecognizer(tapGesture);

			ContentView.AddSubview(HexagonImageView);
		}

		void ConfigureDrinkImage()
		{
			DrinkImageView = new UIImageView();
			DrinkImageView.ContentMode = UIViewContentMode.ScaleAspectFit;

			var point = new CGPoint(ContentView.Frame.X, ContentView.Frame.Y);
			var size = new CGSize(ContentView.Frame.Width, ContentView.Frame.Height);

			DrinkImageView.Frame = new CGRect(point, size);
			DrinkImageView.Center = ContentView.Center;

			ContentView.AddSubview(DrinkImageView);
		}

		void ConfigureNameLabel()
		{
			NameLabel = new UILabel
			{
				TextColor = UIColor.White,
				TextAlignment = UITextAlignment.Center,
				AdjustsFontSizeToFitWidth = true,
				Font = UIFont.FromName("Microsoft-Yi-Baiti", 26f)
			};

			NameLabel.Frame = new CGRect(ContentView.Frame.X,
										 ContentView.Frame.Y,
										 ContentView.Frame.Width,
										 ContentView.Frame.Height);

			ContentView.AddSubview(NameLabel);
		}

		public async void UpdateRow(Recipe element)
		{
			_recipeId = element.Id;
			NameLabel.Text = element.Name;
			DrinkImageView.Image = await LoadImage(element.Img);
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