using BarBot.Core.Model;
using GalaSoft.MvvmLight;
using BarBot.Core.Service.Navigation;
using System.Collections.Generic;
using System.Linq;

namespace BarBot.Core.ViewModel
{
    public class RecipeDetailViewModel : ViewModelBase
	{
        // Services
		private readonly INavigationService navigationService;

        // Commands
        //private RelayCommand goToMenuCommand;

        private byte[] _imageContents;
        private Recipe _recipe;
		private List<double> _quantities;
		private List<Ingredient> _availableIngredients;
		private List<Ingredient> _ingredientsInBarBot;
		private bool _isCustomRecipe;

		public RecipeDetailViewModel(INavigationService navigationService)
		{
            this.navigationService = navigationService;

            MessengerInstance.Register<string>(this, passedString =>
            {
                if (passedString.Equals(Constants.CustomRecipeId))
                {
                    Recipe = Recipe.CustomRecipe();
				}
                else
                {
                    Recipe = new Recipe
                    {
                        RecipeId = passedString
                    };
                }
            });
			MessengerInstance.Register<byte[]>(this, imageContents =>
			{
				ImageContents = imageContents;
			});

			_availableIngredients = new List<Ingredient>();
			_ingredientsInBarBot = new List<Ingredient>();
			_quantities = new List<double>();
			for (var i = 0.5; i <= Constants.MaxVolume; i += 0.5)
			{
				Quantities.Add(i);
			}
		}

        public Recipe Recipe
        {
            get { return _recipe; }
            set
            {
                Set(ref _recipe, value);
            }
        }

		public byte[] ImageContents
		{
			get { return _imageContents; }
			set { Set(ref _imageContents, value); }
		}

		public List<double> Quantities
		{
			get { return _quantities; }
			set { Set(ref _quantities, value); }
		}

		public List<Ingredient> AvailableIngredients
		{
			get { return _availableIngredients; }
			set { Set(ref _availableIngredients, value); }
		}

		public List<Ingredient> IngredientsInBarBot
		{
			get { return _ingredientsInBarBot; }
			set { Set(ref _ingredientsInBarBot, value); }
		}

		public bool IsCustomRecipe
		{
			get { return _isCustomRecipe; }
			set { Set(ref _isCustomRecipe, value); }
		}

		public void Clear()
		{
			ImageContents = null;
			IsCustomRecipe = false;
			AvailableIngredients.Clear();
		}

		// Sets AvailableIngredients equal to Ingredients in BarBot - Ingredients in Recipe
		public void RefreshAvailableIngredients()
		{
			AvailableIngredients = IngredientsInBarBot.ToList();

			// Remove Ingredients already in Recipe
			foreach (Ingredient recipeIngredient in Recipe.Ingredients)
			{
				AvailableIngredients.RemoveAll(availableIngredient => availableIngredient.Name.Equals(recipeIngredient.Name));
			}
		}

		public void ShowDrinkMenuCommand(bool shouldDisplaySearch)
		{
			MessengerInstance.Send(shouldDisplaySearch);
			this.navigationService.CloseModal();
		}

        #region Command

        //public RelayCommand GoToMenuCommand
        //{
        //    get
        //    {
        //        return goToMenuCommand ?? (goToMenuCommand = new RelayCommand(GoToMenuPage));
        //    }
        //}

        #endregion

        //private void GoToMenuPage()
        //{
        //    navigationService.GoBack();
        //}
    }
}
