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
using BarBot.Core.WebSocket;
using System.ComponentModel;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace BarBot.UWP.UserControls
{
    public sealed partial class Uc_DrinkDetail : UserControl, INotifyPropertyChanged
    {
        private Recipe _recipe;
        private WebSocketHandler socket;
        private string barbotID;

        public Uc_DrinkDetail(Recipe SelectedRecipe)
        {
            App app = Application.Current as App;
            socket = app.webSocket;
            barbotID = app.barbotID;
            
            this.InitializeComponent();
            this.DataContext = this;
            Recipe = SelectedRecipe;

            init();
        }

        public void init()
        {
            if (socket.IsOpen)
            {
                Dictionary<String, Object> data = new Dictionary<String, Object>();
                data.Add("recipe_id", _recipe.RecipeId);

                Message message = new Message(Constants.Command, Constants.GetRecipeDetails, data);

                socket.GetRecipeDetailsEvent += Socket_GetRecipeDetailEvent;

                socket.sendMessage(message);
            }
        }

        public Recipe Recipe
        {
            get
            {
                return _recipe;
            }

            set
            {
                _recipe = value;
                OnPropertyChanged("Recipe");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void Back_To_Menu(object sender, RoutedEventArgs e)
        {
            ((Window.Current.Content as Frame).Content as MainPage).ContentFrame.Content = new Uc_Menu();
        }

        private async void Socket_GetRecipeDetailEvent(object sender, WebSocketEvents.GetRecipeDetailsEventArgs args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High,
            () =>
            {
                Recipe.Ingredients = args.Recipe.Ingredients;
            });

            socket.GetRecipeDetailsEvent -= Socket_GetRecipeDetailEvent;
        }

        //private static WriteableBitmap CropImage(WriteableBitmap source,
        //                                                 int xOffset, int yOffset,
        //                                                 int width, int height)
        //{
        //    // Get the width of the source image
        //    var sourceWidth = source.PixelWidth;

        //    // Get the resultant image as WriteableBitmap with specified size
        //    var result = new WriteableBitmap(width, height);

        //    // Create the array of bytes
        //    for (var x = 0; x <= height - 1; x++)
        //    {
        //        var sourceIndex = xOffset + (yOffset + x) * sourceWidth;
        //        var destinationIndex = x * width;

        //        Array.Copy(source.PixelBuffer, sourceIndex, result.PixelBuffer, destinationIndex, width);
        //    }
        //    return result;
        //}

        //private static Array getBitmapPixels()
        //{

        //}
    }
}
