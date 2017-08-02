namespace BarBot.Core.Service.Navigation
{
	public static class NavigationService
	{
		public static INavigationService Current { get; set; }

		public static void OpenModal(this GalaSoft.MvvmLight.Views.INavigationService navigationService, string key)
		{
			Current.OpenModal(key);
		}

		public static void CloseModal(this GalaSoft.MvvmLight.Views.INavigationService navigationService)
		{
			Current.CloseModal();
		}
	}
}
