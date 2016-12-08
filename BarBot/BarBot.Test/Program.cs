using System;
using BarBot.Core.Model;

namespace BarBot.Test
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			Step[] s = { new Step(1, 1, "1", 1.0, "oz") };
			Recipe r = new Recipe("1",
								 "blah",
								 "blah",
			                      s
			);

			var t = new JsonModelTest();
			t.testRecipe(r);
			t.testIngredient();
			t.testStep();
		}
	}
}
