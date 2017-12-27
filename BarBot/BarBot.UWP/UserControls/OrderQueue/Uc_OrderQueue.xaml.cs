using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using BarBot.Core.Model;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Animation;
using BarBot.UWP.Pages;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace BarBot.UWP.UserControls.OrderQueue
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

        //protected override void On

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
            if(app.barbotIOController.CupCount == 0)
            {
                var cupDialog = new ContentDialog()
                {
                    MaxWidth = ActualWidth,
                    Content = new TextBlock()
                    {
                        Text = "There are no cups left! Please place a cup or reset the cup dispenser.",
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        FontSize = 45
                    },
                    Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 56, 114)),
                    Foreground = new SolidColorBrush(Windows.UI.Colors.White),
                    BorderBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(100, 34, 34, 34)),
                    IsPrimaryButtonEnabled = true,
                    IsSecondaryButtonEnabled = true,
                    PrimaryButtonText = "OK",
                    SecondaryButtonText = "RESET"
                };

                cupDialog.PrimaryButtonClick += CupDialog_PrimaryButtonClick;
                cupDialog.SecondaryButtonClick += CupDialog_SecondaryButtonClick;
                await cupDialog.ShowAsync();
            }

            Recipe recipe = (sender as Button).Tag as Recipe;

            var dialog = new ContentDialog()
            {
                MaxWidth = this.ActualWidth,
                Content = new TextBlock()
                {
                    Text = string.Format("Your {0} Is Pouring!", recipe.Name),
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    FontSize = 45
                },
                Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 56, 114)),
                Foreground = new SolidColorBrush(Windows.UI.Colors.White),
                BorderBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(100, 34, 34, 34))
            };

            dialog.Opened += (s, a) => Dialog_Opened(s, a, recipe);
            await dialog.ShowAsync();
        }

        private void CupDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            app.barbotIOController.CupCount = 25;

            sender.Hide();
        }

        private void CupDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            app.barbotIOController.CupCount = 1;

            sender.Hide();
        }

        private async void Dialog_Opened(ContentDialog sender, ContentDialogOpenedEventArgs args, Recipe recipe)
        {
            await Task.Delay(1);

            Dictionary<IO.Devices.IContainer, double> ingredients = Utils.Helpers.GetContainersFromRecipe(recipe, app.barbotIOController.Containers);

            DrinkOrder drinkorder = DrinkOrders.Where(x => x.Recipe.Equals(recipe)).First();
            DrinkOrders.Remove(drinkorder);
            app.DrinkOrders.Remove(drinkorder);

            app.barbotIOController.PourDrinkSync(ingredients, drinkorder.Ice, Core.Constants.GarnishType.NONE /*drinkorder.Garnish*/); // TODO: use int value for garnish type

            sender.Hide();
        }

        private void Back_To_PartyMode(object sender, RoutedEventArgs e)
        {
            ((Window.Current.Content as Frame).Content as MainPage).ContentFrame.Navigate(typeof(PartyMode), null, new DrillInNavigationTransitionInfo());
        }
    }
}
