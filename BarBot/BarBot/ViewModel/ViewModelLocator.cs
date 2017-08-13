using GalaSoft.MvvmLight.Ioc;

using Microsoft.Practices.ServiceLocation;

using BarBot.Core.Service.WebSocket; 
namespace BarBot.Core.ViewModel
{
	public class ViewModelLocator
	{
        public const string HomePageKey = "HomePage";
        public const string LoginPageKey = "LoginPage";
        public const string SignUpPageKey = "SignUpPage"; 		public const string MenuPageKey = "MenuPage";
		public const string RecipeDetailPageKey = "RecipeDetailPage";

		static ViewModelLocator() 	    { 	    	ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            // Register Services
            SimpleIoc.Default.Register<IWebSocketService, WebSocketService>();

            // Register ViewModels
            SimpleIoc.Default.Register<HomeViewModel>();
            SimpleIoc.Default.Register<LoginViewModel>();
            SimpleIoc.Default.Register<SignUpViewModel>(); 	    	SimpleIoc.Default.Register<MenuViewModel>(); 			SimpleIoc.Default.Register<DetailViewModel>(); 	    }

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
		}  	    public MenuViewModel Menu 	    { 	    	get 			{ 				return ServiceLocator.Current.GetInstance<MenuViewModel>(); 			} 	    }  		public DetailViewModel Detail 		{ 			get 			{ 				return ServiceLocator.Current.GetInstance<DetailViewModel>(); 			} 		}   	    public static void Cleanup() 	    { 	    	// TODO Clear the ViewModels 	    }
	}
}
