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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace BarBot.UWP.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Menu : Page
    {
        private UWPWebSocketService webSocketService;
        private List<Category> categories = new List<Category>();
        private List<Recipe> recipes = new List<Recipe>();

        public List<Category> Categories
        {
            get { return categories; }
            set
            {
                categories = value;
                CategoryList.Categories = categories;
            }
        }

        public List<Recipe> Recipes
        {
            get { return recipes; }
            set
            {
                recipes = value;
                RecipeList.Recipes = recipes;
            }
        }

        public Menu()
        {
            this.InitializeComponent();
            webSocketService = (Application.Current as App).webSocketService;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            CategoryList.Visibility = Visibility.Collapsed;
            RecipeList.Visibility = Visibility.Collapsed;
            if (e.Parameter == null)
            {
                webSocketService.Socket.GetCategoriesEvent += Socket_GetCategoriesEvent;
                webSocketService.GetCategories();
                CategoryList.Visibility = Visibility.Visible;
                searchTextBox.Visibility = Visibility.Collapsed;
            }
            else if (e.Parameter.GetType().Equals(Categories.GetType()))
            {
                Categories = e.Parameter as List<Category>;
                CategoryList.Visibility = Visibility.Visible;
                searchTextBox.Visibility = Visibility.Collapsed;
            }
            else if (e.Parameter.GetType().Equals(Recipes.GetType()))
            {
                Recipes = e.Parameter as List<Recipe>;
                RecipeList.Visibility = Visibility.Visible;
                searchTextBox.Visibility = Visibility.Visible;
            }
        }

        private void Back_To_PartyMode(object sender, RoutedEventArgs e)
        {
            ((Window.Current.Content as Frame).Content as MainPage).ContentFrame.GoBack(new DrillInNavigationTransitionInfo());
        }

        private async void Socket_GetCategoriesEvent(object sender, WebSocketEvents.GetCategoriesEventArgs args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High,
            () =>
            {
                Categories = args.Categories;
            });

            webSocketService.Socket.GetCategoriesEvent -= Socket_GetCategoriesEvent;
        }

        private void searchTextBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            List<Recipe> filteredList = recipes.Where(x => x.Name.StartsWith(searchTextBox.Text)).ToList();

            RecipeList.Recipes = filteredList;
        }
    }
}
