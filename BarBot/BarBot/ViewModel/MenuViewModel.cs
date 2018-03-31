using BarBot.Core.Model;
using BarBot.Core.Service.Navigation;
using BarBot.Core.Service.WebSocket;
using BarBot.Core.WebSocket;
using GalaSoft.MvvmLight;
using System.Collections.Generic;

namespace BarBot.Core.ViewModel
{
    public class MenuViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly IWebSocketService webSocketService;

        //private RelayCommand getRecipesCommand;
        //private RelayCommand<RecipeDetailViewModel> showRecipeDetailCommand;

        //public ObservableCollection<RecipeDetailViewModel> Recipes { get; }

        private List<Recipe> _recipes;
        private bool _shouldDisplaySearch = false;
        private Dictionary<string, byte[]> _imageCache;

        public MenuViewModel(INavigationService navigationService,
                             IWebSocketService webSocketService)
        {
            this.navigationService = navigationService;
            this.webSocketService = webSocketService;

            //Recipes = new ObservableCollection<RecipeDetailViewModel>();

            _recipes = new List<Recipe>();
            _imageCache = new Dictionary<string, byte[]>();
            MessengerInstance.Register<bool>(this, shouldDisplaySearch =>
            {
                _shouldDisplaySearch = shouldDisplaySearch;
            });
            Title = "Menu";
        }

        public string Title
        {
            get;
            set;
        }

        public List<Recipe> Recipes
        {
            get { return _recipes; }
            set { Set(ref _recipes, value); }
        }

        public bool ShouldDisplaySearch
        {
            get { return _shouldDisplaySearch; }
            set { Set(ref _shouldDisplaySearch, value); }
        }

        public Dictionary<string, byte[]> ImageCache
        {
            get { return _imageCache; }
            set { Set(ref _imageCache, value); }
        }

        #region Command

        //public RelayCommand GetRecipesCommand
        //{
        //    get
        //    {
        //        return getRecipesCommand ?? (getRecipesCommand = new RelayCommand(webSocketService.GetRecipes));
        //    }
        //}

        //public RelayCommand<RecipeDetailViewModel> ShowRecipeDetailCommand
        //{
        //    get
        //    {
        //        return showRecipeDetailCommand
        //               ?? (showRecipeDetailCommand = new RelayCommand<RecipeDetailViewModel>(
        //                   recipe =>
        //                   {
        //                       if (!ShowRecipeDetailCommand.CanExecute(recipe))
        //                       {
        //                           return;
        //                       }

        //                       navigationService.NavigateTo(ViewModelLocator.RecipeDetailPageKey, recipe);
        //                   },
        //                   recipe => recipe != null));
        //    }
        //}

        public void ShowDrinkDetailsCommand(string recipeIdentifier, byte[] imageContents)
        {
            MessengerInstance.Send(recipeIdentifier);
            if (imageContents != null)
            {
                MessengerInstance.Send(imageContents);
            }
            navigationService.OpenModal(ViewModelLocator.RecipeDetailPageKey);
        }

        public void ShowContainersCommand() {
            navigationService.OpenModal(ViewModelLocator.ContainersPageKey);
        }

		#endregion

		#region Event Handlers

		public void AddEventHandlers(WebSocketEvents.GetRecipesEventHandler recipesHandler,
									 WebSocketEvents.GetIngredientsEventHandler ingredientsHandler)
		{
            webSocketService.AddEventHandler("GetRecipesEvent");
            webSocketService.AddEventHandler("GetIngredientsEvent");
		}

		public void RemoveEventHandlers(WebSocketEvents.GetRecipesEventHandler recipesHandler,
										WebSocketEvents.GetIngredientsEventHandler ingredientsHandler)
		{
			webSocketService.RemoveEventHandler("GetRecipesEvent");
			webSocketService.RemoveEventHandler("GetIngredientsEvent");
		}

        #endregion
    }
}
