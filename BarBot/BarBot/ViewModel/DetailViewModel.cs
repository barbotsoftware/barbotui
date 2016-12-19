using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using BarBot.Core.Model;

namespace BarBot.Core.ViewModel
{
	public class DetailViewModel : ViewModelBase
	{
		private string _title;
		private string _recipeId;
		private Recipe _recipe;

		public DetailViewModel()
		{
			MessengerInstance.Register<string>(this, id => 
			{
				RecipeId = id;
			});
		}

		public string RecipeId
		{
			get { return _recipeId; }
			set { Set(ref _recipeId, value); }
		}

		public Recipe Recipe
		{
			get { return _recipe; }
			set { Set(ref _recipe, value); }
		}

		public string Title
		{
			get { return _title; }
			set { Set(ref _title, value); }
		}
	}
}
