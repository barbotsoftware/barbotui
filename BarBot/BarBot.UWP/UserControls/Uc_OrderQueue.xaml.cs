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
using Windows.UI.Xaml.Navigation;
using BarBot.Core.Model;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace BarBot.UWP.UserControls
{
    public sealed partial class Uc_OrderQueue : UserControl
    {
        private App app;

        public ObservableCollection<DrinkOrder> DrinkOrders { get; set; }

        public Uc_OrderQueue()
        {
            this.InitializeComponent();

            this.DataContext = this;

            app = Application.Current as App;

            DrinkOrders = new ObservableCollection<DrinkOrder>(app.DrinkOrders);

            app.DrinkOrderAdded += App_DrinkOrderAdded;
        }

        private async void App_DrinkOrderAdded(object sender, App.DrinkOrderAddedEventArgs args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High,
            () =>
            {
                DrinkOrders.Add(args.DrinkOrder);
            });
        }

        private async void PourButton_Click(object sender, RoutedEventArgs e)
        {
            Recipe recipe = (sender as Button).Tag as Recipe;

            var dialog = new ContentDialog()
            {
                MaxWidth = this.ActualWidth,
                Content = new TextBlock()
                {
                    Text = string.Format("Your {0} Is Pouring!", recipe.Name),
                    VerticalAlignment = VerticalAlignment.Center,
                    FontSize = 45
                },
                Background = new SolidColorBrush(Windows.UI.Colors.Black),
                Foreground = new SolidColorBrush(Windows.UI.Colors.White),
                BorderBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(100, 34, 34, 34))
            };

            dialog.Opened += (s, a) => Dialog_Opened(s, a, recipe);
            await dialog.ShowAsync();
        }

        private void Dialog_Opened(ContentDialog sender, ContentDialogOpenedEventArgs args, Recipe recipe)
        {
            Dictionary<IO.Devices.IContainer, double> ingredients = Utils.Helpers.GetContainersFromRecipe(recipe, app.barbotIOController.Containers);

            app.barbotIOController.PourDrink(ingredients);

            DrinkOrder drinkorder = DrinkOrders.Where(x => x.Recipe.Equals(recipe)).First();
            DrinkOrders.Remove(drinkorder);
            app.DrinkOrders.Remove(drinkorder);

            sender.Hide();
        }

        private void Back_To_PartyMode(object sender, RoutedEventArgs e)
        {
            ((Window.Current.Content as Frame).Content as MainPage).ContentFrame.Content = new Uc_PartyMode();
        }
    }
}
