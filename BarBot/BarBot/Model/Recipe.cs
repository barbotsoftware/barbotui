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
		public string RecipeId { get; set; }
		public string Name { get; set; }
		public string ImageURL { get; set; }
		public Ingredient[] Ingredients { get; set; }

		public Recipe(string recipeId, string name, string imageURL, Ingredient[] ingredients)
		{
			RecipeId = recipeId;
			Name = name;
			ImageURL = imageURL;
			Ingredients = ingredients;
		}

		public Recipe(string json)
		{
			var r = (Recipe)parseJSON(json);
			RecipeId = r.RecipeId;
			Name = r.Name;
			ImageURL = r.ImageURL;
			Ingredients = r.Ingredients;

			// To-do: query available ingredients to match IngredientId, add to Ingredients array
		}

		// {
		// 		"Name": "Cuba Libre",
		//		"RecipeId": "recipe_8a4d7a",
		// 		"ImageURL": "http:\/\/192.168.1.41\/barbot\/public\/images\/term.jpg",
		//		"Ingredients": [{
		//			"IngredientId": "ingredient_1b4549",
		//			"Quantity": "1.0",
		//			"Measurement": "oz"
		//		}]
		// }
    }
}
