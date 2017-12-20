using BarBot.UWP.UserControls.AppBar.Filter;
using BarBot.UWP.UserControls.AppBar.Search;
using BarBot.UWP.UserControls.AppBar.Settings;
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

        private async void Open_Search(object sender, RoutedEventArgs e)
        {
            var searchDialog = new SearchContentDialog();
            await searchDialog.ShowAsync();
        }

        private async void Open_Filter(object sender, RoutedEventArgs e)
        {
            var filterDialog = new FilterContentDialog();
            await filterDialog.ShowAsync();
        }

        private async void Open_Settings(object sender, RoutedEventArgs e)
        {
            var passwordDialog = new PasswordContentDialog();
            await passwordDialog.ShowAsync();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
