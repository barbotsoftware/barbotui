using System;

namespace BarBot.Test
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			var w = new WebSocketTest();
			w.testGetRecipesForBarBot();

			//var t = new JsonModelTest();
			//t.testRecipe();
			//t.testIngredient();
			//t.testStep();
		}
	}
}
