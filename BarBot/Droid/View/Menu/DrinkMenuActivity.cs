using System.Threading.Tasks;

using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;

using GalaSoft.MvvmLight.Views;

using BarBot.Core.Model;
using BarBot.Core.ViewModel;
using BarBot.Core.WebSocket;

namespace BarBot.Droid.View.Menu
{
	[Activity(Label = "BarBot", MainLauncher = true)]
	public class DrinkMenuActivity : ActivityBase
	{
		MenuViewModel ViewModel => App.Locator.Menu;
		WebSocketUtil WebSocketUtil => App.WebSocketUtil;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "DrinkMenu" layout resource
			SetContentView(Resource.Layout.DrinkMenu);

			// Add Event Handlers
			WebSocketUtil.AddMenuEventHandlers(Socket_GetRecipesEvent, Socket_GetIngredientsEvent);

			//ShowNameDialog();
			App.ConnectWebSocket();

			ConfigureActionBar();
		}

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.top_menus, menu);
			return base.OnCreateOptionsMenu(menu);
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			// TODO: Implement Search
			Toast.MakeText(this, "Action selected: " + item.TitleFormatted,
				ToastLength.Short).Show();
			return base.OnOptionsItemSelected(item);
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
			var textviewTitle = (TextView)viewActionBar.FindViewById(Resource.Id.actionbar_textview);
			textviewTitle.Text = ViewModel.Title;
			abar.SetCustomView(viewActionBar, p);
			abar.SetDisplayShowCustomEnabled(true);
			abar.SetDisplayShowTitleEnabled(false);
		}

		void ConfigureGridView()
		{
			var gridview = FindViewById<GridView>(Resource.Id.gridview);
			gridview.Adapter = new GridAdapter(this);

			gridview.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs args)
			{
				var recipe = ViewModel.Recipes[args.Position];
				ViewModel.ShowDrinkDetailsCommand(recipe.RecipeId,
												  null);
			};
		}

		// USERNAME REGISTRATION

		// Show Name Text Alert
		void ShowNameDialog()
		{
			FragmentTransaction ft = FragmentManager.BeginTransaction();
			//Remove fragment else it will crash as it is already added to backstack
			Fragment prev = FragmentManager.FindFragmentByTag("nameDialog");
			if (prev != null)
			{
				ft.Remove(prev);
			}

			ft.AddToBackStack(null);

			// Create and show the dialog.
			NameDialogFragment newFragment = NameDialogFragment.NewInstance(null);

			//Add fragment
			newFragment.Show(ft, "nameDialog");
		}

		// EVENT HANDLERS

		async void Socket_GetRecipesEvent(object sender, WebSocketEvents.GetRecipesEventArgs args)
		{
			await Task.Run(() => RunOnUiThread(() =>
			{
				ViewModel.Recipes.Clear();
				//CollectionView.ReloadData();
				foreach (Recipe r in args.Recipes)
				{
					ViewModel.Recipes.Add(r);
				}
				ConfigureGridView();

				// Detach Event Handler
				WebSocketUtil.Socket.GetRecipesEvent -= Socket_GetRecipesEvent;
			}));
		}

		async void Socket_GetIngredientsEvent(object sender, WebSocketEvents.GetIngredientsEventArgs args)
		{
			await Task.Run(() => RunOnUiThread(() =>
			{
				App.IngredientsInBarBot.Clear();
				App.IngredientsInBarBot = args.Ingredients;

				// Detach Event Handler
				WebSocketUtil.Socket.GetIngredientsEvent -= Socket_GetIngredientsEvent;
			}));
		}
	}
}
