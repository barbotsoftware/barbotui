using System;
using System.Threading.Tasks;
using BarBot.Core.Model;

namespace BarBot.Core.WebSocket
{
    public class WebSocketHandler
    {
        private Websockets.IWebSocketConnection connection;

        private bool failed = false;

        private bool isOpen = false;

        public bool IsOpen
        {
            get { return isOpen; }
            set { isOpen = value; }
        }

        #region Events

        public event WebSocketEvents.GetRecipesEventHandler GetRecipesEvent = delegate { };
        public event WebSocketEvents.DrinkOrderedEventHandler DrinkOrderedEvent = delegate { };
        public event WebSocketEvents.GetGarnishesEventHandler GetGarnishesEvent = delegate { };
        public event WebSocketEvents.GetIngredientsEventHandler GetIngredientsEvent = delegate { };
        public event WebSocketEvents.GetRecipeDetailsEventHandler GetRecipeDetailsEvent = delegate { };
        public event WebSocketEvents.OrderDrinkEventHandler OrderDrinkEvent = delegate { };
		public event WebSocketEvents.CreateCustomRecipeEventHandler CreateCustomRecipeEvent = delegate { };
        public event WebSocketEvents.GetCategoriesEventHandler GetCategoriesEvent = delegate { };
        public event WebSocketEvents.GetCategoryEventHandler GetCategoryEvent = delegate { };
        public event WebSocketEvents.GetContainersEventHandler GetContainersEvent = delegate { };
        public event WebSocketEvents.UpdateContainerEventHandler UpdateContainerEvent = delegate { };
        public event WebSocketEvents.UpdateGarnishEventHandler UpdateGarnishEvent = delegate { };

        #endregion

        public void Init()
        {
            connection = Websockets.WebSocketFactory.Create();
            connection.OnMessage += Connection_OnMessage;
            connection.OnOpened += Connection_OnOpened;
			connection.OnClosed += Connection_OnClosed;
        }

        public async Task<bool> OpenConnection(String url)
        {
            connection.Open(url);

            while (!isOpen && !failed)
            {
                await Task.Delay(10);
            }

            return isOpen;
        }

        public async Task<bool> CloseConnection()
        {
            connection.Close();

            while (isOpen)
            {
                await Task.Delay(10);
            }

            return true;
        }

        public void sendMessage(Message message)
        {
            if (isOpen)
            {
                connection.Send(message.toJSON());
            }
        }

        private void Connection_OnOpened()
        {
			isOpen = true;
        }

		private void Connection_OnClosed()
		{
			isOpen = false;
		}

        private void Connection_OnMessage(string obj)
        {
            Message message = new Message(obj);

            switch (message.Type)
            {
                case Constants.Event:
                    handleEvent(message);
                    break;
                case Constants.Response:
                    handleResponse(message);
                    break;
            }
        }

        /// <summary>
        /// Handles WebSocket Server responses
        /// </summary>
        /// <param name="message"></param>
        public void handleResponse (Message message)
        {
            switch (message.Command)
            {
                case Constants.GetRecipesForBarbot:
                    var RecipeList = new RecipeList(message.Data["recipes"].ToString());
                    GetRecipesEvent(this, new WebSocketEvents.GetRecipesEventArgs(RecipeList.Recipes));
                    break;
                case Constants.GetRecipeDetails:
                    Recipe recipe = new Recipe(message.Data["recipe"].ToString());
                    GetRecipeDetailsEvent(this, new WebSocketEvents.GetRecipeDetailsEventArgs(recipe));
                    break;
                case Constants.GetGarnishesForBarbot:
                    var GarnishList = new GarnishList(message.Data["garnishes"].ToString());
                    GetGarnishesEvent(this, new WebSocketEvents.GetGarnishesEventArgs(GarnishList.Garnishes));
                    break;
                case Constants.GetIngredientsForBarbot:
                    var IngredientList = new IngredientList(message.Data["ingredients"].ToString());
                    GetIngredientsEvent(this, new WebSocketEvents.GetIngredientsEventArgs(IngredientList.Ingredients));
                    break;
                case Constants.GetContainersForBarbot:
                    var ContainerList = new ContainerList(message.Data["containers"].ToString());
                    GetContainersEvent(this, new WebSocketEvents.GetContainersEventArgs(ContainerList.Containers));
                    break;
                case Constants.OrderDrink:
                    string DrinkOrderId = message.Data["drink_order_id"].ToString();
                    OrderDrinkEvent(this, new WebSocketEvents.OrderDrinkEventArgs(DrinkOrderId));
                    break;
				case Constants.CreateCustomRecipe:
					string RecipeId = message.Data["recipe_id"].ToString();
					CreateCustomRecipeEvent(this, new WebSocketEvents.CreateCustomRecipeEventArgs(RecipeId));
					break;
                case Constants.GetCategories:
                    var CategoryList = new CategoryList(message.Data["categories"].ToString());
                    GetCategoriesEvent(this, new WebSocketEvents.GetCategoriesEventArgs(CategoryList.Categories));
                    break;
                case Constants.GetCategory:
                    Category category = new Category(message.Data["category"].ToString());
                    GetCategoryEvent(this, new WebSocketEvents.GetCategoryEventArgs(category));
                    break;
            }
        }

        public void handleEvent(Message message)
        {
            switch (message.Command)
            {
                case Constants.DrinkOrderedEvent:
                    DrinkOrder drinkOrder = new DrinkOrder(message.Data["drink_order"].ToString());
                    DrinkOrderedEvent(this, new WebSocketEvents.DrinkOrderedEventArgs(drinkOrder));
                    break;
            }
        }

        private async void Timeout()
        {
            await Task.Delay(Constants.Timeout);

            failed = true;
        }
    }
}
