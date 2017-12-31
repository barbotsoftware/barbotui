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

        public Core.Model.Garnish Garnish1
        {
            get { return garnish1; }
            set
            {
                garnish1 = value;
                OnPropertyChanged("Garnish1");
            }
        }

        public Core.Model.Garnish Garnish2
        {
            get { return garnish2; }
            set
            {
                garnish2 = value;
                OnPropertyChanged("Garnish2");
            }
        }

        public GarnishLoadContentDialog()
        {
            this.InitializeComponent();
            this.DataContext = this;

            app = Application.Current as App;

            this.Garnish1 = app.Garnishes.Where(g => g.OptionNumber == 1).First();
            this.Garnish2 = app.Garnishes.Where(g => g.OptionNumber == 2).First();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            sender.Hide();
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
                        app.webSocketService.UpdateGarnish(Garnish1);
                        break;
                    case "Garnish2":
                        Garnish2.Quantity = Constants.GarnishCapacity;
                        app.webSocketService.UpdateGarnish(Garnish2);
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
