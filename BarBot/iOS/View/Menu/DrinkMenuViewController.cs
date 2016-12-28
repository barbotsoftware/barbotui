using System.Collections.Generic;
using System.Threading.Tasks;
using UIKit;
using CoreGraphics;
using BarBot.Core.Model;
using BarBot.Core.WebSocket;
using BarBot.Core.ViewModel;
using BarBot.iOS.Util;
using BarBot.iOS.View.Menu.Search;

using GalaSoft.MvvmLight.Helpers;
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

		UIAlertAction ActionToEnable;
		UIButton CustomButton;

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

			ConfigureCustomButton();
			InitCollectionView();

			Delegate = (AppDelegate)UIApplication.SharedApplication.Delegate;

			// if new user
			if (Delegate.User.Uid == null)
			{
				ShowUserNameAlert();
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

		// COLLECTION VIEW

		// Initialize and Style Collection View
		void InitCollectionView()
		{
			CollectionView.RegisterClassForCell(typeof(DrinkCollectionViewCell), DrinkCollectionViewCell.CellID);
			CollectionView.ShowsHorizontalScrollIndicator = false;
			CollectionView.Source = source;
			CollectionView.BackgroundColor = Color.BackgroundGray;
		}

		// USERNAME REGISTRATION

		// Show Name Text Prompt
		public void ShowUserNameAlert()
		{
			// Create Alert
			var nameInputAlertController = UIAlertController.Create("Please enter your name", null, UIAlertControllerStyle.Alert);

			UITextField field = null;

			// Add Text Input
			nameInputAlertController.AddTextField(textField =>
			{
				field = textField;
				ConfigureKeyboard(field, "Your Name");
				field.Text = textField.Text;
			});

			//  Add Actionn
			var submit = UIAlertAction.Create("Submit", UIAlertActionStyle.Default, async (obj) =>
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
			});

			nameInputAlertController.AddAction(submit);
			ActionToEnable = submit;
			submit.Enabled = false;

			// Present Alert
			PresentViewController(nameInputAlertController, true, null);
		}

		// CUSTOM DRINK BUTTON

		void ConfigureCustomButton()
		{
			CustomButton = UIButton.FromType(UIButtonType.Custom);

			CustomButton.SetBackgroundImage(UIImage.FromFile("Images/CustomTile.png"), UIControlState.Normal);
			CustomButton.Frame = new CGRect((CollectionView.Bounds.Width / 2) - 21, 20, 199, 83);
			CustomButton.SetTitle("Custom", UIControlState.Normal);
			SharedStyles.StyleButtonText(CustomButton, 26);

			CustomButton.TouchUpInside+= (sender, e) =>
			{
				ShowCustomAlertController();
			};

			// hide button initially
			CustomButton.Hidden = true;
			CollectionView.AddSubview(CustomButton);
		}

		// Display alert popup when adding a new (custom) drink.
		// Prompts user to enter a name, and adds it to the menu.
		void ShowCustomAlertController()
		{
			var alertController = UIAlertController.Create("Name Your Drink", null, UIAlertControllerStyle.Alert);

			UITextField field = null;

			// Configure text view
			alertController.AddTextField(textField =>
			{
				field = textField;
				field.Text = textField.Text;
				ConfigureKeyboard(field, "Drink Name");
			});

			var ok = UIAlertAction.Create("OK", UIAlertActionStyle.Default, (obj) =>
			{
				//ViewModel.ShowDrinkDetailsCommand(null, null)
				System.Diagnostics.Debug.WriteLine(field.Text);	
			});

			var cancel = UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, (obj) =>
			{
				alertController.DismissViewController(true, null);
			});

			alertController.AddAction(ok);
			alertController.AddAction(cancel);

			ActionToEnable = ok;
			ok.Enabled = false;
			PresentViewController(alertController, true, null);

			//// OK button
			//let ok: UIAlertAction = UIAlertAction.init(title: "OK", style: .Default, handler: { (action: UIAlertAction) in
			//    let field = alert.textFields!.first!
			//    let customDrink: Drink = Drink.init(name: field.text!)
			//    self.drinkList.insert(customDrink, atIndex: 0)
			//    let newIndexPath: NSIndexPath = NSIndexPath(forRow:0, inSection:0)
			//    self.tableView.insertRowsAtIndexPaths([newIndexPath], withRowAnimation: .Top)
			//}
		}

		// UITextField

		void ConfigureKeyboard(UITextField field, string placeholder)
		{
			field.Placeholder = placeholder;
			field.AutocorrectionType = UITextAutocorrectionType.No;
			field.EnablesReturnKeyAutomatically = true;
			field.KeyboardType = UIKeyboardType.Default;
			field.KeyboardAppearance = UIKeyboardAppearance.Dark;
			field.ReturnKeyType = UIReturnKeyType.Default;
			field.Delegate = new TextFieldDelegate();
			field.AddTarget((sender, e) => { TextChanged(field); }, UIControlEvent.EditingChanged);
		}

		// Enable Action when Text is not empty
		void TextChanged(UITextField sender)
		{
			ActionToEnable.Enabled = (sender.Text != "");
    	}

		public class TextFieldDelegate : UITextFieldDelegate
		{
			public override bool ShouldChangeCharacters(UITextField textField, NSRange range, string replacementString)
			{
				string resultText = textField.Text.Substring(0, (int)range.Location) + replacementString + textField.Text.Substring((int)(range.Location + range.Length));
				return resultText.Length <= 32;
			}
		}

		// SEARCH

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

		// WEBSOCKET

		void InitWebSocketUtil()
		{
			WebSocketUtil = Delegate.WebSocketUtil;
			WebSocketUtil.AddMenuEventHandlers(Socket_GetRecipesEvent, Socket_GetIngredientsEvent);
			WebSocketUtil.OpenWebSocket(Delegate.User.Uid, true);

			// show custom button
			CustomButton.Hidden = false;
		}

		// EVENT HANDLERS

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
	}
}