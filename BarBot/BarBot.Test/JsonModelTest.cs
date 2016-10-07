using System;
using BarBot.Model;

namespace BarBot.Test
{
	public class JsonModelTest
	{
		public JsonModelTest()
		{
		}

		public void runTest()
		{
			Recipe r = new Recipe("recipe_8a4d7a",
					  "Cuba Libre",
					  "http:\\/\\/192.168.1.41\\/barbot\\/public\\/images\\/term.jpg",
					  new Step[] { new Step(1, 1, "ingredient_1b4549", 1.0, "oz") }
					 );
			Console.WriteLine(r.toJSON());
		}
	}
}
