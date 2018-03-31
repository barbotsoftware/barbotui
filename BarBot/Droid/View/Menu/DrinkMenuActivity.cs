using System.Threading.Tasks;

using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;

using Calligraphy;

using BarBot.Core.Model;
using BarBot.Core.ViewModel;
using BarBot.Core.WebSocket;
using BarBot.Core.Service.WebSocket;
using BarBot.Droid.Utils;

namespace BarBot.Droid.View.Menu
{
	[Activity(Label = "BarBot", MainLauncher = true)]
	public class DrinkMenuActivity : BaseActivity
	{
		MenuViewModel ViewModel => App.Locator.Menu;
        WebSocketService WebSocketService => App.WebSocketService;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			s_mainactivityvisible = true;

			// Prevent Rotation
			RequestedOrientation = Android.Content.PM.ScreenOrientation.Nosensor;

			// Set our view from the "DrinkMenu" layout resource
			SetContentView(Resource.Layout.DrinkMenu);

			// Add Event Handlers
            WebSocketService.AddMenuEventHandlers(Socket_GetRecipesEvent, Socket_GetIngredientsEvent);

			// Attempt to load UserId from SharedPrefs
			App.LoadSharedPreferences();

			if (App.User.Name == "")
			{
				ShowNameDialog();
			}
			else
			{
				App.ConnectWebSocket();
                WebSocketService.GetRecipes();
                WebSocketService.GetIngredients();
			}

			ConfigureAppBar();
            ConfigureGridView();
		}

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.top_menus, menu);
			return base.OnCreateOptionsMenu(menu);
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
            switch (item.TitleFormatted.ToString()) {
                case "Containers":
                    ViewModel.ShowContainersCommand();
                    break;
                default:
                    break;
            }
			return base.OnOptionsItemSelected(item);
		}

		protected override void OnStart()
		{
			s_mainactivityvisible = true;
			base.OnStart();
		}

		// Save SharedPreferences On Stop
		protected override void OnStop()
		{
			s_mainactivityvisible = false;
			base.OnPause();
		}

		protected override void OnResume()
		{
			base.OnResume();
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
			var textviewTitle = (TextView)viewActionBar.FindViewById(Resource.Id.actionbar_textview);
			textviewTitle.Text = ViewModel.Title;
			abar.SetCustomView(viewActionBar, p);
			abar.SetDisplayShowCustomEnabled(true);
			abar.SetDisplayShowTitleEnabled(false);
		}

		void ConfigureAppBar()
		{
			var appBar = (Toolbar)FindViewById(Resource.Id.toolbar); // Attaching the layout to the toolbar object
			SetActionBar(appBar);
			
			var textviewTitle = (TextView)appBar.FindViewById(Resource.Id.toolbar_textview);
			textviewTitle.Text = ViewModel.Title;

			ActionBar.SetDisplayShowTitleEnabled(false);
		}

        GridView gridview;

		void ConfigureGridView()
		{
			gridview = FindViewById<GridView>(Resource.Id.gridview);
			gridview.Adapter = new GridAdapter(this);

			//// Add Progress Bar
			//ProgressBar progressBar = new ProgressBar(this);
			//progressBar.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WrapContent,
			//														  ViewGroup.LayoutParams.WrapContent);
			//progressBar.Indeterminate = true;
			//gridview.EmptyView = progressBar;

			//// Must add the progress bar to the root of the layout
			//ViewGroup root = (ViewGroup)FindViewById(Android.Resource.Id.Content);
			//root.AddView(progressBar);

            gridview.ItemClick += GridView_ItemClick;
		}

        void GridView_ItemClick(object sender, AdapterView.ItemClickEventArgs args)
        {
            var recipe = ViewModel.Recipes[args.Position];
            ViewModel.ShowDrinkDetailsCommand(recipe.RecipeId,
                                              null);
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
			NameDialogFragment newFragment = NameDialogFragment.NewInstance(null, this);

			//Add fragment
			newFragment.Show(ft, "nameDialog");
		}

		// EVENT HANDLERS

		async void Socket_GetRecipesEvent(object sender, WebSocketEvents.GetRecipesEventArgs args)
		{
            await Task.Run(() => RunOnUiThread(() =>
            {
                ViewModel.Recipes.Clear();

                // Add Custom Recipe
                var customRecipe = Recipe.CustomRecipe();
				ViewModel.Recipes.Add(customRecipe);

				foreach (Recipe r in args.Recipes)
				{
					ViewModel.Recipes.Add(r);
				}

				// Detach Event Handler
                WebSocketService.Socket.GetRecipesEvent -= Socket_GetRecipesEvent;
			}));
		}

		async void Socket_GetIngredientsEvent(object sender, WebSocketEvents.GetIngredientsEventArgs args)
		{
			await Task.Run(() => RunOnUiThread(() =>
			{
				App.IngredientsInBarBot.Clear();
				App.IngredientsInBarBot = args.Ingredients;

                foreach (Ingredient i in App.IngredientsInBarBot) 
                {
                    i.Name = Helpers.UppercaseWords(i.Name);
                }

				// Detach Event Handler
                WebSocketService.Socket.GetIngredientsEvent -= Socket_GetIngredientsEvent;
			}));
		}
	}
}
