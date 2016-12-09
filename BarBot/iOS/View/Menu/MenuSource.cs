using System;
using System.Collections.Generic;
using Foundation;
using UIKit;
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

			//UIStoryboard Storyboard = UIStoryboard.FromName("Order", null);

			//Recipe row = Rows[indexPath.Row];
			//RecipeViewModel viewModel = new RecipeViewModel(row);
			//var recipeDetailViewController = Storyboard.InstantiateInitialViewController() as RecipeDetailViewController;
			//recipeDetailViewController.ViewModel = viewModel;
			//Controller.NavigationController.PushViewController(recipeDetailViewController, true);
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
