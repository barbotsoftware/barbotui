using Microsoft.Practices.ServiceLocation;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;

using BarBot.Core.Model;
using BarBot.Core.Service.Login;

namespace BarBot.Core.ViewModel
{
    public class SignUpViewModel: ViewModelBase
    {
		INavigationService navigationService;
		ILoginService loginService;

        RelayCommand goToHomeCommand;
        RelayCommand registerUserCommand;

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
					RegisterUserCommand.RaiseCanExecuteChanged();
				}
			}
		}

		public SignUpViewModel(INavigationService navigationService, ILoginService loginService)
		{
			this.navigationService = navigationService;
			this.loginService = loginService;
		}

		#region Command

		public RelayCommand GoToHomeCommand
		{
			get
			{
				return goToHomeCommand ?? (goToHomeCommand = new RelayCommand(GoToHomePage));
			}
		}

		public RelayCommand RegisterUserCommand
		{
			get
			{
				return registerUserCommand ??
					(registerUserCommand = new RelayCommand(
					async () =>
					{
						IsLoading = true;

						var result = await loginService.RegisterUser(Model.Name, Model.EmailAddress, Model.Password);

						if (result == null)
						{
							// Handle error when logging in
							var dialog = ServiceLocator.Current.GetInstance<IDialogService>();
							await
								dialog.ShowError(
									"Error signing up, please try again later.",
									"Error",
									"OK",
									null);
						}
	                    else
	                    {
                            Model = result;    
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
