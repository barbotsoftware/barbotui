using System.ComponentModel;
using Windows.UI.Xaml.Controls;

namespace BarBot.UWP.UserControls.RecipeDetail.Dialogs
{
    public sealed partial class IceContentDialog : ContentDialog, INotifyPropertyChanged
    {
        Uc_RecipeDetail parentUserControl;
        public bool ShouldProceed { get; set; }
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

        public IceContentDialog(Uc_RecipeDetail parent)
        {
            this.InitializeComponent();
            this.DataContext = this;

            this.parentUserControl = parent;
            this.ShouldProceed = false;
            this.DisplayText = string.Format("Add Ice to your {0}?", parent.Recipe.Name);
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            parentUserControl.Ice = true;
            ShouldProceed = true;
            sender.Hide();
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            parentUserControl.Ice = false;
            ShouldProceed = true;
            sender.Hide();
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
