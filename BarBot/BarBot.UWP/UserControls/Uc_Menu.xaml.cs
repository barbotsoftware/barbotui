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
using BarBot.Model;
using BarBot.WebSocket;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace BarBot.UWP.UserControls
{
    public sealed partial class Uc_Menu : UserControl
    {
        private WebSocketHandler socket;

        public Uc_Menu()
        {
            this.InitializeComponent();

            init();
        }

        public async void init()
        {
            socket = new WebSocketHandler();

            bool success = await socket.OpenConnection(String.Format("{0}?id={1}", Constants.EndpointURL, Constants.BarbotId));

            if (success)
            {
                Dictionary<String, Object> data = new Dictionary<String, Object>();
                data.Add("barbot_id", Constants.BarbotId);

                Message message = new Message(Constants.Command, Constants.GetRecipesForBarbot, data);

                socket.GetRecipesEvent += Socket_GetRecipesEvent;

                socket.sendMessage(message);
            }
        }

        private async void Socket_GetRecipesEvent(object sender, WebSocketEvents.GetRecipesEventArgs args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High,
            () =>
            {
                int count = 0;
                for(int row = 0; row < recipesGrid.RowDefinitions.Count; row++)
                {
                    for(int col = 0; col < recipesGrid.ColumnDefinitions.Count; col++)
                    {
                        if (args.Recipes.Count > count)
                        {
                            Uc_RecipeTile recipeTile = new Uc_RecipeTile();
                            recipeTile.Recipe = args.Recipes.ElementAt(count);
                            Grid.SetColumn(recipeTile, col);
                            Grid.SetRow(recipeTile, row);
                            recipesGrid.Children.Add(recipeTile);
                            count++;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            });
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<String, Object> data = new Dictionary<String, Object>();
            data.Add("recipe_id", "recipe_9aa19a");

            Message message = new Message(Constants.Command, Constants.GetRecipeDetails, data);

            socket.GetRecipesEvent += Socket_GetRecipesEvent1;

            socket.sendMessage(message);
        }

        private async void Socket_GetRecipesEvent1(object sender, WebSocketEvents.GetRecipesEventArgs args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High,
            () => 
            {
                
            });
        }
    }
}
