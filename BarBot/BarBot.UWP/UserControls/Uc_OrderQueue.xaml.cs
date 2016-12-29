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

            DrinkOrders = new ObservableCollection<DrinkOrder>();

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

            Dictionary<IO.Devices.IContainer, double> ingredients =  Utils.Helpers.GetContainersFromRecipe(recipe, app.barbotIOController.Containers);

            //app.barbotIOController.PourDrink(ingredients);

            var dialog = new ContentDialog()
            {
                MaxWidth = this.ActualWidth,
                Content = new TextBlock()
                {
                    Text = string.Format("Your {0} Is Pouring!", recipe.Name),
                    VerticalAlignment = VerticalAlignment.Center,
                    FontSize = 45
                },
                Background = new SolidColorBrush(Windows.UI.Color.FromArgb(100, 19, 19, 19)),
                Foreground = new SolidColorBrush(Windows.UI.Colors.White),
                BorderBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(100, 34, 34, 34))
            };

            dialog.ShowAsync();

            await Task.Delay(5000);

            dialog.Hide();
        }
    }
}
