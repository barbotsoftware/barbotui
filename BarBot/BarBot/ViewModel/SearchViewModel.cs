using GalaSoft.MvvmLight;

using BarBot.Core.Service.Navigation;

namespace BarBot.Core.ViewModel
{
    public class SearchViewModel : ViewModelBase
    {
        INavigationService navigationService;

        public SearchViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
        }
    }
}
