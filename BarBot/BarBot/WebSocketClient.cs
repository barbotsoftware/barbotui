/*
 * WebSocketClient.cs
 * BarBot
 * 
 * Created by Naveen Yadav on 10/8/16.
 * Copyright © 2016 BarBot. All rights reserved.
 */

using System.Diagnostics;
using System.Threading.Tasks;

using Websockets;

namespace BarBot
{
	public class WebSocketClient
	{
		private IWebSocketConnection connection;
		private bool Failed;
		private string BarBotId { get; set; }
		private string UserId { get; set; }
		private string Host { get; set; }

		public WebSocketClient(string barBotId, string userId, string host)
		{
			BarBotId = barBotId;
			UserId = userId;
			Host = host;
		}

		public void Setup()
		{
			#if __IOS__
				Websockets.Ios.WebsocketConnection.Link();
			#endif

			#if __ANDROID__
				Websockets.Droid.WebsocketConnection.Link();
			#endif
		}

		public async void Connect()
		{
			connection = WebSocketFactory.Create();
			connection.OnLog += Connection_OnLog;
			connection.OnError += Connection_OnError;
			connection.OnMessage += Connection_OnMessage;
			connection.OnOpened += Connection_OnOpened;

			//Timeout / Setup
			Failed = false;
			Timeout();

			Debug.WriteLine("connecting to server...");
			connection.Open("ws://" + Host + ":8000?id=" + BarBotId);

			while (!connection.IsOpen && !Failed)
			{
				await Task.Delay(10);
			}

			if (!connection.IsOpen)
			{
				return;
			}
		}

		private void Connection_OnOpened()
		{
			Debug.WriteLine("Websocket is connected");
		}

		async void Timeout()
		{
			await Task.Delay(120000);
			Failed = true;
			Debug.WriteLine("Timeout");
		}

		private void Connection_OnMessage(string obj)
		{
			Debug.WriteLine(obj);
		}

		private void Connection_OnError(string obj)
		{
			#if __MOBILE__
				Trace.Write("ERROR " + obj);
			#endif
			Failed = true;
		}

		private void Connection_OnLog(string obj)
		{
			#if __MOBILE__
				Trace.Write(obj);
			#endif
		}
	}
}
