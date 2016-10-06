/*
 * Recipe.cs
 * BarBot
 * 
 * Created by Naveen Yadav on 9/22/16.
 * Copyright © 2016 BarBot. All rights reserved.
 */

namespace BarBot.Model
{
	public class Recipe : JsonModelObject
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Img { get; set; }
		public Step[] Steps { get; set; }

		public Recipe(string id, string name, string img, Step[] steps)
		{
			Id = id;
			Name = name;
			Img = img;
			Steps = steps;
		}

		public Recipe(string json)
		{
			var r = (Recipe)parseJSON(json);
			Id = r.Id;
			Name = r.Name;
			Img = r.Img;
			Steps = r.Steps;

			// To-do: query available ingredients to match IngredientId, add to Ingredients array
		}

		// {
		// 		"name": "Cuba Libre",
		//		"id": "recipe_8a4d7a",
		// 		"img": "http:\/\/192.168.1.41\/barbot\/public\/images\/term.jpg",
		//		"steps": [{
		//			"IngredientId": "ingredient_1b4549",
		//			"Quantity": "1.0",
		//			"Measurement": "oz"
		//		}]
		// }
    }
}
