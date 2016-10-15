using System;
using System.Collections.Generic;
using Foundation;
using UIKit;
using CoreGraphics;
using BarBot.Model;

namespace BarBot.iOS.Menu
{
	public class MenuSource : UICollectionViewSource
	{
		public List<Recipe> Rows { get; private set; }

		public MenuSource()
		{
			Rows = new List<Recipe>();
		}

		public override nint NumberOfSections(UICollectionView collectionView)
		{
			return 1;
		}

		public override nint GetItemsCount(UICollectionView collectionView, nint section)
		{
			return Rows.Count;
		}

		public override bool ShouldHighlightItem(UICollectionView collectionView, NSIndexPath indexPath)
		{
			return true;
		}

		public override void ItemHighlighted(UICollectionView collectionView, NSIndexPath indexPath)
		{
			var cell = (RecipeCell)collectionView.CellForItem(indexPath);
			cell.ImageView.Alpha = 0.5f;
		}

		public override void ItemUnhighlighted(UICollectionView collectionView, NSIndexPath indexPath)
		{
			var cell = (RecipeCell)collectionView.CellForItem(indexPath);
			cell.ImageView.Alpha = 1;

			Recipe row = Rows[indexPath.Row];
		}

		public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
		{
			var cell = (RecipeCell)collectionView.DequeueReusableCell(RecipeCell.CellID, indexPath);

			Recipe row = Rows[indexPath.Row];

			cell.UpdateRow(row);

			return cell;
		}
	}
}
