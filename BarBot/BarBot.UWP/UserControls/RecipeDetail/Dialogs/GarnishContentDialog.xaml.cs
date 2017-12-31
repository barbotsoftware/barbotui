using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using static BarBot.Core.Constants;

namespace BarBot.UWP.UserControls.RecipeDetail.Dialogs
{
    public sealed partial class GarnishContentDialog : ContentDialog, INotifyPropertyChanged
    {
        Uc_RecipeDetail parentUserControl;
        public Boolean ShouldProceed { get; set; }
        private string garnishOption1;
        private string garnishOption2;
        
        public string GarnishOption1
        {
            get { return garnishOption1; }
            set
            {
                garnishOption1 = value;
                OnPropertyChanged("GarnishOption1");
            }
        }

        public string GarnishOption2
        {
            get { return garnishOption2; }
            set
            {
                garnishOption2 = value;
                OnPropertyChanged("GarnishOption2");
            }
        }

        public GarnishContentDialog(Uc_RecipeDetail parent)
        {
            this.InitializeComponent();
            this.DataContext = this;

            this.parentUserControl = parent;
            this.ShouldProceed = false;

            App app = Application.Current as App;

            this.GarnishOption1 = app.Garnishes.Where(g => g.OptionNumber == 1).First().Name;
            this.GarnishOption2 = app.Garnishes.Where(g => g.OptionNumber == 2).First().Name;

            if (GarnishOption1.Equals(GarnishOption2))
            {
                GarnishButton2.Visibility = Visibility.Collapsed;
                GarnishButtonBoth.Visibility = Visibility.Collapsed;
                GarnishButtonNone.Content = "No, thanks";
            }
        }

        private void GarnishButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            if (button != null)
            {
                string garnishOption = button.Tag.ToString();

                switch(garnishOption)
                {
                    case "GarnishOption1":
                        parentUserControl.Garnish = GarnishType.GARNISH1;
                        break;
                    case "GarnishOption2":
                        parentUserControl.Garnish = GarnishType.GARNISH2;
                        break;
                    case "GarnishOptionBoth":
                        parentUserControl.Garnish = GarnishType.BOTH;
                        break;
                    case "GarnishOptionNone":
                        parentUserControl.Garnish = GarnishType.NONE;
                        break;
                }

                ShouldProceed = true;
                Dialog.Hide();
            }
        }

        private void ContentDialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            args.Cancel = true;
            ShouldProceed = false;
            sender.Hide();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
