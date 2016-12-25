using System.Collections.Generic;
using System.Threading.Tasks;
using UIKit;
using CoreGraphics;
using BarBot.Core.Model;
using BarBot.Core.WebSocket;
using BarBot.Core.ViewModel;
using BarBot.iOS.Util;
using BarBot.iOS.Util.WebSocket;
using GalaSoft.MvvmLight.Helpers;

namespace BarBot.iOS.View.Menu
{
	public class DrinkMenuViewController : UICollectionViewController
	{
		// Keep track of bindings to avoid premature garbage collection
		private readonly List<Binding> bindings = new List<Binding>();

		private MenuViewModel ViewModel => Application.Locator.Menu;

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
			InitSearchButton();
			NavBarStyle(NavigationController.NavigationBar);
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

		// Style Navigation Bar
		void NavBarStyle(UINavigationBar NavBar)
		{
			NavBar.TintColor = UIColor.White;
			NavBar.BarTintColor = Color.NavBarGray;
			NavBar.TitleTextAttributes = new UIStringAttributes
			{
				ForegroundColor = UIColor.White,
				Font = UIFont.FromName("Microsoft-Yi-Baiti", 26f)
			};
			var NavBorder = new UIView(new CGRect(0,
												  NavBar.Frame.Size.Height - 1,
												  NavBar.Frame.Size.Width,
												  4));
			NavBorder.BackgroundColor = Color.BarBotBlue;
			NavBorder.Opaque = true;
			NavBar.AddSubview(NavBorder);
		}

		// Initialize and Style Collection View
		void InitCollectionView()
		{
			CollectionView.RegisterClassForCell(typeof(DrinkCollectionViewCell), DrinkCollectionViewCell.CellID);
			CollectionView.ShowsHorizontalScrollIndicator = false;
			CollectionView.Source = source;
			CollectionView.BackgroundColor = Color.BackgroundGray;
		}

		// Initialize Search Button
		void InitSearchButton()
		{
			SearchButton = new UIBarButtonItem(UIBarButtonSystemItem.Search);
			this.NavigationItem.SetRightBarButtonItem(SearchButton, false);

			SearchButton.Clicked += (sender, e) => { };

			//SearchButton.SetCommand("Clicked", ViewModel.SearchCommand);
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
	}
}