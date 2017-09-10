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
using BarBot.UWP.Bluetooth;
using BarBot.UWP.IO;
using BarBot.UWP.Database;
using BarBot.UWP.Pages;
using Windows.UI.Xaml.Media.Animation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace BarBot.UWP.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PartyMode : Page
    {
        private string barbotID;

        private BarbotIOController controller;

        public PartyMode()
        {
            this.InitializeComponent();

            App app = Application.Current as App;

            barbotID = app.barbotID;
        }

        private void Btn_DrinkMenu_Click(object sender, RoutedEventArgs e)
        {
            ((Window.Current.Content as Frame).Content as MainPage).ContentFrame.Navigate(typeof(Menu), null, new DrillInNavigationTransitionInfo());
        }

        private void Btn_OrderQueue_Click(object sender, RoutedEventArgs e)
        {
            ((Window.Current.Content as Frame).Content as MainPage).ContentFrame.Navigate(typeof(OrderQueue), null, new DrillInNavigationTransitionInfo());
        }
    }
}
