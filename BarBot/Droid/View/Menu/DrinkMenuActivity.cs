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
	[Activity(Label = "DrinkMenuActivity", MainLauncher = true)]
	public class DrinkMenuActivity : ActivityBase
	{
		private MenuViewModel ViewModel
		{
			get
			{
				return App.Locator.Menu;
			}
		}

		private WebSocketUtil WebSocketUtil
		{
			get
			{
				return App.WebSocketUtil;
			}
		}

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "DrinkMenu" layout resource
			SetContentView(Resource.Layout.DrinkMenu);

			ConfigureToolbar();
			ConnectWebSocket();
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

		void ConfigureToolbar()
		{
			// Activate Custom Toolbar
			var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);

			var mTitle = (TextView)toolbar.FindViewById(Resource.Id.toolbar_title);
			mTitle.Text = ViewModel.Title;

			SetActionBar(toolbar);
			ActionBar.Title = "";
		}

		// WEBSOCKET

		void ConnectWebSocket()
		{
			if (WebSocketUtil != null)
			{
				// Close WebSocket if Reconnecting
				if (WebSocketUtil.Socket.IsOpen)
				{
					ViewModel.Recipes.Clear();
					WebSocketUtil.CloseWebSocket();
				}

				//RefreshCollectionView(false, true);

				// Add Event Handlers
				WebSocketUtil.AddMenuEventHandlers(Socket_GetRecipesEvent, Socket_GetIngredientsEvent);

				// Open WebSocket
				WebSocketUtil.OpenWebSocket(App.User.Uid, true);
			}
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
				//RefreshCollectionView(true, false);

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
