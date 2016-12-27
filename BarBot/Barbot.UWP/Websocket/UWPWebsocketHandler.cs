using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarBot.Core.WebSocket;
using Websockets.Universal;

namespace BarBot.UWP.Websocket
{
    public class UWPWebsocketHandler : WebSocketHandler
    {
        public UWPWebsocketHandler()
        {
            WebsocketConnection.Link();
            Init();
        }
    }
}
