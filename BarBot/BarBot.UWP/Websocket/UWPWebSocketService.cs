using System.Collections.Generic;
using BarBot.Core.Service.WebSocket;
using BarBot.Core.WebSocket;
using BarBot.Core;
using BarBot.Core.Model;

namespace BarBot.UWP.Websocket
{
    public class UWPWebSocketService : WebSocketService
    {
        public UWPWebSocketService(WebSocketHandler webSocket, string barbotId, string endpoint) : base(webSocket, barbotId, endpoint)
        {
           
        }

        public async void OpenWebSocket(string password)
        {
            bool success = await webSocket.OpenConnection(endpoint + "/ws?barbot_id=" + barbotId
                                                          + "&password=" + password);

            if (success)
            {
                //GetRecipes();
                //GetIngredients();
            }
        }

        public void AddMenuEventHandlers(WebSocketEvents.GetRecipesEventHandler recipesHandler,
                                         WebSocketEvents.GetIngredientsEventHandler ingredientsHandler)
        {
            Socket.GetRecipesEvent += recipesHandler;
            Socket.GetIngredientsEvent += ingredientsHandler;
        }

        public void RemoveMenuEventHandlers(WebSocketEvents.GetRecipesEventHandler recipesHandler,
                                            WebSocketEvents.GetIngredientsEventHandler ingredientsHandler)
        {
            Socket.GetRecipesEvent -= recipesHandler;
            Socket.GetIngredientsEvent -= ingredientsHandler;
        }

        public void AddDetailEventHandlers(WebSocketEvents.GetRecipeDetailsEventHandler recipeDetailsHandler,
                                            WebSocketEvents.OrderDrinkEventHandler orderDrinkHandler,
                                           WebSocketEvents.CreateCustomRecipeEventHandler createCustomDrinkHandler)
        {
            Socket.GetRecipeDetailsEvent += recipeDetailsHandler;
            Socket.OrderDrinkEvent += orderDrinkHandler;
            Socket.CreateCustomRecipeEvent += createCustomDrinkHandler;
        }

        public void RemoveDetailEventHandlers(WebSocketEvents.GetRecipeDetailsEventHandler recipeDetailsHandler,
                                              WebSocketEvents.OrderDrinkEventHandler orderDrinkHandler,
                                              WebSocketEvents.CreateCustomRecipeEventHandler createCustomDrinkHandler)
        {
            Socket.GetRecipeDetailsEvent -= recipeDetailsHandler;
            Socket.OrderDrinkEvent -= orderDrinkHandler;
            Socket.CreateCustomRecipeEvent -= createCustomDrinkHandler;
        }

        public void GetRecipes(string categoryId = "")
        {
            if (Socket.IsOpen)
            {
                var data = new Dictionary<string, object>();
                data.Add("barbot_id", barbotId);
                if(!"".Equals(categoryId))
                {
                    data.Add("category_id", categoryId);
                }

                var message = new Message(Constants.Command, Constants.GetRecipesForBarbot, data);

                Socket.sendMessage(message);
            }
        }

        public void GetIngredients()
        {
            if (Socket.IsOpen)
            {
                var data = new Dictionary<string, object>();
                data.Add("barbot_id", barbotId);

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
                data.Add("barbot_id", barbotId);
                data.Add("recipe_id", recipeId);
                data.Add("ice", ice ? 1 : 0);
                data.Add("garnish", garnish ? 1 : 0);

                var message = new Message(Constants.Command, Constants.OrderDrink, data);

                Socket.sendMessage(message);
            }
        }

        public void CreateCustomRecipe(Recipe recipe)
        {
            if (Socket.IsOpen)
            {
                var data = new Dictionary<string, object>();
                data.Add("recipe", recipe);

                var message = new Message(Constants.Command, Constants.CreateCustomRecipe, data);

                Socket.sendMessage(message);
            }
        }

        public void GetCategories()
        {
            if(Socket.IsOpen)
            {
                var message = new Message(Constants.Command, Constants.GetCategories, new Dictionary<string, object>());
                Socket.sendMessage(message);
            }
        }

        public void GetCategory(string categoryId)
        {
            if(Socket.IsOpen)
            {
                var data = new Dictionary<string, object>();
                data.Add("category_id", categoryId);

                var message = new Message(Constants.Command, Constants.GetCategory, data);

                Socket.sendMessage(message);
            }
        }

        public void UpdateContainer(Container container)
        {
            if (Socket.IsOpen)
            {
                var data = new Dictionary<string, object>()
                {
                    { "barbot_id", barbotId },
                    { "container", container }
                };

                var message = new Message(Constants.Command, Constants.UpdateContainer, data);

                Socket.sendMessage(message);
            }
        }
    }
}
