using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Threading;
using GalaSoft.MvvmLight.Views;
using BarBot.Core.ViewModel;
using BarBot.Droid.View.Detail;
using BarBot.Droid.View.Menu;

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
					DispatcherHelper.Initialize();

					var nav = new NavigationService();

					SimpleIoc.Default.Register<INavigationService>(() => nav);

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
