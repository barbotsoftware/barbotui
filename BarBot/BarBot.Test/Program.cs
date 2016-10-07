using System;

namespace BarBot.Test
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			var t = new JsonModelTest();
			t.testRecipe();
			t.testIngredient();
			t.testStep();
		}
	}
}
