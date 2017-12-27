using BarBot.UWP.IO;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BarBot.UWP.UserControls.RecipeDetail.Dialogs
{
    public sealed partial class CupContentDialog : ContentDialog
    {
        private BarbotIOController barbotIOController;
        public bool ShouldProceed { get; set; }

        public CupContentDialog()
        {
            this.InitializeComponent();

            this.barbotIOController = (Application.Current as App).barbotIOController;
            this.ShouldProceed = false;
        }

        private void CupDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (barbotIOController != null)
            {
                barbotIOController.CupCount = 25;
            }
            ShouldProceed = true;
            sender.Hide();
        }

        private void CupDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (barbotIOController != null)
            {
                barbotIOController.CupCount = 1;
            }
            ShouldProceed = true;
            sender.Hide();
        }

        private void ContentDialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            args.Cancel = true;
            ShouldProceed = false;
            sender.Hide();
        }
    }
}
