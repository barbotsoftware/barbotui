/*
 * Ingredient.cs
 * BarBot
 * 
 * Created by Naveen Yadav on 9/22/16.
 * Copyright © 2016 BarBot. All rights reserved.
 */

namespace BarBot.Model
{
	public class Ingredient : JsonModelObject
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Brand { get; set; }
		public string Type { get; set; }

		public Ingredient()
		{
		}

        public Ingredient(string id, string name, string brand, string type)
        {
            Id = id;
            Name = name;
			Brand = brand;
			Type = type;
        }

		public Ingredient(string json)
		{
			var i = (Ingredient)parseJSON(json, typeof(Ingredient));
			Id = i.Id;
			Name = i.Name;
			Brand = i.Brand;
			Type = i.Type;
		}
    }
}
