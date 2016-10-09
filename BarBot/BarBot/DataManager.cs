/*
 * DataManager.cs
 * BarBot
 * 
 * Created by Naveen Yadav on 10/8/16.
 * Copyright © 2016 BarBot. All rights reserved.
 */

using System.Collections.Generic;
using Newtonsoft.Json;

namespace BarBot
{
	public class DataManager
	{
		private WebSocketClient ws;

		public DataManager()
		{
			ws = new WebSocketClient("barbot_805d2a", "user_348604", "192.168.1.80");
			ws.Setup();
			ws.Connect();
		}

		public void RequestDataFromServer(string command, Dictionary<string, string> args)
		{
			Dictionary<string, object> json = new Dictionary<string, object>()
			{
				{ "type", "command" },
				{ "command", command },
				{ "args", args }
			};

			ws.Request(JsonConvert.SerializeObject(json, Formatting.Indented));
		}
	}
}
