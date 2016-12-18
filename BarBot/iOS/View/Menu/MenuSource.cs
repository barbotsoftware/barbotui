using System;
using System.Collections.Generic;
using Foundation;
using UIKit;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using BarBot.Core.Model;
using BarBot.Core.ViewModel;

namespace BarBot.iOS.View.Menu
{
	// TODO: Change to ObservableCollectionViewSource
	public class MenuSource : UICollectionViewSource
	{
		private MenuViewModel ViewModel
		{
			get
			{
				return Application.Locator.Menu;
			}
		}
		public List<Recipe> Rows { get; private set; }

		public MenuSource()
		{
			Rows = ViewModel.Recipes;
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

			//ViewModel.NavigateCommand;
			//var nav = ServiceLocator.Current.GetInstance<INavigationService>();
			//nav.NavigateTo(ViewModelLocator.DrinkDetailKey);
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
