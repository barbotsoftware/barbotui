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
using BarBot.Core;
using BarBot.Core.Model;
using BarBot.Core.WebSocket;
using BarBot.UWP.Database;
using BarBot.UWP.Bluetooth;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace BarBot.UWP.UserControls
{
    public sealed partial class Uc_Menu : UserControl
    {
        private WebSocketHandler socket;

        private string barbotID;

        // Public in case i wanna access it elsewhere
        private int margin = 40;
        private int hexPadding = 20;
        private List<List<Recipe>> AllRecipes = new List<List<Recipe>>();
        private object PageTracker;
        private int Page = 0;

        public Uc_Menu()
        {
            this.InitializeComponent();

            App app = Application.Current as App;

            socket = app.webSocket;
            barbotID = app.barbotID;

            init();
        }

        public void init()
        {
            if (socket.IsOpen)
            {
                Dictionary<String, Object> data = new Dictionary<String, Object>();
                data.Add("barbot_id", String.Format("barbot_{0}", barbotID));

                Message message = new Message(Constants.Command, Constants.GetRecipesForBarbot, data);

                socket.GetRecipesEvent += Socket_GetRecipesEvent;

                socket.sendMessage(message);
            }

            // Set back and next button sizes
            // Back Button
            double BackButtonSizeRatio = BackButton.Height / BackButton.Width;
            BackButton.Height = (2 * Math.Sqrt(Math.Pow(Constants.HexagonWidth / 2, 2) - Math.Pow(Constants.HexagonWidth / 4, 2)));
            BackButton.Width = BackButton.Height / BackButtonSizeRatio;
            //BackButton.Height = (2 * Math.Sqrt(Math.Pow(Constants.HexagonWidth / 2, 2) - Math.Pow(Constants.HexagonWidth / 4, 2))) - (hexPadding / 2);
            // Left, Top
            BackButton.Margin = new Thickness(recipeTileCanvas.Margin.Left - (hexPadding / 2) - (BackButton.Width / 2), recipeTileCanvas.Margin.Top + margin + (BackButton.Height / 2) + (hexPadding / 2), 0, 0);

            // Next Button
            double NextButtonSizeRatio = NextButton.Height / NextButton.Width;
            NextButton.Height = (2 * Math.Sqrt(Math.Pow(Constants.HexagonWidth / 2, 2) - Math.Pow(Constants.HexagonWidth / 4, 2)));
            NextButton.Width = NextButton.Height / NextButtonSizeRatio;
            //NextButton.Height = (2 * Math.Sqrt(Math.Pow(Constants.HexagonWidth / 2, 2) - Math.Pow(Constants.HexagonWidth / 4, 2))) - (hexPadding / 2);
            // Left, Top
            NextButton.Margin = new Thickness(0, recipeTileCanvas.Margin.Top + margin + (NextButton.Height / 2) + (hexPadding / 2), recipeTileCanvas.Margin.Left - (hexPadding / 2) - (NextButton.Width / 2), 0);
        }

        private async void Socket_GetRecipesEvent(object sender, WebSocketEvents.GetRecipesEventArgs args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High,
            () =>
            {
                List<Recipe> page = new List<Recipe>();
                // Populate AllRecipes
                for (var i = 0; i < args.Recipes.Count; i++)
                {
                    page.Add(args.Recipes[i]);
                    if((i != 0 && i % 10 == 9)|| i == args.Recipes.Count - 1)
                    {
                        AllRecipes.Add(page);
                        page = new List<Recipe>();
                    }
                }
                displayPage(Page);
            });

            socket.GetRecipesEvent -= Socket_GetRecipesEvent;
        }

        private void displayPage(int page)
        {
            // Hide BackButton if it's the first page
            if (page == 0)
            {
                BackButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                BackButton.Visibility = Visibility.Visible;
            }

            if(page == AllRecipes.Count-1)
            {
                NextButton.Visibility = Visibility.Collapsed;
            } else
            {
                NextButton.Visibility = Visibility.Visible;
            }
            

            if (page < AllRecipes.Count)
            {
                recipeTileCanvas.Children.Clear();
                for (int i = 0; i < AllRecipes[page].Count; i++)
                {
                    if (AllRecipes[page][i] != null)
                    {
                        Uc_RecipeTile tile = new Uc_RecipeTile();
                        tile.Recipe = AllRecipes[page][i];
                        Point pos = getPoint(i, Constants.HexagonWidth);
                        Canvas.SetLeft(tile, pos.X);
                        Canvas.SetTop(tile, pos.Y);
                        recipeTileCanvas.Children.Add(tile);
                    }
                }
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<String, Object> data = new Dictionary<String, Object>();
            data.Add("barbot_id", Constants.BarBotId);

            Message message = new Message(Constants.Command, Constants.GetRecipesForBarbot, data);

            socket.GetRecipesEvent += Socket_GetRecipesEvent;

            socket.sendMessage(message);
        }

        private void Next_Page(object sender, RoutedEventArgs e)
        {
            Page++;
            displayPage(Page);
            // XAML: Fix Button to work with this shit
        }

        private void Previous_Page(object sender, RoutedEventArgs e)
        {
            Page--;
            displayPage(Page);
        }
    }
}
