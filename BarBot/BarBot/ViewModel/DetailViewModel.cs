using GalaSoft.MvvmLight;
using System.Collections.Generic;
using BarBot.Core.Model;

namespace BarBot.Core.ViewModel
{
	public class DetailViewModel : ViewModelBase
	{
		private string _title;
		private string _recipeId;
		private Recipe _recipe;
		private List<Ingredient> _ingredients;
		private byte[] _imageContents;

		public DetailViewModel()
		{
			MessengerInstance.Register<string>(this, recipeId => 
			{
				RecipeId = recipeId;
			});
			MessengerInstance.Register<byte[]>(this, imageContents =>
			{
				ImageContents = imageContents;
			});
			_ingredients = new List<Ingredient>();
		}

		public string Title
		{
			get { return _title; }
			set { Set(ref _title, value); }
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
			Ingredients.Clear();
		}
	}
}
