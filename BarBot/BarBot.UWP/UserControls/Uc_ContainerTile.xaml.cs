using BarBot.Core.Model;
using System.ComponentModel;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace BarBot.UWP.UserControls
{
    public sealed partial class Uc_ContainerTile : UserControl, INotifyPropertyChanged
    {
        private Container container;
        private Ingredient ingredient;
        private string maxVolumeLabel;

        public Container Container
        {
            get { return container; }
            set
            {
                container = value;
                SetTextBlockColor();
                SetMaxVolumeLabel();
                OnPropertyChanged("Container");
            }
        }

        public Ingredient Ingredient
        {
            get { return ingredient; }
            set
            {
                ingredient = value;
                OnPropertyChanged("Ingredient");
            }
        }

        public string MaxVolumeLabel
        {
            get { return maxVolumeLabel; }
            set
            {
                maxVolumeLabel = value;
                OnPropertyChanged("MaxVolumeLabel");
            }
        }

        public Uc_ContainerTile()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        private void SetMaxVolumeLabel()
        {
            MaxVolumeLabel = "/ " + Container.MaxVolume + " oz";
        }

        private void SetTextBlockColor()
        {
            if (container.CurrentVolume > 80)
            {
                this.ContainerCurrentVolumeTextBlock.Foreground = new SolidColorBrush(Colors.Green);
            }
            else if (container.CurrentVolume > 60)
            {
                this.ContainerCurrentVolumeTextBlock.Foreground = new SolidColorBrush(Colors.YellowGreen);
            }
            else if (container.CurrentVolume > 40)
            {
                this.ContainerCurrentVolumeTextBlock.Foreground = new SolidColorBrush(Colors.DarkOrange);
            }
            else if (container.CurrentVolume > 20)
            {
                this.ContainerCurrentVolumeTextBlock.Foreground = new SolidColorBrush(Colors.OrangeRed);
            }
            else
            {
                this.ContainerCurrentVolumeTextBlock.Foreground = new SolidColorBrush(Colors.Red);
            }
        }

        private void Container_Click(object sender, RoutedEventArgs e)
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
