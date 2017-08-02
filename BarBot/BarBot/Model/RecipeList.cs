using System.Collections.Generic;

namespace BarBot.Core.Model
{
	public class RecipeList : JsonModelObject
	{
        List<Recipe> recipes;

		public List<Recipe> Recipes
		{
            get
            {
               return recipes;
            }

			set
            {
                recipes = value;
            }
		}

		public RecipeList()
		{
		}

		public RecipeList(string json)
		{
			Recipes = (List<Recipe>)parseJSON(json, typeof(List<Recipe>));
		}
	}
}
