using BarBot.Core;
using BarBot.Core.Model;
using BarBot.UWP.Pages;
using BarBot.UWP.Utils;
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
            queryString = Helpers.CleanInput(SearchAutoSuggestBox.Text);

            if (string.IsNullOrEmpty(queryString))
            {
                args.Cancel = true;
                SearchAutoSuggestBox.Focus(FocusState.Keyboard);
            } else
            {
                if (this.app.AllRecipes.Count == 0)
                {
                    webSocketService.Socket.GetRecipesEvent += Socket_GetRecipesEvent;
                    webSocketService.GetRecipes();
                }
                else
                {
                    DisplaySearchResults(this.app.AllRecipes);
                }
                
                sender.Hide();
            }
        }

        private async void Socket_GetRecipesEvent(object sender, Core.WebSocket.WebSocketEvents.GetRecipesEventArgs args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High,
            () =>
            {
                this.app.AllRecipes = args.Recipes;
                DisplaySearchResults(args.Recipes);
            });

            webSocketService.Socket.GetRecipesEvent -= Socket_GetRecipesEvent;
        }

        private void DisplaySearchResults(List<Recipe> recipes)
        {
            // Get all Recipes from WS Server then filter, TODO: send query string and do search on server side
            List<Recipe> filteredList = recipes.Where(x => x.Name.ToLower().StartsWith(queryString.ToLower())).ToList();

            Dictionary<string, List<Recipe>> recipeDictionary = new Dictionary<string, List<Recipe>>
                {
                    { Constants.SearchCategoryName, filteredList }
                };

            ((Window.Current.Content as Frame).Content as MainPage).ContentFrame.Navigate(typeof(Menu), recipeDictionary, new DrillInNavigationTransitionInfo());
        }
    }
}
