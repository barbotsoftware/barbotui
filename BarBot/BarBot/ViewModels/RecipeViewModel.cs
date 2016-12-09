using BarBot.Core.Model;

namespace BarBot.Core.ViewModels
{
	public class RecipeViewModel
	{
		private Recipe _recipe;
		public readonly string RecipeName;
		public readonly Step[] RecipeSteps;

		public RecipeViewModel(Recipe recipe)
		{
			Recipe = recipe;
			RecipeName = recipe.Name.ToUpper();
		}

		public Recipe Recipe
		{
			get { return _recipe; }
			set { _recipe = value; }
		}
	}
}
