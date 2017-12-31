using BarBot.Core.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
        private Garnish garnish1;
        private Garnish garnish2;
        
        public Garnish Garnish1
        {
            get { return garnish1; }
            set
            {
                garnish1 = value;
                OnPropertyChanged("Garnish1");
            }
        }

        public Garnish Garnish2
        {
            get { return garnish2; }
            set
            {
                garnish2 = value;
                OnPropertyChanged("Garnish2");
            }
        }

        public GarnishContentDialog(Uc_RecipeDetail parent)
        {
            this.InitializeComponent();
            this.DataContext = this;

            this.parentUserControl = parent;
            this.ShouldProceed = false;

            App app = Application.Current as App;

            this.Garnish1 = app.Garnishes.Where(g => g.OptionNumber == 1).First();
            this.Garnish2 = app.Garnishes.Where(g => g.OptionNumber == 2).First();

            this.Opened += GarnishContentDialog_OpenedAsync;

            if (Garnish1.Quantity == 0 && Garnish2.Quantity == 0)
            {
                GarnishButton1.Visibility = Visibility.Collapsed;
                GarnishButton2.Visibility = Visibility.Collapsed;
                GarnishButtonBoth.Visibility = Visibility.Collapsed;
                GarnishButtonNone.Visibility = Visibility.Collapsed;
                GarnishEmptyTextBlock.Visibility = Visibility.Visible;
            }
            else if (Garnish1.Quantity == 0)
            {
                GarnishButton1.Visibility = Visibility.Collapsed;
                GarnishButtonBoth.Visibility = Visibility.Collapsed;
                GarnishButtonNone.Content = "No, thanks";
            }
            else if (Garnish2.Quantity == 0 || Garnish1.Name.Equals(Garnish2.Name))
            {
                GarnishButton2.Visibility = Visibility.Collapsed;
                GarnishButtonBoth.Visibility = Visibility.Collapsed;
                GarnishButtonNone.Content = "No, thanks";
            }
        }

        private async void GarnishContentDialog_OpenedAsync(ContentDialog sender, ContentDialogOpenedEventArgs args)
        {
            if (Garnish1.Quantity == 0 && Garnish2.Quantity == 0)
            {
                await Task.Delay(3000);
                ShouldProceed = true;
                sender.Hide();
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
