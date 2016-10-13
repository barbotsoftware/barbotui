using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Websockets.Universal;
using Websockets.Ios;
using BarBot.Model;
using Newtonsoft.Json;

namespace BarBot.WebSocket
{
    public class WebSocketHandler
    {
        private Websockets.IWebSocketConnection connection;

        private bool failed = false;

        #region Events

        public event WebSocketEvents.GetRecipesEventHandler GetRecipesEvent = delegate { };
        public event WebSocketEvents.GetRecipeDetailsEventHandler GetRecipeDetailsEvent = delegate { };

        #endregion

        public WebSocketHandler()
        {
#if __IOS__
			Websockets.Ios.WebsocketConnection.Link();
#else
#if __ANDROID__
			Websockets.Droid.WebsocketConnection.Link();
#else
			Websockets.Universal.WebsocketConnection.Link();
#endif
#endif

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

            return connection.IsOpen;
        }

        public async Task<bool> CloseConnection()
        {
            connection.Close();

            while (connection.IsOpen)
            {
                await Task.Delay(10);
            }

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
            }
        }

        private async void Timeout()
        {
            await Task.Delay(Constants.Timeout);

            failed = true;
        }
    }
}
