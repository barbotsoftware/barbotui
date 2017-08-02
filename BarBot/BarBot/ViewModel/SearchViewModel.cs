using BarBot.Core.Service.Navigation;

namespace BarBot.Core.ViewModel
{
    public class SearchViewModel
    {
        INavigationService navigationService;

        public SearchViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
        }
    }
}
