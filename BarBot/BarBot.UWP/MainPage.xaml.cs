﻿using BarBot.Core.Model;
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
using BarBot.UWP.UserControls;
using BarBot.UWP.Pages;
using Windows.UI.Xaml.Media.Animation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BarBot.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            //ContentFrame.Content = new Uc_Menu();
            
            ContentFrame.Navigate(typeof(Menu), null, new DrillInNavigationTransitionInfo());
            //ContentFrame.Content = new Uc_PartyMode();
            //App app = App.Current as App;
            //ContentFrame.Content = new TestClient.TestClient(app.barbotDB, app.barbotIOController);
        }
    }
}
