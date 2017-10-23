using BarBot.Core.Model;
using BarBot.Core.WebSocket;
using BarBot.UWP.Websocket;
using BarBot.UWP.UserControls.ContainerPanel;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace BarBot.UWP.Pages
{
    /// <summary>
    /// Container Panel Page
    /// </summary>
    public sealed partial class ContainerPanel : Page
    {
        private UWPWebSocketService webSocketService;
        private List<Container> containers = new List<Container>();

        public List<Container> Containers
        {
            get { return containers; }
            set
            {
                containers = value;
                ContainerList.Containers = containers;
            }
        }

        public ContainerPanel()
        {
            this.InitializeComponent();
            webSocketService = (Application.Current as App).webSocketService;
        }

        private void NavigateBack(object sender, RoutedEventArgs e)
        {
            ((Window.Current.Content as Frame).Content as MainPage).ContentFrame.GoBack(new DrillInNavigationTransitionInfo());
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ContainerList.Visibility = Visibility.Collapsed;
            if (e.Parameter == null)
            {
                webSocketService.Socket.GetContainersEvent += Socket_GetContainersEvent;
                webSocketService.GetContainers();
            }
        }

        private async void Socket_GetContainersEvent(object sender, WebSocketEvents.GetContainersEventArgs args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High,
            () =>
            {
                Containers = args.Containers;
                ContainerList.Visibility = Visibility.Visible;
            });

            webSocketService.Socket.GetContainersEvent -= Socket_GetContainersEvent;
        }
    }
}
