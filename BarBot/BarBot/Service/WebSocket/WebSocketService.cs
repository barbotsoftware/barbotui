﻿using System.Collections.Generic;

using BarBot.Core.Model;
using BarBot.Core.WebSocket;

namespace BarBot.Core.Service.WebSocket
{
    public class WebSocketService : IWebSocketService
    {
        protected WebSocketHandler webSocket;
        protected string barbotId;
        protected string endpoint;

        public WebSocketHandler Socket
        {
            get
            {
                return webSocket;
            }

            set
            {
                webSocket = value;
            }
        }

        public WebSocketService(WebSocketHandler webSocket, string barbotId, string endpoint)
        {
            this.webSocket = webSocket;
            this.barbotId = barbotId;
            this.endpoint = endpoint;
        }

		public async void OpenWebSocket(string username, string password)
		{
			bool success = await webSocket.OpenConnection(endpoint + "/ws?username=" + username 
                                                          + "&password=" + password);

			if (success)
			{
				GetRecipes();
				GetIngredients();
			}
		}

        public async void CloseWebSocket()
        {
			bool success = await webSocket.CloseConnection();

			if (success)
			{
			}
        }

        public void AddEventHandler(string eventName)
        {
            
        }

        public void RemoveEventHandler(string eventName)
        {
            
        }

        public void CreateCustomRecipe(Recipe recipe)
        {
            var data = new Dictionary<string, object>
            {
                { "recipe", recipe }
            };

            SendMessage(data, Constants.CreateCustomRecipe);
        }

        public void GetContainers()
        {
			var data = new Dictionary<string, object>
			{
				{ "barbot_id", barbotId }
			};

            SendMessage(data, Constants.GetContainersForBarbot);
        }

        public void GetIngredients()
        {
			var data = new Dictionary<string, object>
			{
				{ "barbot_id", barbotId }
			};

            SendMessage(data, Constants.GetIngredientsForBarbot);
        }

        public void GetRecipeDetails(string recipeId) 
        {
            var data = new Dictionary<string, object>
            {
                { "recipe_id", recipeId }
            };

            SendMessage(data, Constants.GetRecipeDetails);
        }

        public void GetRecipes()
        {
            var data = new Dictionary<string, object>
            {
                { "barbot_id", barbotId }
            };

            SendMessage(data, Constants.GetRecipesForBarbot);
        }

        public void OrderDrink(string recipeId, bool ice, bool garnish)
        {
            var data = new Dictionary<string, object>
            {
                { "barbot_id", barbotId },
                { "recipe_id", recipeId },
                { "ice", ice ? 1 : 0 },
                { "garnish", garnish ? 1 : 0 }
            };

            SendMessage(data, Constants.OrderDrink);
        }

        public void SetContainers<Container>(List<Container> containers)
        {
            var data = new Dictionary<string, object>
            {
                { "barbot_id", barbotId },
                { "containers", containers }
            };

            SendMessage(data, Constants.SetContainersForBarbot);
        }

        void SendMessage(Dictionary<string, object> data, string command)
        {
            if (webSocket.IsOpen)
            {
                var message = new Message(Constants.Command, command, data);

                webSocket.sendMessage(message);
            }
        }
    }
}
