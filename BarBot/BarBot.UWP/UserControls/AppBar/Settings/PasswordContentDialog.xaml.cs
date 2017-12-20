using BarBot.UWP.Service.Login;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace BarBot.UWP.UserControls.AppBar.Settings
{
    public sealed partial class PasswordContentDialog : ContentDialog
    {
        private App app;
        private UWPLoginService loginService;

        public PasswordContentDialog()
        {
            this.InitializeComponent();

            this.app = Application.Current as App;
            loginService = app.loginService;
        }

        private async void PasswordDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var passwordBox = sender.Content as PasswordBox;
            var password = passwordBox.Password;

            if (string.IsNullOrEmpty(password))
            {
                args.Cancel = true;
                PasswordBox.Header = "Password is required.";
                PasswordBox.Focus(FocusState.Keyboard);
            }
            else
            {
                ContentDialogButtonClickDeferral deferral = args.GetDeferral();
                if (await loginService.LoginUser("barbot", app.barbotName, password))
                {
                    ((Window.Current.Content as Frame).Content as MainPage).ContentFrame.Navigate(typeof(Pages.ContainerPanel), null, new DrillInNavigationTransitionInfo());
                    sender.Hide();
                }
                else
                {
                    args.Cancel = true;
                    PasswordBox.Header = "Incorrect Password";
                    PasswordBox.Password = "";
                    PasswordBox.Focus(FocusState.Keyboard);
                }
                deferral.Complete();
            }
        }

        private void PasswordDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // Dismiss Dialog
            sender.Hide();
        }
    }
}
