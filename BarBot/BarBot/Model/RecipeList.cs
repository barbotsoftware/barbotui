using System.Collections.Generic;

namespace BarBot.Core.Model
{
	public class RecipeList : JsonModelObject
	{
		private List<Recipe> _recipes;

		public RecipeList()
		{
		}

		public RecipeList(string json)
		{
			Recipes = (List<Recipe>)parseJSON(json, typeof(List<Recipe>));
		}

		public List<Recipe> Recipes
		{
			get { return _recipes; }
			set { _recipes = value; }
		}
	}
}
