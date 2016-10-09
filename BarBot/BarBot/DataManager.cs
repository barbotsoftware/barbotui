using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using Newtonsoft.Json;

namespace BarBot
{
	public class DataManager
	{
		public DataManager()
		{
		}

		public string RequestDataFromServer(string command, Dictionary<string, string> args)
		{
			Dictionary<string, object> json = new Dictionary<string, object>()
			{
				{ "type", "command" },
				{ "command", command },
				{ "args", args }
			};

			return JsonConvert.SerializeObject(json, Formatting.Indented);
		}
	}
}
