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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace BarBot.UWP.UserControls.RecipeList
{
    public sealed partial class Uc_RecipeList : UserControl
    {
        private UWPWebSocketService webSocketService;
        private List<Recipe> recipes;
        private App app;

        private int page = 0;
        private int itemsPerPage = 10;
        private int pages = 1;

        public List<Recipe> Recipes
        {
            get { return recipes; }
            set
            {
                // set recipes
                recipes = value;
                
                if (recipes.Count == 0)
                {
                    NoRecipesFoundTextBlock.Visibility = Visibility.Visible;
                }
                
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
        }

        private void displayPage(int page)
        {
            // Hide BackButton if it's the first page, hide NextButton if its the last page
            BackButton.Visibility = page == 0 ? Visibility.Collapsed : Visibility.Visible;
            NextButton.Visibility = page >= pages - 1 ? Visibility.Collapsed : Visibility.Visible;

            // clear out the current recipe tiles
            recipeTileCanvas.Children.Clear();

            for(int i = 0 + (itemsPerPage * page); i < Math.Min((itemsPerPage * page) + itemsPerPage, recipes.Count); i++)
            {
                Uc_RecipeTile tile = new Uc_RecipeTile();
                tile.Recipe = recipes[i];
                Point pos = Helpers.GetPoint(i % itemsPerPage, Constants.HexagonWidth);
                Canvas.SetLeft(tile, pos.X);
                Canvas.SetTop(tile, pos.Y);
                recipeTileCanvas.Children.Add(tile);
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
    }
}
