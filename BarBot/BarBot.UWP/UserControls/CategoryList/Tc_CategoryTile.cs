using BarBot.Core;
using BarBot.Core.Model;
using BarBot.UWP.Pages;
using BarBot.UWP.Utils;
using BarBot.UWP.Websocket;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;

namespace BarBot.UWP.UserControls.CategoryList
{
    public sealed class Tc_CategoryTile : Button, INotifyPropertyChanged
    {
        private App app;
        private Category category;
        private UWPWebSocketService webSocketService;

        public Category Category
        {
            get { return category; }
            set
            {
                category = value;
                
                if (category.Name == Constants.CustomCategoryName)
                {
                    var imageUri = new Uri(category.Img);
                    CachedImage = new BitmapImage(imageUri);
                }

                category.Name = Helpers.UppercaseWords(category.Name);
                OnPropertyChanged("Category");
            }
        }

        private BitmapImage cachedImage;

        public BitmapImage CachedImage
        {
            get { return cachedImage; }
            set
            {
                cachedImage = value;
                OnPropertyChanged("CachedImage");
            }
        }

        public Tc_CategoryTile()
        {
            this.DefaultStyleKey = typeof(Tc_CategoryTile);
            this.DataContext = this;

            this.app = Application.Current as App;
            webSocketService = app.webSocketService;
        }

        protected override void OnApplyTemplate()
        {
            var buttons = new List<Button>();
            buttons.Add(GetTemplateChild("HexagonButton") as Button);
            buttons.Add(GetTemplateChild("CategoryImageButton") as Button);
            buttons.Add(GetTemplateChild("HexagonGradientButton") as Button);
            buttons.Add(GetTemplateChild("CategoryNameButton") as Button);

            foreach (Button btn in buttons)
            {
                btn.Click += Category_Click;
                btn.AddHandler(PointerPressedEvent, new PointerEventHandler(PointerPressed), true);
                btn.AddHandler(PointerReleasedEvent, new PointerEventHandler(PointerReleased), true);
            }
        }

        private void PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            this.CapturePointer(e.Pointer);
            VisualStateManager.GoToState(this, "PointerUp", true);
        }

        private void PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            this.CapturePointer(e.Pointer);
            VisualStateManager.GoToState(this, "PointerDown", true);
        }

        private void Category_Click(object sender, RoutedEventArgs e)
        {
            // All Recipes
            if (category.CategoryId == null || "".Equals(category.CategoryId))
            {
                if (this.app.AllRecipes.Count == 0)
                {
                    webSocketService.Socket.GetRecipesEvent += Socket_GetRecipesEvent;
                    webSocketService.GetRecipes();
                }
                else
                {
                    DisplayRecipesInMenu(this.app.AllRecipes);
                }
            }
            // Custom Recipe -> Recipe Detail
            else if (category.Name.Equals(Constants.CustomCategoryName))
            {
                var customRecipe = Recipe.CustomRecipe();
                ((Window.Current.Content as Frame).Content as MainPage).ContentFrame.Navigate(typeof(Pages.RecipeDetail), customRecipe, new DrillInNavigationTransitionInfo());
            }
            // Get Category, Event handler deals with displaying Recipes for a Category
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
                    // Map Category to Sub Categories and pass as navigation parameter
                    Dictionary<string, List<Category>> categories = new Dictionary<string, List<Category>>
                    {
                        { args.Category.Name, args.Category.SubCategories }
                    };

                    ((Window.Current.Content as Frame).Content as MainPage).ContentFrame.Navigate(typeof(Menu), categories, new DrillInNavigationTransitionInfo());
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
                if (Category.Name == Constants.AllRecipesCategoryName)
                {
                    this.app.AllRecipes = args.Recipes;
                }
                DisplayRecipesInMenu(args.Recipes);
            });

            webSocketService.Socket.GetRecipesEvent -= Socket_GetRecipesEvent;
        }

        private void DisplayRecipesInMenu(List<Recipe> recipes)
        {
            Dictionary<string, List<Recipe>> recipeDictionary = new Dictionary<string, List<Recipe>>
            {
                { Category.Name, recipes }
            };

            ((Window.Current.Content as Frame).Content as MainPage).ContentFrame.Navigate(typeof(Menu), recipeDictionary, new DrillInNavigationTransitionInfo());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
