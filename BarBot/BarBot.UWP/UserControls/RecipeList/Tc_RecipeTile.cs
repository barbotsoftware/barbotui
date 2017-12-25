using BarBot.Core;
using BarBot.Core.Model;
using BarBot.UWP.Utils;
using System;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
                        var imageUri = new Uri("http://" + webserverUrl + "/barbotweb/public/img/recipe_images/custom_recipe.png");
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
            var hexButton = GetTemplateChild("HexagonButton") as Button;
            var imageButton = GetTemplateChild("RecipeImageButton") as Button;
            var hexGradient = GetTemplateChild("HexagonGradientButton") as Button;
            var recipeName = GetTemplateChild("RecipeNameButton") as Button;

            hexButton.Click += Recipe_Detail;
            imageButton.Click += Recipe_Detail;
            hexGradient.Click += Recipe_Detail;
            recipeName.Click += Recipe_Detail;
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
