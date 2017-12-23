using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using BarBot.Core;
using BarBot.Core.Model;
using BarBot.UWP.Websocket;
using BarBot.UWP.Utils;
using System.Collections.ObjectModel;

namespace BarBot.UWP.UserControls.RecipeList
{
    public sealed partial class Uc_RecipeList : UserControl
    {
        private App app;
        private UWPWebSocketService webSocketService;
        private ObservableCollection<Recipe> recipes;
        
        private int page = 0;
        private int itemsPerPage = 10;
        private int pages = 1;

        public ObservableCollection<Recipe> Recipes
        {
            get { return recipes; }
            set
            {
                // set recipes
                recipes = value;

                DisplayNoCocktailsFound(recipes);
                
                // calculate page count, and reset current page to 0
                pages = (recipes.Count + itemsPerPage - 1) / itemsPerPage;
                Page = 0;
            }
        }

        public int Page
        {
            get { return page; }
            set
            {
                page = value;
                displayPage(page);
            }
        }

        public Uc_RecipeList()
        {
            this.InitializeComponent();

            app = Application.Current as App;
            webSocketService = app.webSocketService;

            app.FilterApplied += App_FilterApplied;
        }

        private void displayPage(int page)
        {
            // Hide BackButton if it's the first page, hide NextButton if its the last page
            BackButton.Visibility = page == 0 ? Visibility.Collapsed : Visibility.Visible;
            NextButton.Visibility = page >= pages - 1 ? Visibility.Collapsed : Visibility.Visible;

            // clear out the current recipe tiles
            recipeTileCanvas.Children.Clear();

            for (int i = 0 + (itemsPerPage * page); i < Math.Min((itemsPerPage * page) + itemsPerPage, recipes.Count); i++)
            {
                Tc_RecipeTile tile = new Tc_RecipeTile();
                tile.Recipe = recipes[i];
                Point pos = Helpers.GetPoint(i % itemsPerPage, Constants.HexagonWidth);
                Canvas.SetLeft(tile, pos.X);
                Canvas.SetTop(tile, pos.Y);
                recipeTileCanvas.Children.Add(tile);
            }
        }

        private void DisplayNoCocktailsFound(ObservableCollection<Recipe> recipes)
        {
            if (recipes.Count == 0)
            {
                NoRecipesFoundTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                NoRecipesFoundTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        private void Next_Page(object sender, RoutedEventArgs e)
        {
            page++;
            displayPage(page);
        }

        private void Previous_Page(object sender, RoutedEventArgs e)
        {
            page--;
            displayPage(page);
        }

        private async void App_FilterApplied(object sender, App.FilterAppliedEventArgs args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High,
            () =>
            {
                var filteredRecipes = ApplyFilter(args.FilteredIngredients);
                Recipes = new ObservableCollection<Recipe>(filteredRecipes);
            });
        }

        // Accept a list of Ingredients, and return all Recipes in the current List
        // that have ALL of those Ingredients
        private List<Recipe> ApplyFilter(List<Ingredient> filteredIngredients)
        {
            List<Recipe> filteredRecipes = new List<Recipe>();
            List<string> filteredIngredientIds = filteredIngredients.Select(i => i.IngredientId).ToList();

            foreach (Recipe r in app.RecipesToFilter)
            {
                List<string> recipeIngredientIds = r.Ingredients.Select(i => i.IngredientId).ToList();
                bool ingredientsMatch = true;

                foreach (string id in filteredIngredientIds)
                {
                    if (!recipeIngredientIds.Contains(id))
                    {
                        ingredientsMatch = false;
                        break;
                    }
                }

                if (ingredientsMatch)
                {
                    filteredRecipes.Add(r);
                }
            }

            return filteredRecipes;
        }
    }
}
