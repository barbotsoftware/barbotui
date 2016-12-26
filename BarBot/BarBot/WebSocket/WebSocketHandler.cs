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
    		public event WebSocketEvents.GetIngredientsEventHandler GetIngredientsEvent = delegate { };
    		public event WebSocketEvents.GetRecipeDetailsEventHandler GetRecipeDetailsEvent = delegate { };
    		public event WebSocketEvents.OrderDrinkEventHandler OrderDrinkEvent = delegate { };

		    #endregion

        public void Init()
        {
            connection = Websockets.WebSocketFactory.Create();
            connection.OnMessage += Connection_OnMessage;
            connection.OnOpened += Connection_OnOpened;
        }

        public async Task<bool> OpenConnection(String url)
        {
            connection.Open(url);

            while(!connection.IsOpen && !failed)
            {
                await Task.Delay(10);
            }

            isOpen = true;

            return connection.IsOpen;
        }

        public async Task<bool> CloseConnection()
        {
            connection.Close();

            while (connection.IsOpen)
            {
                await Task.Delay(10);
            }

            isOpen = false;

            return true;
        }

        public void sendMessage(Message message)
        {
            if(connection.IsOpen)
            {
                connection.Send(message.toJSON());
            }
        }

        private void Connection_OnOpened()
        {

        }

        private void Connection_OnMessage(string obj)
        {
            Message message = new Message(obj);

            switch(message.Type)
            {
                case Constants.Command:
                    handleCommand(message);
                    break;
                case Constants.Event:
                    handleEvent(message);
                    break;
            }
        }

        public void handleCommand(Message message)
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
        				case Constants.GetIngredientsForBarbot:
        					  var IngredientList = new IngredientList(message.Data["ingredients"].ToString());
        					  GetIngredientsEvent(this, new WebSocketEvents.GetIngredientsEventArgs(IngredientList.Ingredients));
        					  break;
        				case Constants.OrderDrink:
        					string DrinkOrderId = message.Data["drink_order_id"].ToString();
        					OrderDrinkEvent(this, new WebSocketEvents.OrderDrinkEventArgs(DrinkOrderId));
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
