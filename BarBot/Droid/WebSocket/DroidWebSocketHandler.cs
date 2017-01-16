using Websockets.Droid;
using BarBot.Core.WebSocket;

namespace BarBot.Droid.WebSocket
{
	public class DroidWebSocketHandler : WebSocketHandler
	{
		public DroidWebSocketHandler()
		{
			WebsocketConnection.Link();
			Init();
		}
	}
}
