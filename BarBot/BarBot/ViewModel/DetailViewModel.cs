using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using BarBot.Core.Model;

namespace BarBot.Core.ViewModel
{
	public class DetailViewModel : ViewModelBase
	{
		private readonly INavigationService _navigationService;
		private string _title;

		private Recipe _recipe;

		public DetailViewModel(INavigationService navigationService)
		{
			_navigationService = navigationService;
		}

		public Recipe Recipe
		{
			get { return _recipe; }
			set { Set(ref _recipe, value); }
		}

		public string Title
		{
			get { return _title; }
			set { Set(ref _title, value); }
		}
	}
}
