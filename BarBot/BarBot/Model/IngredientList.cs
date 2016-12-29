using System.Collections.Generic;

namespace BarBot.Core.Model
{
	public class IngredientList : JsonModelObject
	{
		private List<Ingredient> _ingredients;

		public IngredientList()
		{
		}

		public IngredientList(string json)
		{
			Ingredients = (List<Ingredient>)parseJSON(json, typeof(List<Ingredient>));
		}

		public List<Ingredient> Ingredients 
		{
			get { return _ingredients; }
			set { _ingredients = value; }
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
