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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using BarBot.Core;
using BarBot.Core.Model;
using System.ComponentModel;
using Windows.UI.Xaml.Media.Animation;
using BarBot.UWP.Pages;
using BarBot.Core.ViewModel;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace BarBot.UWP.UserControls
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

                    if (value.Name.Equals("Custom Recipe"))
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
            ((Window.Current.Content as Frame).Content as MainPage).ContentFrame.Navigate(typeof(RecipeDetail), new RecipeDetailViewModel(null, Recipe), new SlideNavigationTransitionInfo());
        }
    }
}
