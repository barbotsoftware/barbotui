using GalaSoft.MvvmLight;
using BarBot.Core.Model;

namespace BarBot.Core.ViewModel
{
	public class DetailViewModel : ViewModelBase
	{
		private string _title;

		private Recipe _recipe;
		public readonly string RecipeName;
		public readonly Step[] RecipeSteps;

		public DetailViewModel(Recipe r)
		{
			Recipe = r;
			Title = Recipe.Name.ToUpper();
		}

		public Recipe Recipe
		{
			get { return _recipe; }
			set { _recipe = value; }
		}

		public string Title
		{
			get { return _title; }
			set { Set(ref _title, value); }
		}
	}
}
