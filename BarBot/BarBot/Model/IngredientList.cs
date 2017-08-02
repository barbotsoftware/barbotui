using System.Collections.Generic;

namespace BarBot.Core.Model
{
	public class IngredientList : JsonModelObject
	{
        List<Ingredient> ingredients;

		public List<Ingredient> Ingredients
		{
			get
			{
				return ingredients;
			}

			set
			{
				ingredients = value;
			}
		}

		public IngredientList()
		{
		}

		public IngredientList(string json)
		{
			Ingredients = (List<Ingredient>)parseJSON(json, typeof(List<Ingredient>));
		}

		public string GetIngredientName(Ingredient ingredient)
		{
			foreach (Ingredient i in Ingredients)
			{
				if (i.IngredientId.Equals(ingredient.IngredientId))
				{
					return i.Name;
				}
			}
			return "";
		}
	}
}
