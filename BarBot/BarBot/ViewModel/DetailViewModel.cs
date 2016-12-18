using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using BarBot.Core.Model;

namespace BarBot.Core.ViewModel
{
	public class DetailViewModel : ViewModelBase
	{
		private string _title;
		private string _recipeId;

		public DetailViewModel()
		{
			MessengerInstance.Register<string>(this, id => 
			{
				Title = id;
				_recipeId = id;
			});
		}

		//public Recipe Recipe
		//{
		//	get { return _recipe; }
		//	set { Set(ref _recipe, value); }
		//}

		public string Title
		{
			get { return _title; }
			set { Set(ref _title, value); }
		}
	}
}
