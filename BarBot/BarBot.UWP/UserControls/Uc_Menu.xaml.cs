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
                data.Add("barbot_id", String.Format("barbot_{0}", (Application.Current as App).barbotID));

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
                        Point pos = getPoint(i);
                        Canvas.SetLeft(tile, pos.X);
                        Canvas.SetTop(tile, pos.Y);
                        recipeTileCanvas.Children.Add(tile);
                    }
                }
            });
        }

        private Point getPoint(int i)
        {
            int pos = i % 4;
            int r = i / 4;

            int top = 0;
            int left = 0;

            //311.7691453623979

            if (pos == 0)
            {
                top = 0; 
                left = 570 * r;
            }
            else if (pos == 1)
            {
                top = 325;
                //top = 320;
                left = 570 * r;
            }
            else if(pos == 2)
            {
                top = 165;
                //top = 160;
                left = 570 * r + 285;
            }
            else if(pos == 3)
            {
                top = 490;
                //top = 480;
                left = 570 * r + 285;
            }

            return new Point(left, top);
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
