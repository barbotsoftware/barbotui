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
using BarBot.UWP.Database;
using System.Collections.ObjectModel;

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

            foreach(DrinkOrder drinkOrder in app.barbotDB.DrinkOrders.ToList())
            {
                DrinkOrders.Add(drinkOrder);
            }

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
    }
}
