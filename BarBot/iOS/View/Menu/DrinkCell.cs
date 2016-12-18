using System;
//using System.Net.Http;
//using System.Threading.Tasks;
using Foundation;
using UIKit;
using CoreGraphics;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using BarBot.Core.Model;
using BarBot.Core.ViewModel;

namespace BarBot.iOS.View.Menu
{
    public class RecipeCollectionViewCell : UICollectionViewCell
    {
		public static NSString CellID = new NSString("RecipeCell");
		public UILabel NameLabel;
		public UIImageView HexagonImageView;

        public RecipeCollectionViewCell (IntPtr handle) : base (handle)
        {
        }

		[Export("initWithFrame:")]
		public RecipeCollectionViewCell(CGRect Frame) : base(Frame)
		{
			ConfigureBackgroundView();
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

			UITapGestureRecognizer tapGesture = new UITapGestureRecognizer(Tapped);
			HexagonImageView.AddGestureRecognizer(tapGesture);

			ContentView.AddSubview(HexagonImageView);
		}

		void Tapped()
		{
			var nav = ServiceLocator.Current.GetInstance<INavigationService>();
			nav.NavigateTo(ViewModelLocator.DrinkDetailKey);
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

		public void UpdateRow(Recipe element)
		{
			NameLabel.Text = element.Name;
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

	class Hexagon : UIImageView
	{

		CGRect maskFrame;

		public Hexagon(string inside, CGPoint point, CGSize size)
		{
			Image = UIImage.FromFile(inside);
			Frame = new CGRect(point, size);;
			maskFrame = Frame;
		}

		public override bool PointInside(CGPoint point, UIEvent uievent)
		{
			var p = UIBezierPath.FromOval(maskFrame);
			return p.ContainsPoint(point);
		}
	}
}