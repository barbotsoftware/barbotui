using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Websockets.Universal;
using BarBot.Model;
using Newtonsoft.Json;

namespace BarBot.WebSocket
{
    public class WebSocketHandler
    {
        private Websockets.IWebSocketConnection connection;

        private bool failed = false;

        public event WebSocketEvents.GetRecipesEventHandler GetRecipesEvent = delegate { };

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
            connection.OnLog += Connection_OnLog;
            connection.OnError += Connection_OnError; ;
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
            }
        }

        private void Connection_OnError(string obj)
        {
            
        }

        private void Connection_OnLog(string obj)
        {
            
        }
    }
}
