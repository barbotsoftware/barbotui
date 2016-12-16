using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;

using BarBot.Core.Model;

namespace BarBot.Core.ViewModel
{
	public class MenuViewModel : ViewModelBase
	{
		private readonly INavigationService _navigationService;
		private string _title;
		private Recipe[] _recipes;
		private RelayCommand _navigateCommand;

		public MenuViewModel(INavigationService navigationService)
		{
			_navigationService = navigationService;

			Title = "DRINK MENU";
		}

		public string Title
		{
			get { return _title; }
			set { Set(ref _title, value); }
		}

		public Recipe[] Recipes
		{
			get { return _recipes; }
			set { Set(ref _recipes, value); }
		}

		public RelayCommand NavigateCommand
		{
			get
			{
				return _navigateCommand
					?? (_navigateCommand = new RelayCommand(() => _navigationService.NavigateTo(
							ViewModelLocator.DrinkDetailKey)));
			}
		}
	}
}
