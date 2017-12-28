using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace BarBot.UWP.UserControls.RecipeDetail.Dialogs
{
    public sealed partial class GarnishContentDialog : ContentDialog
    {
        public Boolean ShouldProceed { get; set; }

        public GarnishContentDialog()
        {
            this.InitializeComponent();

            ShouldProceed = false;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
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
