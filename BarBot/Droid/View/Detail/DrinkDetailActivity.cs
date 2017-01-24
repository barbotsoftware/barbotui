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

using Calligraphy;

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
		DetailViewModel ViewModel => App.Locator.Detail;
		WebSocketUtil WebSocketUtil => App.WebSocketUtil;

		// UI Elements
		TextView TitleTextView;
		Switch IceSwitch;
		Switch GarnishSwitch;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "DrinkMenu" layout resource
			SetContentView(Resource.Layout.DrinkDetail);

			ConfigureAppBar();
			ConfigureIceSwitch();
			ConfigureGarnishSwitch();
			ConfigureOrderButton();

			// Add Event Handlers
			WebSocketUtil.AddDetailEventHandlers(Socket_GetRecipeDetailsEvent, Socket_OrderDrinkEvent, Socket_CreateCustomDrinkEvent);

			// Set Ingredients in BarBot
			ViewModel.IngredientsInBarBot = App.IngredientsInBarBot;

			// Get RecipeDetails
			WebSocketUtil.GetRecipeDetails(ViewModel.RecipeId);
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

			// Clear ViewModel
			ViewModel.Clear();
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

		protected override void AttachBaseContext(Android.Content.Context @base)
		{
			base.AttachBaseContext(CalligraphyContextWrapper.Wrap(@base));
		}

		// Configure UI

		void ConfigureActionBar()
		{
			//Customize the ActionBar

			ActionBar abar = ActionBar;
			Android.Views.View viewActionBar = LayoutInflater.Inflate(Resource.Layout.ActionBar, null);
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

		void ConfigureAppBar()
		{
			var appBar = (Toolbar)FindViewById(Resource.Id.toolbar); // Attaching the layout to the toolbar object
			SetActionBar(appBar);

			TitleTextView = (TextView)appBar.FindViewById(Resource.Id.toolbar_textview);

			ActionBar.SetDisplayShowTitleEnabled(false);
			ActionBar.SetDisplayHomeAsUpEnabled(true);
		}

		void ConfigureHexagon()
		{
			// get layout from hexagon.xml
			var hexagon = FindViewById(Resource.Id.hexagon);

			var hexagonImageView = hexagon.FindViewById<ImageView>(Resource.Id.hexagon_tile);
			var drinkImageView = hexagon.FindViewById<ImageView>(Resource.Id.hexagon_drink_image);

			// resize drink imageview
			drinkImageView.LayoutParameters = hexagonImageView.LayoutParameters;

			// load drink image
			var url = "http://" + App.HostName + "/" + ViewModel.Recipe.Img;
			Picasso.With(this).Load(url).Fit().CenterInside().Into(drinkImageView);

			// Hide Gradient
			var hexagonGradientImageView = hexagon.FindViewById<ImageView>(Resource.Id.hexagon_tile_gradient);
			hexagonGradientImageView.Visibility = ViewStates.Invisible;
		}

		void ConfigureListView()
		{
			ArrayAdapter adapter = new ArrayAdapter<Ingredient>(this, 
			                                                    Resource.Layout.ListViewRow,
			                                                    ViewModel.Recipe.Ingredients);

			ListView listView = (ListView)FindViewById(Resource.Id.ingredient_listview);
			listView.Adapter = adapter;
		}

		void ConfigureIceSwitch()
		{
			IceSwitch = FindViewById<Switch>(Resource.Id.iceswitch);
		}

		void ConfigureGarnishSwitch()
		{
			GarnishSwitch = FindViewById<Switch>(Resource.Id.garnishswitch);
		}

		void ConfigureOrderButton()
		{
			var orderButton = FindViewById<Button>(Resource.Id.orderbutton);
			orderButton.Click += OrderButton_Click;
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
			ConfigureListView();
			//(View as DrinkDetailView).IngredientTableView.ReloadSections(NSIndexSet.FromIndex(0), UITableViewRowAnimation.Automatic);

			// Set Available Ingredients
			ViewModel.RefreshAvailableIngredients();
		}

		// OrderDrink
		async void Socket_OrderDrinkEvent(object sender, WebSocketEvents.OrderDrinkEventArgs args)
		{
			await Task.Run(() => RunOnUiThread(() =>
			{
				ShowSuccessAlert();

				// Detach Event Handler
				WebSocketUtil.Socket.OrderDrinkEvent -= Socket_OrderDrinkEvent;
			}));
		}

		// CreateCustomDrink
		async void Socket_CreateCustomDrinkEvent(object sender, WebSocketEvents.CreateCustomDrinkEventArgs args)
		{
			await Task.Run(() => RunOnUiThread(() =>
			{				
				WebSocketUtil.OrderDrink(args.RecipeId,
										 IceSwitch.Checked,
										 GarnishSwitch.Checked);

				// Detach Event Handler
				WebSocketUtil.Socket.CreateCustomDrinkEvent -= Socket_CreateCustomDrinkEvent;
			}));
		}

		// Event Handler for OrderButton
		void OrderButton_Click(object sender, System.EventArgs e)
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
										 IceSwitch.Checked,
										 GarnishSwitch.Checked);
			}
		}

		// Alert Dialogs

		// Show Volume Alert
		void ShowVolumeAlert()
		{
			FragmentTransaction ft = FragmentManager.BeginTransaction();

			//Remove fragment else it will crash as it is already added to backstack
			Fragment prev = FragmentManager.FindFragmentByTag("volumeDialog");
			if (prev != null)
			{
				ft.Remove(prev);
			}

			ft.AddToBackStack(null);

			// Create and show the dialog.
			VolumeDialogFragment newFragment = VolumeDialogFragment.NewInstance(null);

			//Add fragment
			newFragment.Show(ft, "volumeDialog");
		}

		// Show Success Alert
		void ShowSuccessAlert()
		{
			FragmentTransaction ft = FragmentManager.BeginTransaction();

			//Remove fragment else it will crash as it is already added to backstack
			Fragment prev = FragmentManager.FindFragmentByTag("successDialog");
			if (prev != null)
			{
				ft.Remove(prev);
			}

			ft.AddToBackStack(null);

			// Create and show the dialog.
			SuccessDialogFragment newFragment = SuccessDialogFragment.NewInstance(null);

			//Add fragment
			newFragment.Show(ft, "successDialog");
		}
	}
}
