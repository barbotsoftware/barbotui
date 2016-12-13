using GalaSoft.MvvmLight; using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation; 
namespace BarBot.Core.ViewModel
{
	public class ViewModelLocator
	{
		public const string DrinkDetailKey = "DrinkDetail";

		public ViewModelLocator() 	    { 	    	ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default); 	    	SimpleIoc.Default.Register<MenuViewModel>(); 	    }  	    public MenuViewModel Menu 	    { 	    	get 			{ 				return ServiceLocator.Current.GetInstance<MenuViewModel>();  			} 	    }   	    public static void Cleanup() 	    { 	    	// TODO Clear the ViewModels 	    }
	}
}
