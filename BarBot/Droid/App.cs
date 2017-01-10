using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Threading;
using GalaSoft.MvvmLight.Views;
using BarBot.Core.ViewModel;
using BarBot.Droid.View.Detail;
using BarBot.Droid.View.Menu;
using BarBot.Droid.Util;

namespace BarBot.Droid
{
	public static class App
	{
		private static ViewModelLocator locator;

		public static ViewModelLocator Locator
		{
			get
			{
				if (locator == null)
				{
					// MVVMLight's DispatcherHelper for cross-thread handling.
					DispatcherHelper.Initialize();

					// Initialize NavigationService
					var nav = new Util.NavigationServiceExtension();
					nav.Initialize();

					// Register NavigationService interfaces
					SimpleIoc.Default.Register<INavigationService>(() => nav);
					SimpleIoc.Default.Register<INavigationServiceExtension>(() => nav);

					// Configure PageKeys
					nav.Configure(
					  ViewModelLocator.DrinkMenuKey,
					  typeof(DrinkMenuActivity));

					nav.Configure(
					  ViewModelLocator.DrinkDetailKey,
					  typeof(DrinkDetailActivity));

					locator = new ViewModelLocator();
				}

				return locator;
			}
		}
	}
}
