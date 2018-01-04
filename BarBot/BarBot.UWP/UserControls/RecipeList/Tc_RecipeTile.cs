using BarBot.Core;
using BarBot.Core.Model;
using BarBot.UWP.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;

namespace BarBot.UWP.UserControls.RecipeList
{
    public sealed class Tc_RecipeTile : Control, INotifyPropertyChanged
    {
        App app;
        private string webserverUrl;

        private BitmapImage _cachedImage;

        public BitmapImage CachedImage
        {
            get { return _cachedImage; }
            set
            {
                _cachedImage = value;
                OnPropertyChanged("CachedImage");
            }
        }

        private Recipe recipe;

        public Recipe Recipe
        {
            get
            {
                return recipe;
            }

            set
            {
                recipe = value;
                if (value.Name != null)
                {
                    recipe.Name = Helpers.UppercaseWords(value.Name);
                    if (value.Name.Equals(Constants.CustomRecipeName))
                    {
                        var imageUri = new Uri("http://" + webserverUrl + ":" + Constants.PortNumber + "/img/custom_recipe.png");
                        var recipeImage = new BitmapImage(imageUri);
                        CachedImage = recipeImage;
                    }
                    else
                    {
                        CachedImage = app.getCachedImage(value);
                    }
                }
                OnPropertyChanged("Recipe");
            }
        }

        public Tc_RecipeTile()
        {
            this.DefaultStyleKey = typeof(Tc_RecipeTile);
            this.DataContext = this;

            app = Application.Current as App;
            webserverUrl = app.webserverUrl;

            Recipe = new Recipe();
        }

        protected override void OnApplyTemplate()
        {
            var buttons = new List<Button>();
            buttons.Add(GetTemplateChild("HexagonButton") as Button);
            buttons.Add(GetTemplateChild("RecipeImageButton") as Button);
            buttons.Add(GetTemplateChild("HexagonGradientButton") as Button);
            buttons.Add(GetTemplateChild("RecipeNameButton") as Button);

            foreach (Button btn in buttons)
            {
                btn.Click += Recipe_Detail;
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

        private void Recipe_Detail(object sender, RoutedEventArgs e)
        {
            ((Window.Current.Content as Frame).Content as MainPage).ContentFrame.Navigate(typeof(Pages.RecipeDetail), Recipe, new DrillInNavigationTransitionInfo());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
