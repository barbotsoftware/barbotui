using System.Collections.Generic;

namespace BarBot.Core.WebSocket
{
	public class WebSocketUtil
	{
		public WebSocketHandler Socket { get; set; }

		public WebSocketUtil(WebSocketHandler socket)
		{
			Socket = socket;
		}

		public async void OpenWebSocket(string userId)
		{
			bool success = await Socket.OpenConnection("ws://" + Constants.IPAddress + ":" + Constants.PortNumber + "?id=" + userId);

			if (success)
			{
				GetRecipes();
				GetIngredients();
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
									 WebSocketEvents.OrderDrinkEventHandler orderDrinkHandler)
		{
			Socket.GetRecipeDetailsEvent += recipeDetailsHandler;
			Socket.OrderDrinkEvent += orderDrinkHandler;
		}

		public void GetRecipes()
		{
			if (Socket.IsOpen)
			{
				var data = new Dictionary<string, object>();
				data.Add("barbot_id", Constants.BarBotId);

				var message = new Message(Constants.Command, Constants.GetRecipesForBarbot, data);

				Socket.sendMessage(message);
			}
		}

		public void GetIngredients()
		{
			if (Socket.IsOpen)
			{
				var data = new Dictionary<string, object>();
				data.Add("barbot_id", Constants.BarBotId);

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
	}
}
