using GalaSoft.MvvmLight.Ioc;

using Microsoft.Practices.ServiceLocation;

namespace BarBot.Core.ViewModel
{
	public class ViewModelLocator
	{
        public const string HomePageKey = "HomePage";
        public const string LoginPageKey = "LoginPage";
        public const string SignUpPageKey = "SignUpPage";
		public const string MenuPageKey = "MenuPage";
		public const string RecipeDetailPageKey = "RecipeDetailPage";
        public const string ContainersPageKey = "ContainersPage";

		static ViewModelLocator()
	    {
	    	ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            // Register ViewModels
            SimpleIoc.Default.Register<HomeViewModel>();
            SimpleIoc.Default.Register<LoginViewModel>();
            SimpleIoc.Default.Register<SignUpViewModel>();
	    	SimpleIoc.Default.Register<MenuViewModel>(true);
			SimpleIoc.Default.Register<RecipeDetailViewModel>(true);
            SimpleIoc.Default.Register<ContainersViewModel>(true);
	    }

        public HomeViewModel Home
        {
            get
            {
                return ServiceLocator.Current.GetInstance<HomeViewModel>();
            }
        }

        public LoginViewModel Login
		{
			get
			{
				return ServiceLocator.Current.GetInstance<LoginViewModel>();
			}
		}

        public SignUpViewModel SignUp
		{
			get
			{
				return ServiceLocator.Current.GetInstance<SignUpViewModel>();
			}
		}

	    public MenuViewModel Menu
	    {
	    	get
			{
				return ServiceLocator.Current.GetInstance<MenuViewModel>();
			}
	    }

		public RecipeDetailViewModel Detail
		{
			get
			{
				return ServiceLocator.Current.GetInstance<RecipeDetailViewModel>();
			}
		}

        public ContainersViewModel Containers
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ContainersViewModel>();
            }
        }


	    public static void Cleanup()
	    {
	    	// TODO Clear the ViewModels
	    }
	}
}
