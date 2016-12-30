using System.Collections.Generic;
using System.Threading.Tasks;
using UIKit;
using CoreGraphics;
using BarBot.Core;
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

		public UIBarButtonItem RefreshButton
		{
			get;
			private set;
		}

		UIAlertAction ActionToEnable;
		UIButton ReconnectButton;
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

			// Init Search Controller
			InitSearchController();

			// Init Refresh Button
			InitRefreshButton();

			// Style Nav Bar
			SharedStyles.NavBarStyle(NavigationController.NavigationBar);

			source = new MenuSource();

			bindings.Add(
				this.SetBinding(
					() => ViewModel.Recipes,
					() => source.Rows));

			ConfigureReconnectButton();
			ConfigureCustomButton();

			// Init Collection View
			InitCollectionView();

			Delegate = (AppDelegate)UIApplication.SharedApplication.Delegate;

			// if new user
			if (Delegate.User.Uid == null)
			{
				ShowUserNameAlert();
			}
			else
			{
				ConnectWebSocket();
			}
		}

		public override void ViewWillAppear(bool animated)
		{
			if (!ViewModel.ShouldDisplaySearch)
			{
				DismissSearchController();
			}
		}

		// REFRESH BUTTON
		void InitRefreshButton()
		{
			RefreshButton = new UIBarButtonItem(UIBarButtonSystemItem.Refresh);
			NavigationItem.SetLeftBarButtonItem(RefreshButton, false);

			RefreshButton.Clicked += (sender, e) =>
			{
				ConnectWebSocket();
			};
		}

		void ConfigureReconnectButton()
		{
			ReconnectButton = new UIButton();
			ReconnectButton.SetTitle("RECONNECT", UIControlState.Normal);
			var height = 36;
			SharedStyles.StyleButtonText(ReconnectButton, height);

			var width = ReconnectButton.IntrinsicContentSize.Width;
			var x = (View.Frame.Width - width) / 2;
			var y = (View.Frame.Height - height) / 2 - 64;

			ReconnectButton.Frame = new CGRect(x, y, width, height);

			ReconnectButton.TouchUpInside += (sender, e) =>
			{
				ShowHostNameAlert();
			};
			CollectionView.Add(ReconnectButton);
		}

		// HOST NAME PROMPT
		void ShowHostNameAlert()
		{
			var hostNameAlertController = UIAlertController.Create("Please enter a host name", null, UIAlertControllerStyle.Alert);

			UITextField field = null;

			hostNameAlertController.AddTextField(textField =>
			{
				field = textField;
				field.Text = textField.Text;
				ConfigureKeyboard(field, "Host Name", UIKeyboardType.Default);
			});

			var submit = UIAlertAction.Create("Submit", UIAlertActionStyle.Default, (obj) => 
			{
				// ping host name
				if (field.Text != null)
				{
					// Save IP Address to User Defaults
    				Delegate.UserDefaults.SetString(field.Text, "HostName");

					// Store EndPoint in Delegate
					Delegate.HostName = field.Text;

					// Store in DB
					Delegate.UserDefaults.Synchronize();
				
 					if (Delegate.User.Uid == null)
					{
						ShowUserNameAlert();
					}
					else
					{
						ConnectWebSocket();
					}	
				}
				else
				{
					PresentViewController(hostNameAlertController, true, () =>
					{
						hostNameAlertController.Title = "Please enter a valid Host Name";
					});
				}
			});

			var cancel = UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, null);

			hostNameAlertController.AddAction(submit);
			hostNameAlertController.AddAction(cancel);
			ActionToEnable = submit;
			submit.Enabled = false;

			// Present Alert
			PresentViewController(hostNameAlertController, true, null);
		}

		// USERNAME REGISTRATION

		// Show Name Text Prompt
		void ShowUserNameAlert()
		{
			// Create Alert
			var nameInputAlertController = UIAlertController.Create("Please enter your name", null, UIAlertControllerStyle.Alert);

			UITextField field = null;

			// Add Text Input
			nameInputAlertController.AddTextField(textField =>
			{
				field = textField;
				field.Text = textField.Text;
				ConfigureKeyboard(field, "Your Name", UIKeyboardType.Default);
			});

			//  Add Actionn
			var submit = UIAlertAction.Create("Submit", UIAlertActionStyle.Default, async (obj) =>
			{
				var rest = new RestService(Delegate.HostName);
				var user = await rest.SaveUserNameAsync(field.Text);

				if (user.Uid.Equals("name_taken"))
				{
					PresentViewController(nameInputAlertController, true, () =>
					{
						nameInputAlertController.Title = "That name is taken";
					});
				}
				else if (user.Uid.Equals("exception"))
				{
					DismissViewController(true, null);
				}
				else
				{
					// Save value
					Delegate.UserDefaults.SetString(user.Uid, "UserId");

					// Set to Delegate
					Delegate.User = user;

					// Sync changes to database
					Delegate.UserDefaults.Synchronize();

					ConnectWebSocket();
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
				ConfigureKeyboard(field, "Drink Name", UIKeyboardType.Default);
			});

			var ok = UIAlertAction.Create("OK", UIAlertActionStyle.Default, (obj) =>
			{
				ViewModel.ShowDrinkDetailsCommand(field.Text, null);
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
		}

		// UITextField

		void ConfigureKeyboard(UITextField field, string placeholder, UIKeyboardType keyboardType)
		{
			field.Placeholder = placeholder;
			field.AutocorrectionType = UITextAutocorrectionType.No;
			field.AutocapitalizationType = UITextAutocapitalizationType.Words;
			field.EnablesReturnKeyAutomatically = true;
			field.KeyboardType = keyboardType;
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
				NavigationItem.SetLeftBarButtonItem(RefreshButton, true);
				Title = ViewModel.Title;
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

		void RefreshCollectionView(bool reconnectHidden, bool customHidden)
		{
			// Hide Reconnect Button
			ReconnectButton.Hidden = reconnectHidden;

			// Show Custom Button
			CustomButton.Hidden = customHidden;

			// Reload Collection View
			CollectionView.ReloadData();
		}

		// WEBSOCKET

		void ConnectWebSocket()
		{
			// Get WebSocketUtil from AppDelegate
			WebSocketUtil = Delegate.WebSocketUtil;

			if (WebSocketUtil != null)
			{
				// Close WebSocket if Reconnecting
				if (WebSocketUtil.Socket.IsOpen)
				{
					ViewModel.Recipes.Clear();
					WebSocketUtil.CloseWebSocket();
				}

				RefreshCollectionView(false, true);

				// Add Event Handlers
				WebSocketUtil.AddMenuEventHandlers(Socket_GetRecipesEvent, Socket_GetIngredientsEvent);

				// Open WebSocket
				WebSocketUtil.OpenWebSocket(Delegate.User.Uid, true);
			}
		}

		// EVENT HANDLERS

		private async void Socket_GetRecipesEvent(object sender, WebSocketEvents.GetRecipesEventArgs args)
		{
			await Task.Run(() => UIApplication.SharedApplication.InvokeOnMainThread(() =>
			{
				ViewModel.Recipes.Clear();
				CollectionView.ReloadData();
				foreach (Recipe r in args.Recipes)
				{
					ViewModel.Recipes.Add(r);
				}
				RefreshCollectionView(true, false);
			}));

			// Detach Event Handlerr
			WebSocketUtil.Socket.GetRecipesEvent -= Socket_GetRecipesEvent;
		}

		private async void Socket_GetIngredientsEvent(object sender, WebSocketEvents.GetIngredientsEventArgs args)
		{
			await Task.Run(() => UIApplication.SharedApplication.InvokeOnMainThread(() =>
			{
				Delegate.IngredientsInBarBot.Clear();
				Delegate.IngredientsInBarBot = args.Ingredients;
			}));

			// Detach Event Handler
			WebSocketUtil.Socket.GetIngredientsEvent -= Socket_GetIngredientsEvent;
		}
	}
}