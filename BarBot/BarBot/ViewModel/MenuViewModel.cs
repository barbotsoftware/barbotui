using GalaSoft.MvvmLight;

using System.Collections.Generic;

using BarBot.Core.Model;

namespace BarBot.Core.ViewModel
{
	public class MenuViewModel : ViewModelBase
	{
		private readonly INavigationServiceExtension _navigationService;
		private string _title;
		private List<Recipe> _recipes;
		private List<Ingredient> _ingredients;

		public MenuViewModel(INavigationServiceExtension navigationService)
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

		public List<Ingredient> Ingredients
		{
			get { return _ingredients; }
			set { Set(ref _ingredients, value); }
		}

		public void ShowDrinkDetailsCommand(string obj, byte[] imageContents)
		{
			MessengerInstance.Send(obj);
			MessengerInstance.Send(imageContents);
			_navigationService.OpenModal(ViewModelLocator.DrinkDetailKey);
		}
	}
}
