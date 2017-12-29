using BarBot.Core.Model;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;

namespace BarBot.UWP.UserControls.Dialogs
{
    public sealed partial class ContainerVolumeContentDialog : ContentDialog, INotifyPropertyChanged
    {
        private string displayText;

        public string DisplayText
        {
            get { return displayText; }
            set
            {
                displayText = value;
                OnPropertyChanged("DisplayText");
            }
        }

        public ContainerVolumeContentDialog(Container container, 
                                            Ingredient ingredient,
                                            Recipe recipe)
        {
            this.InitializeComponent();
            this.DataContext = this;

            this.DisplayText = string.Format("There is not enough {0} to pour your {1}. Please reload Container {2}.", 
                ingredient.Name, recipe.Name, container.Number);
        }

        private void Dialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            args.Cancel = true;
            sender.Hide();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
