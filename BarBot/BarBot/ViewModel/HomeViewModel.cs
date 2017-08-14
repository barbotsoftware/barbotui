using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;

namespace BarBot.Core.ViewModel
{
    public class HomeViewModel : ViewModelBase
    {
        INavigationService navigationService;

        RelayCommand goToLoginCommand;
        RelayCommand goToSignUpCommand;

        public string TitleText { get; set; }
        public string LoginButtonText { get; set; }
        public string SignUpButtonText { get; set; }

        public HomeViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;

            TitleText = AppResources.HomeTitleLabel;
            LoginButtonText = AppResources.LoginButtonLabel;
            SignUpButtonText = AppResources.SignUpButtonLabel;
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
            navigationService.NavigateTo(ViewModelLocator.LoginPageKey);
        }

        private void GoToSignUpPage()
        {
            navigationService.NavigateTo(ViewModelLocator.SignUpPageKey);
        }
    }
}
