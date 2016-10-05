/*
 * IngredientBase.cs
 * BarBot
 * 
 * Created by Naveen Yadav on 9/22/16.
 * Copyright © 2016 BarBot. All rights reserved.
 */

namespace BarBot.Model
{
	public class Ingredient : JsonModelObject
	{
		public string IngredientId { get; set; }
		public string Name { get; set; }
		public double? Quantity { get; set; }
		public string Measurement { get; set; }

        public Ingredient(string ingredientId, string name, double? quantity, string measurement)
        {
            IngredientId = ingredientId;
            Name = name;
			Quantity = quantity;
			Measurement = measurement;
        }

		public Ingredient(string json)
		{
			var i = (Ingredient)parseJSON(json);
			IngredientId = i.IngredientId;
			Name = i.Name;
			Quantity = i.Quantity;
			Measurement = i.Measurement;
		}
    }
}
