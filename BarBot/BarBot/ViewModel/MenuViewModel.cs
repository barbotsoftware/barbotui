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
		private bool _shouldDisplaySearch = false;

		public MenuViewModel(INavigationServiceExtension navigationService)
		{
			_navigationService = navigationService;
			_recipes = new List<Recipe>();
			MessengerInstance.Register<bool>(this, shouldDisplaySearch =>
			{
				_shouldDisplaySearch = shouldDisplaySearch;
			});
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

		public bool ShouldDisplaySearch
		{
			get { return _shouldDisplaySearch; }
			set { Set(ref _shouldDisplaySearch, value); }
		}

		public void ShowDrinkDetailsCommand(string recipeIdentifier, byte[] imageContents)
		{
			// recipeIdentifier = name for Custom, recipeId otherwise
			MessengerInstance.Send(recipeIdentifier);
			if (imageContents != null)
			{
				MessengerInstance.Send(imageContents);
			}
			_navigationService.OpenModal(ViewModelLocator.DrinkDetailKey);
		}
	}
}
