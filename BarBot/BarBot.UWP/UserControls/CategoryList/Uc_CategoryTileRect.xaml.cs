using BarBot.Core.Model;
using BarBot.UWP.Pages;
using BarBot.UWP.Websocket;
using System;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace BarBot.UWP.UserControls.CategoryList
{
    public sealed partial class Uc_CategoryTileRect : UserControl
    {
        private Category category;
        private UWPWebSocketService webSocketService;

        public Category Category
        {
            get { return category; }
            set
            {
                category = value;
                OnPropertyChanged("Category");
            }
        }

        public Uc_CategoryTileRect()
        {
            this.InitializeComponent();
            Category category = new Category();
            this.DataContext = this;

            App app = Application.Current as App;
            webSocketService = app.webSocketService;
        }


        private void Category_Click(object sender, RoutedEventArgs e)
        {
            if (category.CategoryId == null || "".Equals(category.CategoryId))
            {
                webSocketService.Socket.GetRecipesEvent += Socket_GetRecipesEvent;
                webSocketService.GetRecipes();
            }
            else
            {
                webSocketService.Socket.GetCategoryEvent += Socket_GetCategoryEvent;
                webSocketService.GetCategory(category.CategoryId);
            }
        }

        private async void Socket_GetCategoryEvent(object sender, Core.WebSocket.WebSocketEvents.GetCategoryEventArgs args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High,
            () =>
            {
                // If category has sub categories, navigate to category list, populate with sub category list
                // Otherwise, navigate to menu with recipe list
                if (args.Category.SubCategories != null && args.Category.SubCategories.Count > 0)
                {
                    ((Window.Current.Content as Frame).Content as MainPage).ContentFrame.Navigate(typeof(Menu), args.Category.SubCategories, new DrillInNavigationTransitionInfo());
                }
                else
                {
                    webSocketService.Socket.GetRecipesEvent += Socket_GetRecipesEvent;
                    webSocketService.GetRecipes(args.Category.CategoryId);
                }
            });

            webSocketService.Socket.GetCategoryEvent -= Socket_GetCategoryEvent;
        }

        private async void Socket_GetRecipesEvent(object sender, Core.WebSocket.WebSocketEvents.GetRecipesEventArgs args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High,
            () =>
            {
                ((Window.Current.Content as Frame).Content as MainPage).ContentFrame.Navigate(typeof(Menu), args.Recipes, new DrillInNavigationTransitionInfo());
            });

            webSocketService.Socket.GetRecipesEvent -= Socket_GetRecipesEvent;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
