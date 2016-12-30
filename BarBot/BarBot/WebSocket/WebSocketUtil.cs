using System.Collections.Generic;
using BarBot.Core.Model;

namespace BarBot.Core.WebSocket
{
	public class WebSocketUtil
	{
		public WebSocketHandler Socket { get; set; }
        public string EndPoint { get; set; }
        public string BarBotId { get; set; }

		public WebSocketUtil(WebSocketHandler socket)
		{
			Socket = socket;
		}

		public async void OpenWebSocket(string userId, bool isMobile)
		{
			bool success = await Socket.OpenConnection(EndPoint + "?id=" + userId);

            // Make calls in OpenWebSocket if mobile
			if (success)
			{
                if (isMobile)
                {
                    BarBotId = Constants.BarBotId;
                    GetRecipes();
                    GetIngredients();
                }
				else
                {
                    BarBotId = userId;
                }
			}
		}

		public async void CloseWebSocket()
		{
			bool success = await Socket.CloseConnection();

			if (success)
			{
			}
		}

		public void AddMenuEventHandlers(WebSocketEvents.GetRecipesEventHandler recipesHandler,
										 WebSocketEvents.GetIngredientsEventHandler ingredientsHandler)
		{
			Socket.GetRecipesEvent += recipesHandler;
			Socket.GetIngredientsEvent += ingredientsHandler;
		}

		public void AddDetailEventHandlers(WebSocketEvents.GetRecipeDetailsEventHandler recipeDetailsHandler,
									 	   WebSocketEvents.OrderDrinkEventHandler orderDrinkHandler,
		                                   WebSocketEvents.CreateCustomDrinkEventHandler createCustomDrinkHandler)
		{
			Socket.GetRecipeDetailsEvent += recipeDetailsHandler;
			Socket.OrderDrinkEvent += orderDrinkHandler;
			Socket.CreateCustomDrinkEvent += createCustomDrinkHandler;
		}

		public void GetRecipes()
		{
			if (Socket.IsOpen)
			{
				var data = new Dictionary<string, object>();
				data.Add("barbot_id", BarBotId);

				var message = new Message(Constants.Command, Constants.GetRecipesForBarbot, data);

				Socket.sendMessage(message);
			}
		}

		public void GetIngredients()
		{
			if (Socket.IsOpen)
			{
				var data = new Dictionary<string, object>();
				data.Add("barbot_id", BarBotId);

				var message = new Message(Constants.Command, Constants.GetIngredientsForBarbot, data);

				Socket.sendMessage(message);
			}
		}

		public void GetRecipeDetails(string recipeId)
		{
			if (Socket.IsOpen)
			{
				var data = new Dictionary<string, object>();
				data.Add("recipe_id", recipeId);

				var message = new Message(Constants.Command, Constants.GetRecipeDetails, data);

				Socket.sendMessage(message);
			}
		}

		public void OrderDrink(string recipeId, bool ice, bool garnish)
		{
			if (Socket.IsOpen)
			{
				var data = new Dictionary<string, object>();
				data.Add("barbot_id", Constants.BarBotId);
				data.Add("recipe_id", recipeId);
				data.Add("ice", ice ? 1 : 0);
				data.Add("garnish", garnish ? 1 : 0);

				var message = new Message(Constants.Command, Constants.OrderDrink, data);

				Socket.sendMessage(message);
			}
		}

		public void CreateCustomDrink(Recipe recipe)
		{
			if (Socket.IsOpen)
			{
				var data = new Dictionary<string, object>();
				data.Add("recipe", recipe);

				var message = new Message(Constants.Command, Constants.CreateCustomDrink, data);

				Socket.sendMessage(message);
			}
		}
	}
}
