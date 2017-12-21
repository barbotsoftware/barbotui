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
        private UWPWebSocketService webSocketService;
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
            webSocketService = app.webSocketService;
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
                webSocketService.Socket.GetCategoriesEvent += Socket_GetCategoriesEvent;
                webSocketService.GetCategories();
                CategoryList.Visibility = Visibility.Visible;
            }
            else if (e.Parameter.GetType() == typeof(Dictionary<string, List<Category>>))
            {
                // Category Name is mapped to a List of Categories.
                // Grab the categoryName and set it as the Title,
                // and set Categories to the List, and show the CategoryList

                var dictionary = e.Parameter as Dictionary<string, List<Category>>;
                var categoryName = dictionary.Keys.First();
                
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
                args.Categories.Insert(0, Category.AllRecipes());
                Categories = args.Categories;
            });

            webSocketService.Socket.GetCategoriesEvent -= Socket_GetCategoriesEvent;
        }
    }
}
