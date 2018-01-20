using System;
using System.Collections.Generic;

using BarBot.Core.Model;
using BarBot.Core.Service.Navigation;
using BarBot.Core.Service.WebSocket;
using GalaSoft.MvvmLight;

namespace BarBot.Core.ViewModel
{
    public class ContainersViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly IWebSocketService webSocketService;

        private List<Container> containers;

        public ContainersViewModel(INavigationService navigationService,
                                   IWebSocketService webSocketService)
        {
            this.navigationService = navigationService;
            this.webSocketService = webSocketService;

            containers = new List<Container>();
            Title = "Containers";
        }

        public string Title
        {
            get;
            set;
        }

        public List<Container> Containers
        {
            get { return containers; }
            set { Set(ref containers, value); }
        }

        public void ShowMenuCommand()
        {
            this.navigationService.CloseModal();
        }
    }
}
