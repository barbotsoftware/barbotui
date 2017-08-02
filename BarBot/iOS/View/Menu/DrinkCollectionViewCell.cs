using System;
using Foundation;
using UIKit;
using CoreGraphics;
using BarBot.Core.Model;
using BarBot.Core.ViewModel;

namespace BarBot.iOS.View.Menu
{
	public class DrinkCollectionViewCell : UICollectionViewCell
	{
		MenuViewModel ViewModel => Application.Locator.Menu;
		public static NSString CellID = new NSString("RecipeCell");
		public UILabel NameLabel;
		public UIImageView DrinkImageView;
		public UIImageView HexagonImageView;
		public UIImageView HexagonGradientImageView;

		string _recipeId;
		byte[] _imageContents;
		CGPoint point;
		CGSize size;

		public DrinkCollectionViewCell(IntPtr handle) : base(handle)
		{
		}

		[Export("initWithFrame:")]
		public DrinkCollectionViewCell(CGRect Frame) : base(Frame)
		{
			point = new CGPoint(ContentView.Frame.X, ContentView.Frame.Y);
			size = new CGSize(ContentView.Frame.Width, ContentView.Frame.Height);

			ConfigureBackgroundView();
			ConfigureDrinkImage();
			ConfigureGradientView();
			ConfigureNameLabel();
		}

		void ConfigureBackgroundView()
		{
			HexagonImageView = new Hexagon("Images/HexagonTile.png",
										   point,
										   size);
			HexagonImageView.UserInteractionEnabled = true;
			HexagonImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
			HexagonImageView.Center = ContentView.Center;

			var tapGesture = new UITapGestureRecognizer(() => ViewModel.ShowDrinkDetailsCommand(_recipeId, _imageContents));
			HexagonImageView.AddGestureRecognizer(tapGesture);

			ContentView.AddSubview(HexagonImageView);
		}

		void ConfigureDrinkImage()
		{
			DrinkImageView = new UIImageView();
			DrinkImageView.ContentMode = UIViewContentMode.ScaleAspectFit;

			var imgPoint = new CGPoint(point.X, point.Y - 10);
			DrinkImageView.Frame = new CGRect(imgPoint, size);

			ContentView.AddSubview(DrinkImageView);
		}

		void ConfigureGradientView()
		{
			HexagonGradientImageView = new Hexagon("Images/HexagonTileGradient.png",
												   point,
												   size);
			HexagonGradientImageView.UserInteractionEnabled = true;
			HexagonGradientImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
			HexagonGradientImageView.Center = ContentView.Center;

			var tapGesture = new UITapGestureRecognizer(() => ViewModel.ShowDrinkDetailsCommand(_recipeId, _imageContents));
			HexagonGradientImageView.AddGestureRecognizer(tapGesture);

			ContentView.AddSubview(HexagonGradientImageView);
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
			_recipeId = element.RecipeId;
			NameLabel.Text = element.Name;
			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;

			if (!ViewModel.ImageCache.ContainsKey(element.Name))
			{
				// Load new Image
				_imageContents = await appDelegate.RestService.LoadImage(element.Img);
				ViewModel.ImageCache.Add(element.Name, _imageContents);
			}
			else
			{
				// find in Image Cache
				_imageContents = ViewModel.ImageCache[element.Name];
			}

			if (_imageContents != null)
			{
				DrinkImageView.Image = UIImage.LoadFromData(NSData.FromArray(_imageContents));
			}
		}
	}
}