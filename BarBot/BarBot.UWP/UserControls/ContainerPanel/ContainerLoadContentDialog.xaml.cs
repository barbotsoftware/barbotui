using BarBot.Core.Model;
using BarBot.UWP.Utils;
using BarBot.UWP.Websocket;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BarBot.UWP.UserControls.ContainerPanel
{
    public sealed partial class ContainerLoadContentDialog : ContentDialog, INotifyPropertyChanged
    {
        private UWPWebSocketService webSocketService;
        private Container container;
        private Ingredient ingredient;
        private Uc_ContainerTile containerTile;
        private string text;

        public string Text
        {
            get
            {
                return text;
            }

            set
            {
                text = value;
                OnPropertyChanged("Text");
            }
        }

        public ContainerLoadContentDialog(Container container, Ingredient ingredient, Uc_ContainerTile containerTile)
        {
            this.InitializeComponent();
            this.DataContext = this;

            this.container = container;
            this.ingredient = ingredient;
            this.containerTile = containerTile;

            this.webSocketService = (Application.Current as App).webSocketService;
            this.Text = "Please load " + Helpers.UppercaseWords(ingredient.Name) +
                    " into Container " + container.Number + ".";
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // Update Current Volume
            container.CurrentVolume = container.MaxVolume;

            // Set TextBlock Color on ContainerTile
            containerTile.SetTextBlockColor(container);

            // Call Update Container
            webSocketService.UpdateContainer(container);

            sender.Hide();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
