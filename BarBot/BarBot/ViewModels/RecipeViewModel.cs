using BarBot.Core.Model;
using MvvmCross.Core.ViewModels;

namespace BarBot.Core.ViewModels
{
	public class RecipeViewModel : MvxViewModel
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
			set { _recipe = value; RaisePropertyChanged(() => Recipe); }
		}
	}
}
