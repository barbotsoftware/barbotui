using BarBot.Core.Service.Navigation;
using BarBot.Core.Service.Rest;

namespace BarBot.Core.ViewModel
{
    public class LoginViewModel
    {
		INavigationService navigationService;
		IRestService restService;

        public LoginViewModel(INavigationService navigationService, IRestService restService)
        {
            this.navigationService = navigationService;
            this.restService = restService;
        }
    }
}
