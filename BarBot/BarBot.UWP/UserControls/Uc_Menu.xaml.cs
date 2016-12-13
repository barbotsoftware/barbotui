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
            if(socket.IsOpen)
            {
                Dictionary<String, Object> data = new Dictionary<String, Object>();
                data.Add("barbot_id", String.Format("barbot_{0}", barbotID));

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
                recipeTileCanvas.Children.Clear();

                for(int i = 0; i < 10; i++)
                {
                    if(args.Recipes.ElementAt(i) != null)
                    {
                        Uc_RecipeTile tile = new Uc_RecipeTile();
                        tile.Recipe = args.Recipes.ElementAt(i);
                        Point pos = getPoint(i, (int)tile.hexagon.Width);
                        //Console.WriteLine("Tile: " + tile.Recipe.Name);
                        //Console.WriteLine("Position: " + pos.X + ", " + pos.Y);

                        Canvas.SetLeft(tile, pos.X);
                        Canvas.SetTop(tile, pos.Y);
                        recipeTileCanvas.Children.Add(tile);
                    }
                }
            });
        }

        private Point getPoint(int i, int width)
        {
            int pos = i % 4;
            int r = i / 4;

            int top = 0;
            int left = 0;

            int margin = 20;
            int hexPadding = 20;
            int height = (int)(2 * Math.Sqrt(Math.Pow(width / 2, 2) - Math.Pow(width / 4, 2)));

            // Top Left
            if (pos == 0)
            {
                top = 0;
                left = r * (width + (width - width / 2) + (hexPadding * 2));
            }
            // Bottom Left
            else if (pos == 1)
            {
                top = height + hexPadding;
                left = r * (width + (width - width / 2) + (hexPadding * 2));
            }
            // Top right (Diagonally down and right from top left)
            else if(pos == 2)
            {
                top = height/2 + hexPadding/2;
                left = (width - (width / 4) + hexPadding) + (r * (width + (width - width / 2) + (hexPadding * 2)));
            }
            // Bottom right (Diagonally down and right from bottom left)
            else if(pos == 3)
            {
                top = height + hexPadding + height/2 + hexPadding/2;
                left = (width - (width / 4) + hexPadding) + (r * (width + (width - width / 2) + (hexPadding * 2)));
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
    }
}
