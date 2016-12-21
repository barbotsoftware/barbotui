using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarBot.Core.Model;
using Newtonsoft.Json;

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
        public event WebSocketEvents.GetRecipeDetailsEventHandler GetRecipeDetailsEvent = delegate { };
		public event WebSocketEvents.GetIngredientsEventHandler GetIngredientsEvent = delegate { };

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

            switch(message.Command)
            {
                case Constants.GetRecipesForBarbot:
                    List<Recipe> recipes = JsonConvert.DeserializeObject<List<Recipe>>(message.Data["recipes"].ToString());
                    GetRecipesEvent(this, new WebSocketEvents.GetRecipesEventArgs(recipes));
                    break;
                case Constants.GetRecipeDetails:
                    Recipe recipe = new Recipe(message.Data["recipe"].ToString());
                    GetRecipeDetailsEvent(this, new WebSocketEvents.GetRecipeDetailsEventArgs(recipe));
                    break;
				case Constants.GetIngredientsForBarbot:
					List<Ingredient> ingredients = JsonConvert.DeserializeObject<List<Ingredient>>(message.Data["ingredients"].ToString());
					GetIngredientsEvent(this, new WebSocketEvents.GetIngredientsEventArgs(ingredients));
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
