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
    public sealed partial class Uc_RecipeTile : UserControl, INotifyPropertyChanged
    {
        private Recipe recipe;
        App app;
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

        private string webserverUrl;

        public Windows.UI.Xaml.Shapes.Path hexagon;

        public Uc_RecipeTile()
        {
            this.InitializeComponent();

            app = Application.Current as App;

            Recipe = new Recipe();
            this.DataContext = this;

            webserverUrl = (Application.Current as App).webserverUrl;
        }

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

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void Drink_Detail(object sender, RoutedEventArgs e)
        {
            ((Window.Current.Content as Frame).Content as MainPage).ContentFrame.Navigate(typeof(Pages.RecipeDetail), Recipe, new DrillInNavigationTransitionInfo());
        }
    }
}
