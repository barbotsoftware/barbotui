using System.Collections.Generic;

namespace BarBot.Core.Model
{
	public class IngredientList
	{
		private List<Ingredient> _ingredients;

		public IngredientList()
		{
			_ingredients = new List<Ingredient>();
		}

		public List<Ingredient> Ingredients { get; set; }

		public Ingredient GetIngredient(string ingredientId)
		{
			foreach (Ingredient i in Ingredients)
			{
				if (i.IngredientId.Equals(ingredientId))
				{
					return i;
				}
			}
			return null;
		}

		public List<Ingredient> GetIngredientsForIngredientIds(List<Ingredient> list)
		{
			var newList = new List<Ingredient>();
			foreach (Ingredient i in list)
			{
				newList.Add(GetIngredient(i.IngredientId));
			}
			return newList;
		}
	}
}
