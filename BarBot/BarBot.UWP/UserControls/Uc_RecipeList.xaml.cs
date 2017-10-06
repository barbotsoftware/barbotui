using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Graphics.Imaging;
using BarBot.Core;
using BarBot.Core.Model;
using BarBot.Core.WebSocket;
using BarBot.UWP.Database;
using BarBot.UWP.Bluetooth;
using BarBot.UWP.Websocket;
using BarBot.UWP.Pages;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace BarBot.UWP.UserControls
{
    public sealed partial class Uc_RecipeList : UserControl
    {
        private UWPWebSocketService webSocketService;
        private List<Recipe> recipes;
        private App app;

        private int margin = 40;
        private int hexPadding = 20;

        private int page = 0;
        private int itemsPerPage = 10;
        private int pages = 1;

        public List<Recipe> Recipes
        {
            get { return recipes; }
            set
            {
                // set recipes, and insert custom recipe as first element
                recipes = value;
                if (recipes.Count == 0 || !recipes.ElementAt(0).Name.Equals(Constants.CustomRecipeName))
                {
                    recipes.Insert(0, Recipe.CustomRecipe());
                }
                
                // calculate page count, and reset current page to 0
                pages = (recipes.Count + itemsPerPage - 1) / itemsPerPage;
                Page = 0;

                cacheImages();
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

            init();
        }

        public void init()
        {
            // Set back and next button sizes
            // Back Button
            double BackButtonSizeRatio = BackButton.Height / BackButton.Width;
            BackButton.Height = (2 * Math.Sqrt(Math.Pow(Constants.HexagonWidth / 2, 2) - Math.Pow(Constants.HexagonWidth / 4, 2)));
            BackButton.Width = BackButton.Height / BackButtonSizeRatio;
            // Left, Top
            BackButton.Margin = new Thickness(recipeTileCanvas.Margin.Left - (hexPadding / 2) - (BackButton.Width / 2) - 5, recipeTileCanvas.Margin.Top + margin + (BackButton.Height / 2) + (hexPadding / 2) - 5, 0, 0);

            // Next Button
            double NextButtonSizeRatio = NextButton.Height / NextButton.Width;
            NextButton.Height = (2 * Math.Sqrt(Math.Pow(Constants.HexagonWidth / 2, 2) - Math.Pow(Constants.HexagonWidth / 4, 2)));
            NextButton.Width = NextButton.Height / NextButtonSizeRatio;
            // Left, Top
            NextButton.Margin = new Thickness(0, recipeTileCanvas.Margin.Top + margin + (NextButton.Height / 2) + (hexPadding / 2), recipeTileCanvas.Margin.Left - (hexPadding / 2) - (NextButton.Width / 2), 0);
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
                Point pos = getPoint(i % itemsPerPage, Constants.HexagonWidth);
                Canvas.SetLeft(tile, pos.X);
                Canvas.SetTop(tile, pos.Y);
                recipeTileCanvas.Children.Add(tile);
            }
        }

        private Point getPoint(int i, int width)
        {
            int pos = i % 4;
            int r = i / 4;

            int top = 0;
            int left = 0;
            int height = (int)(2 * Math.Sqrt(Math.Pow(width / 2, 2) - Math.Pow(width / 4, 2)));

            // Top Left
            if (pos == 0)
            {
                top = 0;
                left = r * (width + (width / 2) + (hexPadding * 2));
            }
            // Bottom Left
            else if (pos == 1)
            {
                top = height + hexPadding;
                left = r * (width + (width / 2) + (hexPadding * 2));
            }
            // Top right (Diagonally down and right from top left)
            else if (pos == 2)
            {
                top = height / 2 + hexPadding / 2;
                left = (width - (width / 4) + hexPadding) + (r * (width + (width / 2) + (hexPadding * 2)));
            }
            // Bottom right (Diagonally down and right from bottom left)
            else if (pos == 3)
            {
                top = height + hexPadding + height / 2 + hexPadding / 2;
                left = (width - (width / 4) + hexPadding) + (r * (width + (width / 2) + (hexPadding * 2)));
            }

            return new Point(left, top + margin);
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

        private async void cacheImages()
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High,
            () =>
            {
                for (var i = 0; i < recipes.Count; i++)
                {
                    var imageUri = new Uri("http://" + app.webserverUrl + "/" + recipes[i].Img);
                    var recipeImage = new BitmapImage(imageUri);
                    if (!app._ImageCache.ContainsKey(recipes[i].Name))
                    {
                        app._ImageCache.Add(recipes[i].Name, recipeImage);
                    }
                }
            });
        }
    }
}
