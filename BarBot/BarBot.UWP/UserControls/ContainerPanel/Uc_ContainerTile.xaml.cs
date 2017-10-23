using BarBot.Core.Model;
using BarBot.UWP.Utils;
using BarBot.UWP.Websocket;
using System;
using System.ComponentModel;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace BarBot.UWP.UserControls.ContainerPanel
{
    public sealed partial class Uc_ContainerTile : UserControl, INotifyPropertyChanged
    {
        private UWPWebSocketService webSocketService;

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
            webSocketService = (Application.Current as App).webSocketService;
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
            // Show Dialog
            DisplayContainerLoadDialog();
        }

        private async void DisplayContainerLoadDialog()
        {
            string loadDialogText = "Please load " + Helpers.UppercaseWords(Ingredient.Name) + " into Container " + Container.Number + ".";

            var containerLoadDialog = new ContentDialog()
            {
                MaxWidth = 1920,
                Content = new TextBlock()
                {
                    Text = loadDialogText,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    FontSize = 45
                },
                Background = new SolidColorBrush(Color.FromArgb(255, 22, 22, 22)),
                Foreground = new SolidColorBrush(Windows.UI.Colors.White),
                BorderBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(100, 34, 34, 34)),
                FontFamily = new FontFamily("Microsoft Yi Baiti"),
                IsPrimaryButtonEnabled = true,
                IsSecondaryButtonEnabled = true,
                PrimaryButtonText = "DONE",
                SecondaryButtonText = "CANCEL"
            };

            containerLoadDialog.PrimaryButtonClick += ContainerLoadDialog_PrimaryButtonClick;
            containerLoadDialog.SecondaryButtonClick += ContainerLoadDialog_SecondaryButtonClick;
            await containerLoadDialog.ShowAsync();
        }

        private void ContainerLoadDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // Update Current Volume
            Container.CurrentVolume = Container.MaxVolume;

            // Update Current Volume Color
            SetTextBlockColor();

            // Call Update Container
            webSocketService.UpdateContainer(Container);

            sender.Hide();
        }

        private void ContainerLoadDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // Dismiss Dialog
            sender.Hide();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
