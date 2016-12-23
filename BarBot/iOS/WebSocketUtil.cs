using System.Threading.Tasks;
using System.Collections.Generic;

using UIKit;

using BarBot.Core;
using BarBot.Core.Model;
using BarBot.Core.WebSocket;

namespace BarBot.iOS
{
	public class WebSocketUtil
	{
		public WebSocketHandler Socket { get; set; }

		public WebSocketUtil()
		{
			Socket = new IosWebSocketHandler();
		}

		public async void OpenWebSocket(WebSocketEvents.GetRecipesEventHandler recipesHandler,
		                                WebSocketEvents.GetIngredientsEventHandler ingredientsHandler)
		{
			bool success = await Socket.OpenConnection(Constants.EndpointURL + "?id=" + Constants.BarBotId);

			if (success)
			{
				GetRecipes(recipesHandler);
				GetIngredients(ingredientsHandler);
			}
		}

		public void GetRecipes(WebSocketEvents.GetRecipesEventHandler handler)
		{
			if (Socket.IsOpen)
			{
				var data = new Dictionary<string, object>();
				data.Add("barbot_id", Constants.BarBotId);

				var message = new Message(Constants.Command, Constants.GetRecipesForBarbot, data);

				Socket.GetRecipesEvent += handler;

				Socket.sendMessage(message);
			}
		}

		public void GetRecipeDetails(WebSocketEvents.GetRecipeDetailsEventHandler handler, string recipeId)
		{
			if (Socket.IsOpen)
			{
				var data = new Dictionary<string, object>();
				data.Add("recipe_id", recipeId);

				var message = new Message(Constants.Command, Constants.GetRecipeDetails, data);

				Socket.GetRecipeDetailsEvent += handler;

				Socket.sendMessage(message);
			}
		}

		public void GetIngredients(WebSocketEvents.GetIngredientsEventHandler handler)
		{
			if (Socket.IsOpen)
			{
				var data = new Dictionary<string, object>();
				data.Add("barbot_id", Constants.BarBotId);

				var message = new Message(Constants.Command, Constants.GetIngredientsForBarbot, data);

				Socket.GetIngredientsEvent += handler;

				Socket.sendMessage(message);
			}
		}
	}
}
