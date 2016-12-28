﻿using GalaSoft.MvvmLight;
using System.Collections.Generic;
using BarBot.Core.Model;

namespace BarBot.Core.ViewModel
{
	public class DetailViewModel : ViewModelBase
	{
		private readonly INavigationServiceExtension _navigationService;
		private string _recipeId;
		private Recipe _recipe;
		private List<Ingredient> _ingredients;
		private byte[] _imageContents;

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

		public void Clear()
		{
			RecipeId = null;
			ImageContents = null;
			Ingredients.Clear();
		}

		public void ShowDrinkMenuCommand(bool shouldDisplaySearch)
		{
			MessengerInstance.Send(shouldDisplaySearch);
			_navigationService.CloseModal();
		}
	}
}
