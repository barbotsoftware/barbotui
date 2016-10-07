using System;
using BarBot.Model;

namespace BarBot.Test
{
	public class JsonModelTest
	{
		public JsonModelTest()
		{
		}

		public void testRecipe()
		{
			Console.WriteLine("testRecipe():"); 
			Console.WriteLine();

			var r = new Recipe("recipe_8a4d7a",
							   "Cuba Libre",
							   "http:\\/\\/192.168.1.41\\/barbot\\/public\\/images\\/term.jpg",
							   new Step[] { new Step(1, 1, "ingredient_1b4549", 1.0, "oz") });

			string json = r.toJSON();

			Console.WriteLine("Serializing... ");
			Console.WriteLine();
			Console.WriteLine(json);
			Console.WriteLine();

			var x = new Recipe(json);
			Console.WriteLine("Deserializing... ");
			Console.WriteLine();
			Console.WriteLine("Id: " + x.Id);
			Console.WriteLine("Name: " + x.Name);
			Console.WriteLine("Img: " + x.Img);
			Console.WriteLine("Steps: " + x.Steps[0]);
			Console.WriteLine();
		}

		public void testIngredient()
		{
			Console.WriteLine("testIngredient():");
			Console.WriteLine();

			var i = new Ingredient("ingredient_772b7",
								   "tequila",
								   "El Jimador",
								   "liquor");

			string json = i.toJSON();

			Console.WriteLine("Serializing... ");
			Console.WriteLine();
			Console.WriteLine(json);
			Console.WriteLine();

			var x = new Ingredient(json);
			Console.WriteLine("Deserializing... ");
			Console.WriteLine();
			Console.WriteLine("Id: " + x.Id);
			Console.WriteLine("Name: " + x.Name);
			Console.WriteLine("Brand: " + x.Brand);
			Console.WriteLine("Type: " + x.Type);
			Console.WriteLine();
		}

		public void testStep()
		{
			Console.WriteLine("testStep():");
			Console.WriteLine();

			var s = new Step(1, 1, "ingredient_1b4549", 1.0, "oz");

			string json = s.toJSON();

			Console.WriteLine("Serializing... ");
			Console.WriteLine();
			Console.WriteLine(json);
			Console.WriteLine();

			var x = new Step(json);
			Console.WriteLine("Deserializing... ");
			Console.WriteLine();
			Console.WriteLine("StepNumber: " + x.StepNumber);
			Console.WriteLine("Type: " + x.Type);
			Console.WriteLine("IngredientId: " + x.IngredientId);
			Console.WriteLine("Quantity: " + x.Quantity);
			Console.WriteLine("Measurement: " + x.Measurement);
			Console.WriteLine();
		}
	}
}
