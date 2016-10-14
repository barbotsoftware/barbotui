using System;
using CoreGraphics;
using Foundation;
using UIKit;
using BarBot.Model;
using System.ComponentModel;
using System.Drawing;

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
			SelectedBackgroundView = new UIView { BackgroundColor = UIColor.Green };

			ContentView.Layer.BorderColor = Color.BarBotBlue.CGColor;
			ContentView.Layer.BorderWidth = 2.0f;
			ContentView.BackgroundColor = UIColor.White;
			ContentView.Transform = CGAffineTransform.MakeScale(0.8f, 0.8f);

			ImageView = new UIImageView 
			{
				ContentMode = UIViewContentMode.ScaleToFill	
			};


			ContentView.AddSubview(ImageView);

			LabelView = new UILabel
			{
				TextColor = Color.BarBotBlue,
				TextAlignment = UITextAlignment.Center
			};

			ContentView.AddSubview(LabelView);
		}

		public void UpdateRow(Recipe element, float fontSize, SizeF imageViewSize)
		{
			LabelView.Text = element.Name;
			//ImageView.Image = element.Img; load image from server

			LabelView.Font = UIFont.FromName("HelveticaNeue-Bold", fontSize);

			ImageView.Frame = new CGRect(0, 0, imageViewSize.Width, imageViewSize.Height);
			LabelView.Frame = new CGRect(0, ImageView.Frame.Bottom, imageViewSize.Width,
											 ContentView.Frame.Height - ImageView.Frame.Bottom);
		}
	}
}
