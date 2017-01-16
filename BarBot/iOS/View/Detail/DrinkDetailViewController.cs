using System.Threading.Tasks;

using Foundation;
using UIKit;

using BarBot.Core;
using BarBot.Core.Model;
using BarBot.Core.ViewModel;
using BarBot.Core.WebSocket;

namespace BarBot.iOS.View.Detail
{
	public class DrinkDetailViewController : UIViewController
	{
		// ViewModel
		private DetailViewModel ViewModel => Application.Locator.Detail;

		// Data Properties
		AppDelegate Delegate;
		WebSocketUtil WebSocketUtil;

		// hax
		bool CreateCustomCalled = false;

		public DrinkDetailViewController()
		{
		}

		public override void LoadView()
		{
			base.LoadView();
			var frame = View.Frame;

			// Pass base frame to Custom View
			View = new DrinkDetailView(frame);
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			Delegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
			WebSocketUtil = Delegate.WebSocketUtil;

			// Add Event Handlers
			WebSocketUtil.AddDetailEventHandlers(Socket_GetRecipeDetailsEvent, Socket_OrderDrinkEvent, Socket_CreateCustomDrinkEvent);

			// Set Ingredients in BarBot
			ViewModel.IngredientsInBarBot = Delegate.IngredientsInBarBot;

			// Add Order Button Event Handler
			(View as DrinkDetailView).OrderButton.TouchUpInside += OrderButton_TouchUpInside;
		}

		public override void ViewWillAppear(bool animated)
		{
			// Custom Recipe
			if (ViewModel.RecipeId.Equals(Constants.CustomRecipeId))
			{
				(View as DrinkDetailView).NavBar.TopItem.Title = ViewModel.Recipe.Name.ToUpper();
				(View as DrinkDetailView).DrinkImageView.Image = UIImage.FromFile("Images/custom_recipe.png");
				(View as DrinkDetailView).OrderButton.Enabled = false;
				(View as DrinkDetailView).IngredientTableView.Editing = true;
				ViewModel.IsCustomRecipe = true;
				CreateCustomCalled = false;
			}
			else
			{
				WebSocketUtil.GetRecipeDetails(ViewModel.RecipeId);
			}
		}

		public override void ViewWillDisappear(bool animated)
		{
			// Clear ViewModel
			ViewModel.Clear();
		}

		// EVENT HANDLERS

		// GetRecipeDetails
		async void Socket_GetRecipeDetailsEvent(object sender, WebSocketEvents.GetRecipeDetailsEventArgs args)
		{
			await Task.Run(() => UIApplication.SharedApplication.InvokeOnMainThread(() =>
			{
				ViewModel.Recipe = args.Recipe;
				Reload();

				// Detach Event Handler
				WebSocketUtil.Socket.GetRecipeDetailsEvent -= Socket_GetRecipeDetailsEvent;
			}));
		}

		// Reload View after GetRecipeDetails
		async void Reload()
		{
			(View as DrinkDetailView).NavBar.TopItem.Title = ViewModel.Recipe.Name.ToUpper();
			(View as DrinkDetailView).IngredientTableView.ReloadSections(NSIndexSet.FromIndex(0), UITableViewRowAnimation.Automatic);

			if (ViewModel.ImageContents == null)
			{
				// load new Image
				ViewModel.ImageContents = await Delegate.RESTService.LoadImage(ViewModel.Recipe.Img);

				// Don't set on HTTP 404
				if (ViewModel.ImageContents == null)
				{
					(View as DrinkDetailView).DrinkImageView.Image = UIImage.LoadFromData(NSData.FromArray(ViewModel.ImageContents));
				}
			}
		}

		// OrderDrink
		async void Socket_OrderDrinkEvent(object sender, WebSocketEvents.OrderDrinkEventArgs args)
		{
			await Task.Run(() => UIApplication.SharedApplication.InvokeOnMainThread(() =>
			{
				ShowSucessAlert();

				// Detach Event Handler
				WebSocketUtil.Socket.OrderDrinkEvent -= Socket_OrderDrinkEvent;
			}));
		}

		// CreateCustomDrink
		async void Socket_CreateCustomDrinkEvent(object sender, WebSocketEvents.CreateCustomDrinkEventArgs args)
		{
			await Task.Run(() => UIApplication.SharedApplication.InvokeOnMainThread(() =>
			{
				// Flag to catch multiple calls
				if (!CreateCustomCalled)
				{
					WebSocketUtil.OrderDrink(args.RecipeId, 
					                         (View as DrinkDetailView).IceSwitch.On, 
					                         (View as DrinkDetailView).GarnishSwitch.On);

					// Detach Event Handler
					WebSocketUtil.Socket.CreateCustomDrinkEvent -= Socket_CreateCustomDrinkEvent;
					CreateCustomCalled = true;
				}
			}));
		}

		// Event Handler for OrderButton
		void OrderButton_TouchUpInside(object sender, System.EventArgs e)
		{
			if (ViewModel.IsCustomRecipe)
			{
				var recipe = new Recipe("", ViewModel.Recipe.Name, "", ViewModel.Ingredients);
				if (recipe.GetVolume() > Constants.MaxVolume)
				{
					ShowVolumeAlert();
				}
				else
				{
					WebSocketUtil.CreateCustomDrink(recipe);
				}
			}
			else
			{
				WebSocketUtil.OrderDrink(ViewModel.RecipeId,
										 (View as DrinkDetailView).IceSwitch.On,
										 (View as DrinkDetailView).GarnishSwitch.On);
			}
		}

		// UIAlertControllers

		// Creates and shows an AlertView prompt that:
		// 1. Thanks the user
		// 2. Allows the user to tap to return to the me
		public void ShowSucessAlert()
		{
			// Create Ale
			var successAlertController = UIAlertController.Create("Thank you for ordering!",
																  "Please approach the machine",
																  UIAlertControllerStyle.Alert);

			//  Add Acti
			successAlertController.AddAction(UIAlertAction.Create("Return to menu", UIAlertActionStyle.Default, action =>
			{
				ViewModel.ShowDrinkMenuCommand(false);
			}));

			// Present Ale
			PresentViewController(successAlertController, true, null);
		}

		// Prompt the user to reduce Recipe volume
		public void ShowVolumeAlert()
		{
			var volumeAlertController = UIAlertController.Create("Drink Too Large",
																  "Please reduce volume to " + Constants.MaxVolume + " oz",
																  UIAlertControllerStyle.Alert);

			var ok = UIAlertAction.Create("OK", UIAlertActionStyle.Default, action =>
			{
				volumeAlertController.DismissViewController(true, null);
			});

			volumeAlertController.AddAction(ok);

			PresentViewController(volumeAlertController, true, null);
		}
	}
}