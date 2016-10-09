using System;
using System.Collections.Generic;

namespace BarBot.Test
{
	public class WebSocketTest
	{
		public WebSocketTest()
		{
		}

		public void testGetRecipesForBarBot()
		{
			Dictionary<string, string> a = new Dictionary<string, string>()
			{
				{ "barbot_id", "barbot_805d2a" }
			};

			var d = new DataManager();
			Console.WriteLine(d.RequestDataFromServer("get_recipes_for_barbot", a));
		}
	}
}
