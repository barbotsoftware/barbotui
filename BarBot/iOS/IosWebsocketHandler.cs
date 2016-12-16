using Websockets.Ios;
using BarBot.Core.WebSocket;

namespace BarBot.iOS
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
