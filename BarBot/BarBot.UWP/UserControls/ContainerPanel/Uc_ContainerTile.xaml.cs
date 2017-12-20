using BarBot.Core.Model;
using BarBot.UWP.Utils;
using BarBot.UWP.Websocket;
using System;
using System.ComponentModel;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace BarBot.UWP.UserControls.ContainerPanel
{
    public sealed partial class Uc_ContainerTile : UserControl, INotifyPropertyChanged
    {
        private UWPWebSocketService webSocketService;
        private Container container;
        private Ingredient ingredient;
        private string maxVolumeLabel;
        private SolidColorBrush volumeColor;

        public Container Container
        {
            get { return container; }
            set
            {
                container = value;
                SetTextBlockColor(container);
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
                ingredient.Name = Helpers.UppercaseWords(ingredient.Name);
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

        public SolidColorBrush VolumeColor
        {
            get { return volumeColor; }
            set
            {
                volumeColor = value;
                OnPropertyChanged("VolumeColor");
            }
        }

        public Uc_ContainerTile()
        {
            this.InitializeComponent();
            this.DataContext = this;
            webSocketService = (Application.Current as App).webSocketService;
        }

        private void SetMaxVolumeLabel()
        {
            MaxVolumeLabel = "/" + Container.MaxVolume + " oz";
        }


        public void SetTextBlockColor(Container container)
        {
            if (container.CurrentVolume > 80)
            {
                VolumeColor = new SolidColorBrush(Colors.Green);
            }
            else if (container.CurrentVolume > 60)
            {
                VolumeColor = new SolidColorBrush(Colors.YellowGreen);
            }
            else if (container.CurrentVolume > 40)
            {
                VolumeColor = new SolidColorBrush(Colors.DarkOrange);
            }
            else if (container.CurrentVolume > 20)
            {
                VolumeColor = new SolidColorBrush(Colors.OrangeRed);
            }
            else
            {
                VolumeColor = new SolidColorBrush(Colors.Red);
            }
        }

        private async void Container_Click(object sender, RoutedEventArgs e)
        {
            var containerLoadDialog = new ContainerLoadContentDialog(Container, Ingredient, this);
            await containerLoadDialog.ShowAsync();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
