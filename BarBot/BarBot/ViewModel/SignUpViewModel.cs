using BarBot.Core.Service.Navigation;
using BarBot.Core.Service.Rest;

namespace BarBot.Core.ViewModel
{
    public class SignUpViewModel
    {
		INavigationService navigationService;
		IRestService restService;

		public SignUpViewModel(INavigationService navigationService, IRestService restService)
		{
			this.navigationService = navigationService;
			this.restService = restService;
		}
    }
}
