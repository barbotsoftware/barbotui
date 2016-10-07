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

		public Recipe()
		{
		}

		public Recipe(string id, string name, string img, Step[] steps)
		{
			Id = id;
			Name = name;
			Img = img;
			Steps = steps;
		}

		public Recipe(string json)
		{
			var r = (Recipe)parseJSON(json, typeof(Recipe));
			Id = r.Id;
			Name = r.Name;
			Img = r.Img;
			Steps = r.Steps;

			// To-do: query available ingredients to match IngredientId, add to Ingredients array
		}
    }
}
