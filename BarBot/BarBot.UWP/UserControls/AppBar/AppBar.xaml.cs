using System;
using System.ComponentModel;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace BarBot.UWP.UserControls.AppBar
{
    public sealed partial class AppBar : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register
        (
            "Title",
            typeof(string),
            typeof(AppBar),
            new PropertyMetadata(string.Empty)
        );

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set
            {
                SetValue(TitleProperty, value);
                OnPropertyChanged("Title");
            }
        }

        public static readonly DependencyProperty BackButtonVisibleProperty = DependencyProperty.Register
        (
            "BackButtonVisible",
            typeof(bool),
            typeof(AppBar),
            new PropertyMetadata(false)
        );

        public bool BackButtonVisible
        {
            get { return (bool)GetValue(BackButtonVisibleProperty); }
            set
            {
                SetValue(BackButtonVisibleProperty, value);
                OnPropertyChanged("BackButtonVisible");
            }
        }

        public static readonly DependencyProperty SearchButtonVisibleProperty = DependencyProperty.Register
        (
            "SearchButtonVisible",
            typeof(bool),
            typeof(AppBar),
            new PropertyMetadata(false)
        );

        public bool SearchButtonVisible
        {
            get { return (bool)GetValue(SearchButtonVisibleProperty); }
            set
            {
                SetValue(SearchButtonVisibleProperty, value);
                OnPropertyChanged("SearchButtonVisible");
            }
        }

        public static readonly DependencyProperty FilterButtonVisibleProperty = DependencyProperty.Register
        (
            "FilterButtonVisible",
            typeof(bool),
            typeof(AppBar),
            new PropertyMetadata(false)
        );

        public bool FilterButtonVisible
        {
            get { return (bool)GetValue(FilterButtonVisibleProperty); }
            set
            {
                SetValue(FilterButtonVisibleProperty, value);
                OnPropertyChanged("FilterButtonVisible");
            }
        }

        public static readonly DependencyProperty SettingsButtonVisibleProperty = DependencyProperty.Register
        (
            "SettingsButtonVisible",
            typeof(bool),
            typeof(AppBar),
            new PropertyMetadata(false)
        );

        public bool SettingsButtonVisible
        {
            get { return (bool)GetValue(SettingsButtonVisibleProperty); }
            set
            {
                SetValue(SettingsButtonVisibleProperty, value);
                OnPropertyChanged("SettingsButtonVisible");
            }
        }

        public AppBar()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        private void NavigateBack(object sender, RoutedEventArgs e)
        {
            ((Window.Current.Content as Frame).Content as MainPage).ContentFrame.GoBack(new DrillInNavigationTransitionInfo());
        }

        private void Open_Search(object sender, RoutedEventArgs e)
        {

        }

        private void Open_Filter(object sender, RoutedEventArgs e)
        {

        }

        private void Open_Settings(object sender, RoutedEventArgs e)
        {
            DisplayPasswordDialog();
        }

        private async void DisplayPasswordDialog()
        {
            var passwordDialog = new ContentDialog()
            {
                MaxWidth = 1280,
                Content = new PasswordBox()
                {
                    Width = 500,
                    PlaceholderText = "Password",
                    PasswordRevealMode = PasswordRevealMode.Hidden,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    FontSize = 40
                },
                Background = new SolidColorBrush(Color.FromArgb(255, 22, 22, 22)),
                Foreground = new SolidColorBrush(Windows.UI.Colors.White),
                BorderBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(100, 34, 34, 34)),
                FontFamily = new FontFamily("Microsoft Yi Baiti"),
                IsPrimaryButtonEnabled = true,
                IsSecondaryButtonEnabled = true,
                PrimaryButtonText = "LOGIN",
                SecondaryButtonText = "CANCEL"
            };

            passwordDialog.PrimaryButtonClick += PasswordDialog_PrimaryButtonClick;
            passwordDialog.SecondaryButtonClick += PasswordDialog_SecondaryButtonClick;
            await passwordDialog.ShowAsync();
        }

        private void PasswordDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var password = (sender.Content as PasswordBox).Password;

            // use Contants.BarbotId && password to login via HTTP Client

            ((Window.Current.Content as Frame).Content as MainPage).ContentFrame.Navigate(typeof(Pages.ContainerPanel), null, new DrillInNavigationTransitionInfo());

            sender.Hide();
        }

        private void PasswordDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
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
