using System;
using UIKit;
using CoreGraphics;

namespace BarBot.iOS.Menu
{
	public class GridLayout : UICollectionViewFlowLayout
	{
		public GridLayout()
		{
			CGRect screenRect = UIScreen.MainScreen.Bounds;
			nfloat screenWidth = screenRect.Size.Width;
			nfloat screenHeight = screenRect.Size.Height;
			nfloat cellWidth = screenWidth / 2.0f;
			nfloat cellHeight = (screenHeight / 2.0f) - 50.0f;

			ItemSize = new CGSize(cellWidth, cellHeight);
			SectionInset = new UIEdgeInsets(0, 0, 0, 0);
			MinimumInteritemSpacing = 0.0f;
			MinimumLineSpacing = 0.0f;
		}
	}
}
