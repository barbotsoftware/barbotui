﻿using System;
using BarBot.Model;

namespace BarBot.Test
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			Recipe r = new Recipe();

			var t = new JsonModelTest();
			t.testRecipe(r);
			t.testIngredient();
			t.testStep();
		}
	}
}
