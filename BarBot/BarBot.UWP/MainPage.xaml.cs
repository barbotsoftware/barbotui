using BarBot.Model;
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
using BarBot.WebSocket;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BarBot.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            WebSocketHandler socket = new WebSocketHandler();

            await socket.OpenConnection("ws://localhost:8000?id=barbot_5cf502");

            Dictionary<String, Object> data = new Dictionary<String, Object>();
            data.Add("barbot_id", "barbot_5cf502");

            Message message = new Message(Constants.Command, Constants.GetRecipesForBarbot, data);

            socket.GetRecipesEvent += Socket_GetRecipesEvent;

            socket.sendMessage(message);
        }

        private async void Socket_GetRecipesEvent(object sender, WebSocketEvents.GetRecipesEventArgs args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, 
            () => {
                HelloMessage.Text = "";
                foreach (Recipe recipe in args.Recipes)
                {
                    HelloMessage.Text += " " + recipe.Name;
                }
            });
        }
    }
}
