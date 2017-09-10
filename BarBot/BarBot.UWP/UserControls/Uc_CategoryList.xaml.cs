using BarBot.Core.Model;
using BarBot.Core.WebSocket;
using BarBot.UWP.Websocket;
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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace BarBot.UWP.UserControls
{
    public sealed partial class Uc_CategoryList : UserControl
    {
        private UWPWebSocketService webSocketService;

        public Uc_CategoryList()
        {
            this.InitializeComponent();

            App app = Application.Current as App;
            webSocketService = app.webSocketService;

            webSocketService.Socket.GetCategoriesEvent += Socket_GetCategoriesEvent;
            webSocketService.GetCategories();
        }

        private async void Socket_GetCategoriesEvent(object sender, WebSocketEvents.GetCategoriesEventArgs args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High,
            () =>
            {
                List<Uc_CategoryTile> tiles = new List<Uc_CategoryTile>();
                for(int i = 1; i <= 2; i++)
                {
                    for(int j = 1; j <= 4; j ++)
                    {
                        if (i * j > args.Categories.Count)
                            break;

                        Uc_CategoryTile tile = new Uc_CategoryTile();
                        tile.SetValue(Grid.ColumnProperty, j - 1);
                        tile.SetValue(Grid.RowProperty, i - 1);
                        tile.Category = args.Categories[(i * j) - 1];
                        tiles.Add(tile);
                    }
                }

                icMenuItems.ItemsSource = tiles;
            });

            webSocketService.Socket.GetCategoriesEvent -= Socket_GetCategoriesEvent;
        }
    }
}
