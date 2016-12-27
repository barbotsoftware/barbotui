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
using Foundation;

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

			// if new user
			if (Delegate.User.Uid == null)
			{
				ShowAlert();
			}
			else
			{
				InitWebSocketUtil();
			}
		}

		public override void ViewWillAppear(bool animated)
		{
			if (!ViewModel.ShouldDisplaySearch)
			{
				DismissSearchController();
			}
		}

		void InitWebSocketUtil()
		{
			WebSocketUtil = Delegate.WebSocketUtil;
			WebSocketUtil.AddMenuEventHandlers(Socket_GetRecipesEvent, Socket_GetIngredientsEvent);
			WebSocketUtil.OpenWebSocket(Delegate.User.Uid);
		}

		// Show Name Text Prompt
		public void ShowAlert()
		{
			// Create Alert
			var nameInputAlertController = UIAlertController.Create("Please enter your name", null, UIAlertControllerStyle.Alert);

			UITextField field = null;

			// Add Text Input
			nameInputAlertController.AddTextField(textField =>
			{
				field = textField;
				field.AutocorrectionType = UITextAutocorrectionType.No;
				field.KeyboardType = UIKeyboardType.Default;
				field.KeyboardAppearance = UIKeyboardAppearance.Dark;
				field.ReturnKeyType = UIReturnKeyType.Done;
				field.Text = textField.Text;
				field.Delegate = new TextFieldDelegate();
			});

			//  Add Actionn
			nameInputAlertController.AddAction(UIAlertAction.Create("Submit", UIAlertActionStyle.Default, async (obj) =>
			{
				// Get Shared User Defaults
				var plist = NSUserDefaults.StandardUserDefaults;

				var rest = new RestService();
				var user = await rest.SaveUserNameAsync(field.Text);

				if (user != null)
				{
					// Save value
					plist.SetString(user.Uid, "UserId");

					// Set to Delegate
					Delegate.User = user;

					// Sync changes to database
					plist.Synchronize();

					InitWebSocketUtil();
				}
				else
				{
					PresentViewController(nameInputAlertController, true, () =>
					{
						nameInputAlertController.Title = "That name is taken";
					});
				}
			}));

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

			SearchButton.Clicked += (sender, e) =>
			{
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
				NavigationItem.TitleView = null;
				searchController.SearchBar.Text = "";
				NavigationItem.SetRightBarButtonItem(SearchButton, true);
				Title = ViewModel.Title;
			}
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

		public class TextFieldDelegate : UITextFieldDelegate
		{
			public override bool ShouldChangeCharacters(UITextField textField, NSRange range, string replacementString)
			{
				string resultText = textField.Text.Substring(0, (int)range.Location) + replacementString + textField.Text.Substring((int)(range.Location + range.Length));
				return resultText.Length <= 32;
			}
		}
	}
}