using BarBot.Model;
using ReactiveUI;

namespace BarBot.ViewModel
{
	public class RecipeViewModel : ReactiveObject
	{
		Recipe _Recipe;
		public readonly string RecipeName;
		public readonly Step[] RecipeSteps;

		public RecipeViewModel(Recipe recipe)
		{
			Recipe = recipe;
			RecipeName = recipe.Name.ToUpper();
			//DrinkImage = await LoadImage(recipe.Img);
		}

		public Recipe Recipe
		{
			get { return _Recipe; }
			set { this.RaiseAndSetIfChanged(ref _Recipe, value); }
		}
	}
}
