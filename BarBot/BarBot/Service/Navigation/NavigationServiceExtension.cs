using GalaSoft.MvvmLight.Views;

namespace BarBot.Core.Service.Navigation
{
	public static class NavigationServiceExtension
	{
		public static INavigationServiceExtension Current { get; set; }

		public static void OpenModal(this INavigationService navigationService, string key)
		{
			Current.OpenModal(key);
		}

		public static void CloseModal(this INavigationService navigationService)
		{
			Current.CloseModal();
		}
	}
}
