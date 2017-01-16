using Foundation;
using UIKit;
using CoreGraphics;

using BarBot.iOS.Style;

namespace BarBot.iOS.View.Menu
{
	public class DrinkMenuView : UIView
	{
		public UIButton ReconnectButton;
		public UIButton CustomButton;
		public UICollectionView CollectionView;

		MenuSource source;

		[Export("initWithFrame:")]
		public DrinkMenuView(CGRect Frame) : base(Frame)
		{
			source = new MenuSource();

			ConfigureReconnectButton();
			ConfigureCustomButton();

			// Init Collection View
			InitCollectionView();
		}

		// RECONNECT BUTTON

		void ConfigureReconnectButton()
		{
			ReconnectButton = new UIButton();
			ReconnectButton.SetTitle("RECONNECT", UIControlState.Normal);
			var height = 36;
			SharedStyles.StyleButtonText(ReconnectButton, height);

			var width = ReconnectButton.IntrinsicContentSize.Width;
			var x = (Frame.Width - width) / 2;
			var y = (Frame.Height - height) / 2 - 64;

			ReconnectButton.Frame = new CGRect(x, y, width, height);

			//ReconnectButton.TouchUpInside += (sender, e) =>
			//{
			//	ShowHostNameAlert();
			//};
			CollectionView.Add(ReconnectButton);
		}

		// CUSTOM DRINK BUTTON

		void ConfigureCustomButton()
		{
			CustomButton = UIButton.FromType(UIButtonType.Custom);

			CustomButton.SetBackgroundImage(UIImage.FromFile("Images/CustomTile.png"), UIControlState.Normal);
			CustomButton.Frame = new CGRect((CollectionView.Bounds.Width / 2) - 21, 20, 199, 83);
			CustomButton.SetTitle("CUSTOM", UIControlState.Normal);
			SharedStyles.StyleButtonText(CustomButton, 26);

			//CustomButton.TouchUpInside += (sender, e) =>
			//{
			//	 ShowCustomAlertController();
			//};

			// hide button initially
			CustomButton.Hidden = true;
			CollectionView.AddSubview(CustomButton);
		}

		// COLLECTION VIEW

		// Initialize and Style Collection View
		void InitCollectionView()
		{
			CollectionView.RegisterClassForCell(typeof(DrinkCollectionViewCell), DrinkCollectionViewCell.CellID);
			CollectionView.ShowsHorizontalScrollIndicator = false;
			CollectionView.Source = source;
			CollectionView.BackgroundColor = Color.BackgroundGray;
		}

		void RefreshCollectionView(bool reconnectHidden, bool customHidden)
		{
			// Hide Reconnect Button
			ReconnectButton.Hidden = reconnectHidden;

			// Show Custom Button
			CustomButton.Hidden = customHidden;

			// Scroll Collection View to Top
			CollectionView.SetContentOffset(new CGPoint(-CollectionView.ContentOffset.X, -84), false);

			// Reload Collection View
			CollectionView.ReloadData();
		}
	}
}
