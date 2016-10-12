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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace BarBot.UserControls
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

            bool success = await socket.OpenConnection("ws://192.168.1.41:8000?id=barbot_5cf502");

            if (success)
            {
                Dictionary<String, Object> data = new Dictionary<String, Object>();
                data.Add("barbot_id", "barbot_5cf502");

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
                foreach (Recipe recipe in args.Recipes)
                {
                    Uc_RecipeTile recipeTile = new Uc_RecipeTile();
                    recipesStackPanel.Children.Add(recipeTile);
                    recipeTile.Recipe.Name = recipe.Name;
                }
            });
        }
    }
}
