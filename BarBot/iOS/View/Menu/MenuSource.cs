using System;
using System.Collections.Generic;
using Foundation;
using UIKit;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using BarBot.Core.Model;

namespace BarBot.iOS.View.Menu
{
	public class MenuSource : UICollectionViewSource
	{
		public List<Recipe> Rows { get; private set; }
		//DrinkMenuViewController Controller;

		public MenuSource(DrinkMenuViewController c)
		{
			Rows = new List<Recipe>();
			//Controller = c;
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
			//var cell = (RecipeCell)collectionView.CellForItem(indexPath);
			//cell.ImageView.Alpha = 0.5f;
		}

		public override void ItemUnhighlighted(UICollectionView collectionView, NSIndexPath indexPath)
		{
		}

		public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
		{
			//var cell = (RecipeCollectionViewCell)collectionView.CellForItem(indexPath);
			//cell.ImageView.Alpha = 1;

			//Recipe row = Rows[indexPath.Row];

			var nav = ServiceLocator.Current.GetInstance<INavigationService>();
			nav.NavigateTo(AppDelegate.DrinkDetailKey);
		}

		public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
		{
			var cell = (RecipeCollectionViewCell)collectionView.DequeueReusableCell(RecipeCollectionViewCell.CellID, indexPath);

			Recipe row = Rows[indexPath.Row];

			cell.UpdateRow(row);

			return cell;
		}
	}
}
