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
        private string garnish1DisplayText;
        private string garnish2DisplayText;
        private int garnish1PreviousQuantity;
        private int garnish2PreviousQuantity;

        public Core.Model.Garnish Garnish1
        {
            get { return garnish1; }
            set
            {
                garnish1 = value;
                Garnish1DisplayText = SetGarnishDisplayText(garnish1);
                OnPropertyChanged("Garnish1");
            }
        }

        public Core.Model.Garnish Garnish2
        {
            get { return garnish2; }
            set
            {
                garnish2 = value;
                Garnish2DisplayText = SetGarnishDisplayText(garnish2);
                OnPropertyChanged("Garnish2");
            }
        }

        public string Garnish1DisplayText
        {
            get { return garnish1DisplayText; }
            set
            {
                garnish1DisplayText = value;
                OnPropertyChanged("Garnish1DisplayText");
            }
        }

        public string Garnish2DisplayText
        {
            get { return garnish2DisplayText; }
            set
            {
                garnish2DisplayText = value;
                OnPropertyChanged("Garnish2DisplayText");
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

        private string SetGarnishDisplayText(Core.Model.Garnish garnish)
        {
            if (garnish.Quantity == 0)
            {
                return string.Format("No {0}s Left", garnish.Name);
            }
            else if (garnish.Quantity == 1)
            {
                return string.Format("{0} {1} Left", garnish.Quantity, garnish.Name);
            }
            else
            {
                return string.Format("{0} {1}s Left", garnish.Quantity, garnish.Name);
            }
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
                        Garnish1DisplayText = SetGarnishDisplayText(Garnish1);
                        break;
                    case "Garnish2":
                        Garnish2.Quantity = Constants.GarnishCapacity;
                        Garnish2DisplayText = SetGarnishDisplayText(Garnish2);
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
