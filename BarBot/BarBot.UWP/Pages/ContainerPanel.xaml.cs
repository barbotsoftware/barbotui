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
        private App app;

        public ContainerPanel()
        {
            this.InitializeComponent();
            app = Application.Current as App;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ContainerList.Visibility = Visibility.Collapsed;
            if (e.Parameter == null)
            {
                app.webSocketService.Socket.GetContainersEvent += Socket_GetContainersEvent;
                app.webSocketService.GetContainers();
            }
        }

        private async void Socket_GetContainersEvent(object sender, WebSocketEvents.GetContainersEventArgs args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High,
            () =>
            {
                if (ProgressRing.IsActive)
                {
                    ProgressRing.IsActive = false;
                    ProgressRing.Visibility = Visibility.Collapsed;
                }

                app.Containers = args.Containers;
                ContainerList.Containers = args.Containers;
                ContainerList.Visibility = Visibility.Visible;
            });

            app.webSocketService.Socket.GetContainersEvent -= Socket_GetContainersEvent;
        }
    }
}
