﻿using System;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

using Calligraphy;

using Square.Picasso;

using BarBot.Core;
using BarBot.Core.Model;
using BarBot.Core.ViewModel;
using BarBot.Core.WebSocket;
using BarBot.Core.Service.WebSocket;
using BarBot.Droid.Utils;
using System.Linq;

namespace BarBot.Droid.View.Detail
{
    [Activity(Label = "DrinkDetailActivity")]
    public class DrinkDetailActivity : BaseActivity
    {
        RecipeDetailViewModel ViewModel => App.Locator.Detail;
        WebSocketService WebSocketService => App.WebSocketService;

        // UI Elements
        TextView TitleTextView;
        Switch IceSwitch;
        Button IceButton;
        Switch GarnishSwitch;
        Button GarnishButton;
        ListView ListView;

        // Add Ingredient Row
        Ingredient AddIngredientRow;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Prevent Rotation
            RequestedOrientation = Android.Content.PM.ScreenOrientation.Nosensor;

            // Set our view from the "DrinkDetail" layout resource
            SetContentView(Resource.Layout.DrinkDetail);

            ConfigureAppBar();
            ConfigureIceSwitch();
            ConfigureGarnishSwitch();
            ConfigureOrderButton();

            // Add Event Handlers
            WebSocketService.AddDetailEventHandlers(Socket_GetRecipeDetailsEvent, Socket_OrderDrinkEvent, Socket_CreateCustomRecipeEvent);

            // Set Ingredients in BarBot
            ViewModel.IngredientsInBarBot = App.IngredientsInBarBot;

            // Custom Recipe
            if (ViewModel.Recipe.RecipeId.Equals(Constants.CustomRecipeId))
            {
                // Get Custom Recipe Name
                ShowCustomNameDialog();

                // Set custom boolean
                ViewModel.IsCustomRecipe = true;
            }
            else
            {
                // Get RecipeDetails
                WebSocketService.GetRecipeDetails(ViewModel.Recipe.RecipeId);
            }
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

        protected override void OnStart()
        {
            s_activitycounter++;
            base.OnStart();
        }

        protected override void OnStop()
        {
            // Add Event Handlers
            WebSocketService.RemoveDetailEventHandlers(Socket_GetRecipeDetailsEvent, Socket_OrderDrinkEvent, Socket_CreateCustomRecipeEvent);

            // Clear ViewModel
            ViewModel.Clear();

            s_activitycounter--;
            base.OnPause();
        }

        protected override void OnResume()
        {
            base.OnResume();
        }

        protected override void AttachBaseContext(Context @base)
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
            drinkImageView.SetY(drinkImageView.GetY() - 15);

            // load drink image
            if (ViewModel.Recipe.RecipeId == Constants.CustomRecipeId)
            {
                Picasso.With(this).Load(Resource.Drawable.custom_recipe).Fit().CenterInside().Into(drinkImageView);
            }
            else
            {
                var url = "http://" + App.HostName + "/" + ViewModel.Recipe.Img;
                Picasso.With(this).Load(url).Fit().CenterInside().Into(drinkImageView);
            }

            // Hide Gradient
            var hexagonGradientImageView = hexagon.FindViewById<ImageView>(Resource.Id.hexagon_tile_gradient);
            hexagonGradientImageView.Visibility = ViewStates.Invisible;
        }

        void ConfigureListView()
        {
            AddIngredientRow = CreateNewAddIngredientRow();

            ViewModel.Recipe.Ingredients.Add(AddIngredientRow);

            ListView = (ListView)FindViewById(Resource.Id.ingredient_listview);
            ListView.Adapter = new IngredientAdapter(this, ViewModel.Recipe.Ingredients);
            ListView.ItemClick += ListView_ItemClick;
        }

        public void ReloadListView()
        {
            // add new AddIngredientRow if necessary
            if (ViewModel.Recipe.Ingredients.Count < ViewModel.IngredientsInBarBot.Count)
            {
                if (!ViewModel.Recipe.Ingredients.Contains(AddIngredientRow))
                {
                    AddIngredientRow = CreateNewAddIngredientRow();
                    ViewModel.Recipe.Ingredients.Add(AddIngredientRow);
                    (ListView.Adapter as IngredientAdapter).Insert(AddIngredientRow, ListView.Adapter.Count - 1);
                }
            }

            (ListView.Adapter as IngredientAdapter).NotifyDataSetChanged();
        }

        Ingredient CreateNewAddIngredientRow()
        {
            return new Ingredient(Constants.AddIngredientId,
                                            "Add Ingredient",
                                            0.5);
        }

        void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            ShowIngredientDialog(e.Position);
        }

        void ConfigureIceSwitch()
        {
            IceSwitch = FindViewById<Switch>(Resource.Id.iceswitch);
            IceButton = FindViewById<Button>(Resource.Id.icebutton);
            IceButton.Click += (sender, e) =>
            {
                IceSwitch.Checked = !IceSwitch.Checked;
            };
        }

        void ConfigureGarnishSwitch()
        {
            GarnishSwitch = FindViewById<Switch>(Resource.Id.garnishswitch);
            GarnishButton = FindViewById<Button>(Resource.Id.garnishbutton);
            GarnishButton.Click += (sender, e) =>
            {
                GarnishSwitch.Checked = !GarnishSwitch.Checked;
            };
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
                foreach (Ingredient i in ViewModel.Recipe.Ingredients)
                {
                    i.Name = App.IngredientsInBarBot.First(ingredient => ingredient.IngredientId == i.IngredientId).Name;
                }
                Reload();

                // Detach Event Handler
                WebSocketService.Socket.GetRecipeDetailsEvent -= Socket_GetRecipeDetailsEvent;
            }));
        }

        // Reload View after GetRecipeDetails
        public void Reload()
        {
            TitleTextView.Text = Helpers.UppercaseWords(ViewModel.Recipe.Name);
            ConfigureHexagon();
            ConfigureListView();

            // Set Available Ingredients
            ViewModel.RefreshAvailableIngredients();
        }

        // OrderDrink
        async void Socket_OrderDrinkEvent(object sender, WebSocketEvents.OrderDrinkEventArgs args)
        {
            await Task.Run(() => RunOnUiThread(() =>
            {
                ShowSuccessDialog();

                // Detach Event Handler
                WebSocketService.Socket.OrderDrinkEvent -= Socket_OrderDrinkEvent;
            }));
        }

        // CreateCustomDrink
        async void Socket_CreateCustomRecipeEvent(object sender, WebSocketEvents.CreateCustomRecipeEventArgs args)
        {
            await Task.Run(() => RunOnUiThread(() =>
            {
                WebSocketService.OrderDrink(args.RecipeId,
                                         IceSwitch.Checked,
                                         GarnishSwitch.Checked);

                // Detach Event Handler
                WebSocketService.Socket.CreateCustomRecipeEvent -= Socket_CreateCustomRecipeEvent;
            }));
        }

        // Event Handler for OrderButton
        void OrderButton_Click(object sender, EventArgs e)
        {
            if (ViewModel.Recipe.Ingredients.Contains(AddIngredientRow))
            {
                ViewModel.Recipe.Ingredients.Remove(AddIngredientRow);
            }

            if (ViewModel.IsCustomRecipe)
            {
                var recipe = new Recipe("", ViewModel.Recipe.Name, "", ViewModel.Recipe.Ingredients);
                if (recipe.GetVolume() > Constants.MaxVolume)
                {
                    ShowVolumeDialog();
                }
                else if (ViewModel.Recipe.Ingredients.Count == 0)
                {
                    ShowEmptyRecipeDialog();
                }
                else
                {
                    WebSocketService.CreateCustomRecipe(recipe);
                }
            }
            else
            {
                WebSocketService.OrderDrink(ViewModel.Recipe.RecipeId,
                                         IceSwitch.Checked,
                                         GarnishSwitch.Checked);
            }
        }

        // Alert Dialogs

        void ShowCustomNameDialog()
        {
            CustomNameDialogFragment customNameDialog = CustomNameDialogFragment.NewInstance(null);
            CreateDialog(customNameDialog, "customNameDialog");
        }

        void ShowIngredientDialog(int position)
        {
            IngredientDialogFragment ingredientDialog = IngredientDialogFragment.NewInstance(null, this, position);
            CreateDialog(ingredientDialog, "ingredientDialog");
        }

        // Show Volume Dialog
        void ShowVolumeDialog()
        {
            VolumeDialogFragment volumeDialog = VolumeDialogFragment.NewInstance(null);
            CreateDialog(volumeDialog, "volumeDialog");
        }

        // Show Empty RecipeD ialog
        void ShowEmptyRecipeDialog()
        {
            EmptyRecipeDialogFragment emptyRecipeDialog = EmptyRecipeDialogFragment.NewInstance(null);
            CreateDialog(emptyRecipeDialog, "emptyRecipeDialog");
        }

        // Show Success Dialog
        void ShowSuccessDialog()
        {
            SuccessDialogFragment successDialog = SuccessDialogFragment.NewInstance(null);
            CreateDialog(successDialog, "successDialog");
        }

        void CreateDialog(DialogFragment newFragment, string identifier)
        {
            FragmentTransaction ft = FragmentManager.BeginTransaction();

            //Remove fragment else it will crash as it is already added to backstack
            Fragment prev = FragmentManager.FindFragmentByTag(identifier);
            if (prev != null)
            {
                ft.Remove(prev);
            }

            ft.AddToBackStack(null);

            //Add fragment
            newFragment.Show(ft, identifier);
        }
    }
}
