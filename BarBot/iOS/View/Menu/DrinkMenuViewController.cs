using System.Collections.Generic;
using System.Threading.Tasks;
using UIKit;
using BarBot.Core.Model;
using BarBot.Core.WebSocket;
using BarBot.Core.ViewModel;
using BarBot.iOS.Util;
using BarBot.iOS.Util.WebSocket;
using BarBot.iOS.View.Menu.Search;

using GalaSoft.MvvmLight.Helpers;
using System;

namespace BarBot.iOS.View.Menu
{
	public class DrinkMenuViewController : UICollectionViewController
	{
		// Keep track of bindings to avoid premature garbage collection
		private readonly List<Binding> bindings = new List<Binding>();

		private MenuViewModel ViewModel => Application.Locator.Menu;

		DrinkSearchController searchController;

		public UIBarButtonItem SearchButton
		{
			get;
			private set;
		}

		AppDelegate Delegate;
		WebSocketUtil WebSocketUtil;
		MenuSource source;

		public DrinkMenuViewController(UICollectionViewLayout layout) : base(layout)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			Title = ViewModel.Title;
			InitSearchController();
			SharedStyles.NavBarStyle(NavigationController.NavigationBar);
			NavigationItem.BackBarButtonItem = new UIBarButtonItem("", UIBarButtonItemStyle.Plain, null);

			source = new MenuSource();

			bindings.Add(
				this.SetBinding(
					() => ViewModel.Recipes,
					() => source.Rows));

			InitCollectionView();

			Delegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
			WebSocketUtil = Delegate.WebSocketUtil;
			WebSocketUtil.AddMenuEventHandlers(Socket_GetRecipesEvent, Socket_GetIngredientsEvent);
			WebSocketUtil.OpenWebSocket();

			// if new user
			//ShowAlert();
		}

		public override void ViewWillAppear(bool animated)
		{
			if (!ViewModel.ShouldDisplaySearch)
			{
				DismissSearchController();
			}
		}

		// Show Name Text Prompt
		public void ShowAlert()
		{
			// Create Alert
			var nameInputAlertController = UIAlertController.Create("Enter your name", null, UIAlertControllerStyle.Alert);

			//Add Text Input
			nameInputAlertController.AddTextField(textField =>
			{
			});

			//  Add Actionn
			nameInputAlertController.AddAction(UIAlertAction.Create("Submit", UIAlertActionStyle.Default, null));

			// Present Alert
			PresentViewController(nameInputAlertController, true, null);
		}

		// Initialize and Style Collection View
		void InitCollectionView()
		{
			CollectionView.RegisterClassForCell(typeof(DrinkCollectionViewCell), DrinkCollectionViewCell.CellID);
			CollectionView.ShowsHorizontalScrollIndicator = false;
			CollectionView.Source = source;
			CollectionView.BackgroundColor = Color.BackgroundGray;
		}

		// Initialize Search Button and Controller
		void InitSearchController()
		{
			SearchButton = new UIBarButtonItem(UIBarButtonSystemItem.Search);
			NavigationItem.SetRightBarButtonItem(SearchButton, false);

			// Init Search ResultsController
			var searchResultsController = new DrinkSearchResultsViewController(ViewModel.Recipes);

			//add the search controller
			searchController = new DrinkSearchController(searchResultsController)
			{
				Delegate = new SearchControllerDelegate(DismissSearchController)
			};

			//Ensure the searchResultsController is presented in the current View Controller 
			DefinesPresentationContext = true;

			SearchButton.Clicked += (sender, e) => {
				searchController.ShowSearchBar(NavigationItem);
			};

			searchController.SearchBar.CancelButtonClicked += (sender, e) =>
			{
				DismissSearchController();
			};
		}

		public void DismissSearchController()
		{
			if (searchController.Active)
			{
				searchController.SearchResultsController.DismissViewController(true, null);
			}
			searchController.HideSearchBar(NavigationItem);
			searchController.SearchBar.Text = "";
			NavigationItem.SetRightBarButtonItem(SearchButton, true);
			Title = ViewModel.Title;
		}

		private async void Socket_GetRecipesEvent(object sender, WebSocketEvents.GetRecipesEventArgs args)
		{
			await Task.Run(() => UIApplication.SharedApplication.InvokeOnMainThread(() =>
			{
				foreach (Recipe r in args.Recipes)
				{
					ViewModel.Recipes.Add(r);
				}
				CollectionView.ReloadData();
			}));

			// Detach Event Handlerr
			WebSocketUtil.Socket.GetRecipesEvent -= Socket_GetRecipesEvent;
		}

		private async void Socket_GetIngredientsEvent(object sender, WebSocketEvents.GetIngredientsEventArgs args)
		{
			await Task.Run(() => UIApplication.SharedApplication.InvokeOnMainThread(() =>
			{
				Delegate.IngredientsInBarBot.Ingredients = args.Ingredients;
			}));

			// Detach Event Handler
			WebSocketUtil.Socket.GetIngredientsEvent -= Socket_GetIngredientsEvent;
		}

		public class SearchControllerDelegate : UISearchControllerDelegate
		{
			Action dismissSearchController;

			public SearchControllerDelegate(Action dismissSearchController)
			{
				this.dismissSearchController = dismissSearchController;
			}

			public override void WillDismissSearchController(UISearchController searchController)
			{
				dismissSearchController();
			}
		}	
	}
}