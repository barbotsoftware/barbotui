using System.Linq;
using System.Collections.Generic;

using GalaSoft.MvvmLight;

using BarBot.Core.Model;

namespace BarBot.Core.ViewModel
{
	public class DetailViewModel : ViewModelBase
	{
		private readonly INavigationServiceExtension _navigationService;
		private string _recipeId;
		private Recipe _recipe;
		private byte[] _imageContents;
		private List<double> _quantities;
		private List<Ingredient> _ingredients;
		private List<Ingredient> _availableIngredients;
		private List<Ingredient> _ingredientsInBarBot;
		private bool _isCustomRecipe;

		public DetailViewModel(INavigationServiceExtension navigationService)
		{
			_navigationService = navigationService;
			MessengerInstance.Register<string>(this, passedString =>
			{
				if (passedString.StartsWith("recipe_", System.StringComparison.CurrentCulture))
				{
					RecipeId = passedString;
				}
				else
				{
					// Custom Recipe
					Recipe = new Recipe(Constants.CustomRecipeId, 
					                    passedString,
					                    null,
					                    new List<Ingredient>());
					RecipeId = Constants.CustomRecipeId;
				}
			});
			MessengerInstance.Register<byte[]>(this, imageContents =>
			{
				ImageContents = imageContents;
			});

			_ingredients = new List<Ingredient>();
			_availableIngredients = new List<Ingredient>();
			_ingredientsInBarBot = new List<Ingredient>();
			_quantities = new List<double>();
			for (var i = 0.5; i <= Constants.MaxVolume; i += 0.5)
			{
				Quantities.Add(i);
			}
		}

		public string RecipeId
		{
			get { return _recipeId; }
			set { Set(ref _recipeId, value); }
		}

		public Recipe Recipe
		{
			get { return _recipe; }
			set 
			{ 
				Set(ref _recipe, value);
				Ingredients = _recipe.Ingredients;
			}
		}

		public List<Ingredient> Ingredients
		{
			get { return _ingredients; }
			set { Set(ref _ingredients, value); }
		}

		public byte[] ImageContents
		{
			get { return _imageContents; }
			set { Set(ref _imageContents, value); }
		}

		public List<double> Quantities
		{
			get { return _quantities; }
			set { Set(ref _quantities, value); }
		}

		public List<Ingredient> AvailableIngredients
		{
			get { return _availableIngredients; }
			set { Set(ref _availableIngredients, value); }
		}

		public List<Ingredient> IngredientsInBarBot
		{
			get { return _ingredientsInBarBot; }
			set { Set(ref _ingredientsInBarBot, value); }
		}

		public bool IsCustomRecipe
		{
			get { return _isCustomRecipe; }
			set { Set(ref _isCustomRecipe, value); }
		}

		public void Clear()
		{
			RecipeId = null;
			ImageContents = null;
			IsCustomRecipe = false;
			Ingredients.Clear();
			AvailableIngredients.Clear();
		}

		// Sets AvailableIngredients equal to Ingredients in BarBot - Ingredients in Recipe
		public void RefreshAvailableIngredients()
		{
			AvailableIngredients = IngredientsInBarBot.ToList();

			// Remove Ingredients already in Recipe
			foreach (Ingredient recipeIngredient in Ingredients)
			{
				AvailableIngredients.RemoveAll(availableIngredient => availableIngredient.Name.Equals(recipeIngredient.Name));
			}
		}

		public void ShowDrinkMenuCommand(bool shouldDisplaySearch)
		{
			MessengerInstance.Send(shouldDisplaySearch);
			_navigationService.CloseModal();
		}
	}
}
