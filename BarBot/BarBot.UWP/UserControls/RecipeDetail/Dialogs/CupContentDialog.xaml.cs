using BarBot.UWP.IO;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BarBot.UWP.UserControls.RecipeDetail.Dialogs
{
    public sealed partial class CupContentDialog : ContentDialog
    {
        private BarbotIOController barbotIOController;

        public CupContentDialog()
        {
            this.InitializeComponent();

            this.barbotIOController = (Application.Current as App).barbotIOController;
        }

        private void CupDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            barbotIOController.CupCount = 25;

            sender.Hide();
        }

        private void CupDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            barbotIOController.CupCount = 1;

            sender.Hide();
        }

        private void ContentDialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // TODO: Attach event handler to close button click to dismiss on cancel and not pour
            sender.Hide();
        }
    }
}
