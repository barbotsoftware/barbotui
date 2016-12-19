using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;

using System.Collections.Generic;

using BarBot.Core.Model;


namespace BarBot.Core.ViewModel
{
	public class MenuViewModel : ViewModelBase
	{
		private readonly INavigationService _navigationService;
		private string _title;
		private List<Recipe> _recipes;

		public MenuViewModel(INavigationService navigationService)
		{
			_navigationService = navigationService;
			_recipes = new List<Recipe>();

			Title = "DRINK MENU";
		}

		public string Title
		{
			get { return _title; }
			set { Set(ref _title, value); }
		}

		public List<Recipe> Recipes
		{
			get { return _recipes; }
			set { Set(ref _recipes, value); }
		}

		public void ShowDrinkDetailsCommand(string obj)
		{
			MessengerInstance.Send(obj);
			_navigationService.NavigateTo(ViewModelLocator.DrinkDetailKey);
		}
	}
}
