using BarBot.Core;
using BarBot.Core.Model;
using BarBot.Core.WebSocket;
using BarBot.UWP.Websocket;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace BarBot.UWP.Pages
{

    public sealed partial class Menu : Page
    {
        private App app;
        private List<Category> categories = new List<Category>();
        private List<Recipe> recipes = new List<Recipe>();

        public List<Category> Categories
        {
            get { return categories; }
            set
            {
                categories = value;
                CategoryList.Categories = categories;
            }
        }

        public List<Recipe> Recipes
        {
            get { return recipes; }
            set
            {
                recipes = value;
                app.RecipesToFilter = recipes;
                RecipeList.Recipes = new ObservableCollection<Recipe>(recipes);
            }
        }

        public Menu()
        {
            this.InitializeComponent();
            this.app = Application.Current as App;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            CategoryList.Visibility = Visibility.Collapsed;
            RecipeList.Visibility = Visibility.Collapsed;
            AppBar.BackButtonVisible = false;

            // Clear filters
            this.app.ClearFilters();

            if (e.Parameter == null)
            {
                app.webSocketService.Socket.GetCategoriesEvent += Socket_GetCategoriesEvent;
                app.webSocketService.GetCategories();
                CategoryList.Visibility = Visibility.Visible;

                // Update Containers
                app.webSocketService.Socket.GetContainersEvent += Socket_GetContainersEvent;
                app.webSocketService.GetContainers();

                // Update Garnishes
                app.webSocketService.Socket.GetGarnishesEvent += Socket_GetGarnishesEvent;
                app.webSocketService.GetGarnishes();
            }
            else if (e.Parameter.GetType() == typeof(Dictionary<string, List<Category>>))
            {
                // Category Name is mapped to a List of Categories.
                // Grab the categoryName and set it as the Title,
                // and set Categories to the List, and show the CategoryList

                var dictionary = e.Parameter as Dictionary<string, List<Category>>;
                var categoryName = dictionary.Keys.First();

                HideProgressRing();

                Categories = dictionary[categoryName] as List<Category>;
                CategoryList.Visibility = Visibility.Visible;

                AppBar.Title = categoryName;
                AppBar.BackButtonVisible = true;
            }
            else if (e.Parameter.GetType() == typeof(Dictionary<string, List<Recipe>>))
            {   
                // Category Name is mapped to a List of Recipes.
                // Grab the categoryName and set it as the Title,
                // and set Recipes to the List, and show the RecipeList

                var dictionary = e.Parameter as Dictionary<string, List<Recipe>>;
                var categoryName = dictionary.Keys.First();

                if (categoryName.Equals(Constants.SearchCategoryName))
                {
                    AppBar.SearchButtonVisible = false;
                    AppBar.FilterButtonVisible = false;
                    AppBar.SettingsButtonVisible = false;
                }
                else
                {
                    AppBar.FilterButtonVisible = true;
                }

                HideProgressRing();

                Recipes = dictionary[categoryName] as List<Recipe>;
                RecipeList.Visibility = Visibility.Visible;

                AppBar.Title = categoryName;
                AppBar.BackButtonVisible = true;
            }
        }

        private async void Socket_GetCategoriesEvent(object sender, WebSocketEvents.GetCategoriesEventArgs args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High,
            () =>
            {
                HideProgressRing();
                args.Categories.Insert(0, Category.AllRecipes());
                Categories = args.Categories;
            });

            app.webSocketService.Socket.GetCategoriesEvent -= Socket_GetCategoriesEvent;
        }

        private async void Socket_GetContainersEvent(object sender, WebSocketEvents.GetContainersEventArgs args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High,
            () =>
            {
                app.Containers = args.Containers;
            });

            app.webSocketService.Socket.GetContainersEvent -= Socket_GetContainersEvent;
        }

        private async void Socket_GetGarnishesEvent(object sender, WebSocketEvents.GetGarnishesEventArgs args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High,
            () =>
            {
                app.Garnishes = args.Garnishes;
            });

            app.webSocketService.Socket.GetGarnishesEvent -= Socket_GetGarnishesEvent;
        }


        private void HideProgressRing()
        {
            if (ProgressRing.IsActive)
            {
                ProgressRing.IsActive = false;
                ProgressRing.Visibility = Visibility.Collapsed;
            }
        }
     }
}
