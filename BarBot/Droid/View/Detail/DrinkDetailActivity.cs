using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using GalaSoft.MvvmLight.Views;

using Square.Picasso;

using BarBot.Core;
using BarBot.Core.Model;
using BarBot.Core.ViewModel;
using BarBot.Core.WebSocket;

namespace BarBot.Droid.View.Detail
{
	[Activity(Label = "DrinkDetailActivity")]
	public class DrinkDetailActivity : ActivityBase
	{
		private DetailViewModel ViewModel => App.Locator.Detail;
		private WebSocketUtil WebSocketUtil => App.WebSocketUtil;

		// UI Elements
		TextView TitleTextView;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "DrinkMenu" layout resource
			SetContentView(Resource.Layout.DrinkDetail);

			ConfigureActionBar();

			// Add Event Handlers
			WebSocketUtil.AddDetailEventHandlers(Socket_GetRecipeDetailsEvent, Socket_OrderDrinkEvent, Socket_CreateCustomDrinkEvent);

			// Set Ingredients in BarBot
			ViewModel.IngredientsInBarBot = App.IngredientsInBarBot;

			// Get RecipeDetails
			WebSocketUtil.GetRecipeDetails(ViewModel.RecipeId);
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{
				// Respond to the action bar's Up/Home button
				case Android.Resource.Id.Home:
					ViewModel.ShowDrinkMenuCommand(false);
					return true;
				default:
					return base.OnOptionsItemSelected(item);
			}
		}

		// Configure UI

		void ConfigureActionBar()
		{
			//Customize the ActionBar

			ActionBar abar = ActionBar;
			Android.Views.View viewActionBar = LayoutInflater.Inflate(Resource.Layout.actionbar, null);
			var p = new ActionBar.LayoutParams(
					ViewGroup.LayoutParams.WrapContent,
					ViewGroup.LayoutParams.MatchParent,
					GravityFlags.Center);
			TitleTextView = (TextView)viewActionBar.FindViewById(Resource.Id.actionbar_textview);
			abar.SetCustomView(viewActionBar, p);
			abar.SetDisplayShowCustomEnabled(true);
			abar.SetDisplayShowTitleEnabled(false);
			abar.SetDisplayHomeAsUpEnabled(true);
		}

		void ConfigureHexagon()
		{
			var drinkImageView = (ImageView)FindViewById(Resource.Id.hexagon_drink_image); // place id of hexagon image here

			// get Recipe
			var recipe = ViewModel.Recipe;

			// load drink image
			//var url = "http://" + App.HostName + "/" + recipe.Img;
			//Picasso.With(this).Load(url).Into(drinkImageView);
		}

		// EVENT HANDLERS

		// GetRecipeDetails
		async void Socket_GetRecipeDetailsEvent(object sender, WebSocketEvents.GetRecipeDetailsEventArgs args)
		{
			await Task.Run(() => RunOnUiThread(() =>
			{
				ViewModel.Recipe = args.Recipe;
				Reload();

				// Detach Event Handler
				WebSocketUtil.Socket.GetRecipeDetailsEvent -= Socket_GetRecipeDetailsEvent;
			}));
		}

		// Reload View after GetRecipeDetails
		void Reload()
		{
			TitleTextView.Text = ViewModel.Recipe.Name.ToUpper();
			ConfigureHexagon();
			//(View as DrinkDetailView).IngredientTableView.ReloadSections(NSIndexSet.FromIndex(0), UITableViewRowAnimation.Automatic);

			// Set Available Ingredients
			ViewModel.RefreshAvailableIngredients();
		}

		// OrderDrink
		async void Socket_OrderDrinkEvent(object sender, WebSocketEvents.OrderDrinkEventArgs args)
		{
			await Task.Run(() => RunOnUiThread(() =>
			{
				//ShowSucessAlert();

				// Detach Event Handler
				WebSocketUtil.Socket.OrderDrinkEvent -= Socket_OrderDrinkEvent;
			}));
		}

		// CreateCustomDrink
		async void Socket_CreateCustomDrinkEvent(object sender, WebSocketEvents.CreateCustomDrinkEventArgs args)
		{
			await Task.Run(() => RunOnUiThread(() =>
			{
				// Flag to catch multiple calls
				//if (!CreateCustomCalled)
				//{
				//	WebSocketUtil.OrderDrink(args.RecipeId,
				//							 (View as DrinkDetailView).IceSwitch.On,
				//							 (View as DrinkDetailView).GarnishSwitch.On);

					// Detach Event Handler
					WebSocketUtil.Socket.CreateCustomDrinkEvent -= Socket_CreateCustomDrinkEvent;
				//	CreateCustomCalled = true;
				//}
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
					//ShowVolumeAlert();
				}
				else
				{
					WebSocketUtil.CreateCustomDrink(recipe);
				}
			}
			else
			{
				//WebSocketUtil.OrderDrink(ViewModel.RecipeId,
				//						 (View as DrinkDetailView).IceSwitch.On,
				//						 (View as DrinkDetailView).GarnishSwitch.On);
			}
		}
	}
}
