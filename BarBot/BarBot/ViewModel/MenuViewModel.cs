using System.Collections.Generic;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using BarBot.Core.Model;
using BarBot.Core.Service.Navigation;
using BarBot.Core.Service.WebSocket;
using BarBot.Core.WebSocket;

namespace BarBot.Core.ViewModel
{
    public class MenuViewModel : ViewModelBase
    {
        private readonly INavigationServiceExtension navigationService;
        private readonly IWebSocketService webSocketService;

        private RelayCommand getRecipesCommand;

        private string _title;
        private List<Recipe> _recipes;
        private bool _shouldDisplaySearch = false;
        private Dictionary<string, byte[]> _imageCache;

        public MenuViewModel(INavigationServiceExtension navigationService,
                             IWebSocketService webSocketService)
        {
            this.navigationService = navigationService;
            this.webSocketService = webSocketService;

            _recipes = new List<Recipe>();
            _imageCache = new Dictionary<string, byte[]>();
            MessengerInstance.Register<bool>(this, shouldDisplaySearch =>
            {
                _shouldDisplaySearch = shouldDisplaySearch;
            });
            Title = "DRINK MENU";
        }

        public string Title
        {
            get { return _title; }
            set { Set(ref _title, value); }
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

        public RelayCommand GetRecipesCommand
        {
            get
            {
                return getRecipesCommand ?? (getRecipesCommand = new RelayCommand(webSocketService.GetRecipes));
            }
        }

        public void ShowDrinkDetailsCommand(string recipeIdentifier, byte[] imageContents)
        {
            // recipeIdentifier = name for Custom, recipeId otherwise
            MessengerInstance.Send(recipeIdentifier);
            if (imageContents != null)
            {
                MessengerInstance.Send(imageContents);
            }
            navigationService.OpenModal(ViewModelLocator.DrinkDetailKey);
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
