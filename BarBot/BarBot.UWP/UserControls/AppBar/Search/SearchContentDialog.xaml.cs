using BarBot.Core;
using BarBot.Core.Model;
using BarBot.UWP.Pages;
using BarBot.UWP.Websocket;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace BarBot.UWP.UserControls.AppBar.Search
{
    public sealed partial class SearchContentDialog : ContentDialog
    {
        private App app;
        private UWPWebSocketService webSocketService;
        private string queryString;

        public SearchContentDialog()
        {
            this.InitializeComponent();

            this.app = Application.Current as App;
            webSocketService = app.webSocketService;
        }

        private void SearchDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var AutoSuggestBox = sender.Content as AutoSuggestBox;
            queryString = AutoSuggestBox.Text;

            if (string.IsNullOrEmpty(queryString))
            {
                args.Cancel = true;
                AutoSuggestBox.Focus(FocusState.Keyboard);
            } else
            {
                webSocketService.Socket.GetRecipesEvent += Socket_GetRecipesEvent;
                webSocketService.GetRecipes();
                sender.Hide();
            }
        }

        private async void Socket_GetRecipesEvent(object sender, Core.WebSocket.WebSocketEvents.GetRecipesEventArgs args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High,
            () =>
            {
                // Get all Recipes from WS Server then filter, TODO: send query string and do search on server side
                List<Recipe> filteredList = args.Recipes.Where(x => x.Name.ToLower().StartsWith(queryString.ToLower())).ToList();

                Dictionary<string, List<Recipe>> recipes = new Dictionary<string, List<Recipe>>
                {
                    { Constants.SearchCategoryName, filteredList }
                };

                ((Window.Current.Content as Frame).Content as MainPage).ContentFrame.Navigate(typeof(Menu), recipes, new DrillInNavigationTransitionInfo());
            });

            webSocketService.Socket.GetRecipesEvent -= Socket_GetRecipesEvent;
        }

        private void SearchDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // Dismiss Dialog
            sender.Hide();
        }
    }
}
