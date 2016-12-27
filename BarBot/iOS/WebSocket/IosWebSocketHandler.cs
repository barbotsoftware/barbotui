using Websockets.Ios;
using BarBot.Core.WebSocket;

namespace BarBot.iOS.WebSocket
{
	public class IosWebSocketHandler : WebSocketHandler
	{
		public IosWebSocketHandler()
		{
			WebsocketConnection.Link();
			Init();
		}
	}
}
