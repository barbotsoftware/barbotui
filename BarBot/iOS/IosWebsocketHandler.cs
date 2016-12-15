using System.Collections.Generic;
using System.Threading.Tasks;
using Websockets.Ios;
using BarBot.Core;
using BarBot.Core.WebSocket;

namespace BarBot.iOS
{
	public class IosWebsocketHandler : WebSocketHandler
	{
		public IosWebsocketHandler()
		{
			WebsocketConnection.Link();
			Init();
		}
	}
}
