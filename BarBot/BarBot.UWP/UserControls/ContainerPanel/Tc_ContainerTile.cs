using BarBot.Core.Model;
using BarBot.UWP.Utils;
using BarBot.UWP.Websocket;
using System;
using System.ComponentModel;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace BarBot.UWP.UserControls.ContainerPanel
{
    public sealed class Tc_ContainerTile : Control, INotifyPropertyChanged
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

        public Tc_ContainerTile()
        {
            this.DefaultStyleKey = typeof(Tc_ContainerTile);
            this.DataContext = this;
            webSocketService = (Application.Current as App).webSocketService;
        }

        private void SetMaxVolumeLabel()
        {
            MaxVolumeLabel = "/" + Container.MaxVolume + " oz";
        }

        public void SetTextBlockColor(Container container)
        {
            if (container.CurrentVolume > (container.MaxVolume * .8))
            {
                VolumeColor = new SolidColorBrush(Colors.Green);
            }
            else if (container.CurrentVolume > (container.MaxVolume * .6))
            {
                VolumeColor = new SolidColorBrush(Colors.YellowGreen);
            }
            else if (container.CurrentVolume > (container.MaxVolume * .4))
            {
                VolumeColor = new SolidColorBrush(Colors.DarkOrange);
            }
            else if (container.CurrentVolume > (container.MaxVolume * .2))
            {
                VolumeColor = new SolidColorBrush(Colors.OrangeRed);
            }
            else
            {
                VolumeColor = new SolidColorBrush(Colors.Red);
            }
        }

        protected override void OnPointerPressed(PointerRoutedEventArgs e)
        {
            this.CapturePointer(e.Pointer);
            VisualStateManager.GoToState(this, "PointerDown", true);
        }

        protected override void OnPointerReleased(PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "PointerUp", true);
            this.ReleasePointerCapture(e.Pointer);
            ShowContainerDialog();
        }

        private async void ShowContainerDialog()
        {
            var containerLoadDialog = new ContainerLoadContentDialog(Container, Ingredient, this);
            await containerLoadDialog.ShowAsync();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
