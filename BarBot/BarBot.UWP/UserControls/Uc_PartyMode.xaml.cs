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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace BarBot.UWP.UserControls
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Uc_PartyMode : Page
    {
        private string barbotID;

        private BLEPublisher publisher;

        public Uc_PartyMode()
        {
            this.InitializeComponent();

            App app = Application.Current as App;

            barbotID = app.barbotID;
            publisher = app.blePublisher;

            init();
        }

        public void init()
        {
            publisher.Start();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            publisher.Stop();
        }
    }
}
