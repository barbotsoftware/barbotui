using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using BarBot.Core.Service.Navigation;

namespace BarBot.Core.ViewModel
{
    public class HomeViewModel : ViewModelBase
    {
        INavigationService navigationService;

        RelayCommand goToLoginCommand;
        RelayCommand goToSignUpCommand;

        public string Title
        {
            get;
            private set;
        }

        public HomeViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;

            Title = "BarBot";
        }

		#region Command

		public RelayCommand GoToLoginCommand
		{
			get
			{
				return goToLoginCommand ?? (goToLoginCommand = new RelayCommand(GoToLoginPage));
			}
		}

		public RelayCommand GoToSignUpCommand
		{
			get
			{
                return goToSignUpCommand ?? (goToSignUpCommand = new RelayCommand(GoToSignUpPage));
			}
		}

        #endregion

        private void GoToLoginPage()
        {
            navigationService.NavigateTo("Login");
        }

        private void GoToSignUpPage()
        {
            navigationService.NavigateTo("SignUp");
        }
    }
}
