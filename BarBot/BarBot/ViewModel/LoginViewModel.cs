using Microsoft.Practices.ServiceLocation;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;

using BarBot.Core.Model;
using BarBot.Core.Service.Login;

namespace BarBot.Core.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
		INavigationService navigationService;
		ILoginService loginService;

        RelayCommand goToHomeCommand;
        RelayCommand loginUserCommand;
        RelayCommand forgotPasswordCommand;

        private bool isLoading;

        public User Model
        {
            get;
            set;
        }

		public bool IsLoading
		{
			get { return isLoading; }
			set
			{
				if (Set(ref isLoading, value))
				{
					GoToHomeCommand.RaiseCanExecuteChanged();
					LoginUserCommand.RaiseCanExecuteChanged();
				}
			}
		}

        public LoginViewModel(INavigationService navigationService, ILoginService loginService, User model)
        {
            this.navigationService = navigationService;
            this.loginService = loginService;

            Model = model;
        }

		#region Command

		public RelayCommand GoToHomeCommand
		{
			get
			{
				return goToHomeCommand ?? (goToHomeCommand = new RelayCommand(GoToHomePage));
			}
		}

		public RelayCommand LoginUserCommand
		{
			get
			{
				return loginUserCommand ?? 
                    (loginUserCommand = new RelayCommand(
					async () =>
                    {
                        IsLoading = true;

                        var result = await loginService.LoginUser(Model.EmailAddress, Model.Password);

                        if (!result)
                        {
							// Handle error when logging in
							var dialog = ServiceLocator.Current.GetInstance<IDialogService>();
							await
								dialog.ShowError(
									"Error logging in, please try again later.",
									"Error",
									"OK",
									null);
                        }

                        IsLoading = false;
                    }, 
                    () => !IsLoading));
			}
		}

        public RelayCommand ForgotPasswordCommand
        {
            get
            {
                return forgotPasswordCommand ??
                    (forgotPasswordCommand = new RelayCommand(
					async () =>
					{
						IsLoading = true;

						var result = await loginService.ForgotPassword(Model.EmailAddress);

						if (!result)
						{
							// Handle error when logging in
							var dialog = ServiceLocator.Current.GetInstance<IDialogService>();
							await
								dialog.ShowError(
									"Error logging in, please try again later.",
									"Error",
									"OK",
									null);
						}

						IsLoading = false;
					},
					() => !IsLoading));
            }
        }

		#endregion

        private void GoToHomePage()
        {
            navigationService.NavigateTo(ViewModelLocator.HomeKey);
        }
	}
}
