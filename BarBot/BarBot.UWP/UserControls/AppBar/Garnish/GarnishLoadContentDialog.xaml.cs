using BarBot.Core;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BarBot.UWP.UserControls.AppBar.Garnish
{
    public sealed partial class GarnishLoadContentDialog : ContentDialog, INotifyPropertyChanged
    {
        private App app;
        private Core.Model.Garnish garnish1;
        private Core.Model.Garnish garnish2;
        private string quantity1DisplayText;
        private string quantity2DisplayText;
        private int garnish1PreviousQuantity;
        private int garnish2PreviousQuantity;

        public Core.Model.Garnish Garnish1
        {
            get { return garnish1; }
            set
            {
                garnish1 = value;
                Quantity1DisplayText = Garnish1.Quantity + " Left";
                OnPropertyChanged("Garnish1");
            }
        }

        public Core.Model.Garnish Garnish2
        {
            get { return garnish2; }
            set
            {
                garnish2 = value;
                Quantity2DisplayText = Garnish2.Quantity + " Left";
                OnPropertyChanged("Garnish2");
            }
        }

        public string Quantity1DisplayText
        {
            get { return quantity1DisplayText; }
            set
            {
                quantity1DisplayText = value;
                OnPropertyChanged("Quantity1DisplayText");
            }
        }

        public string Quantity2DisplayText
        {
            get { return quantity2DisplayText; }
            set
            {
                quantity2DisplayText = value;
                OnPropertyChanged("Quantity2DisplayText");
            }
        }

        public GarnishLoadContentDialog()
        {
            this.InitializeComponent();
            this.DataContext = this;

            app = Application.Current as App;

            this.Garnish1 = app.Garnishes.Where(g => g.OptionNumber == 1).First();
            this.Garnish2 = app.Garnishes.Where(g => g.OptionNumber == 2).First();

            garnish1PreviousQuantity = garnish1.Quantity;
            garnish2PreviousQuantity = garnish2.Quantity;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            app.webSocketService.UpdateGarnish(Garnish1);
            app.webSocketService.UpdateGarnish(Garnish2);
            sender.Hide();
        }

        private void ContentDialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // Roll back changes
            Garnish1.Quantity = garnish1PreviousQuantity;
            Garnish2.Quantity = garnish2PreviousQuantity;
        }

        private void ReloadGarnishButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            if (button != null)
            {
                string garnishOption = button.Tag.ToString();

                switch (garnishOption)
                {
                    case "Garnish1":
                        Garnish1.Quantity = Constants.GarnishCapacity;
                        Quantity1DisplayText = Garnish1.Quantity + " Left";
                        break;
                    case "Garnish2":
                        Garnish2.Quantity = Constants.GarnishCapacity;
                        Quantity2DisplayText = Garnish2.Quantity + " Left";
                        break;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
