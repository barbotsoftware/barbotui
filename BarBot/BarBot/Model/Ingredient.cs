/*
 * Ingredient.cs
 * BarBot
 * 
 * Created by Naveen Yadav on 9/22/16.
 * Copyright © 2016 BarBot. All rights reserved.
 */

using System.ComponentModel;

using Newtonsoft.Json;

namespace BarBot.Core.Model
{
	public class Ingredient : JsonModelObject, INotifyPropertyChanged
	{
        string ingredientId;
        string name;
        double amount;

        [JsonProperty("ingredient_id")]
		public string IngredientId
		{
			get { return ingredientId; }
			set
			{
				ingredientId = value;
				OnPropertyChanged("IngredientId");
			}
		}

        [JsonProperty("name")]
		public string Name
		{
			get { return name; }
			set
			{
				name = value;
				OnPropertyChanged("Name");
			}
		}

        [JsonProperty("amount")]
		public double Amount
		{
			get { return amount; }
			set
			{
				amount = value;
				OnPropertyChanged("Amount");
			}
		}

		public Ingredient()
		{
		}

        public Ingredient(string ingredientId, string name, double amount)
        {
            IngredientId = ingredientId;
			Name = name;
			Amount = amount;
        }

		public Ingredient(string json)
		{
			var i = (Ingredient)parseJSON(json, typeof(Ingredient));
			IngredientId = i.IngredientId;
			Name = i.Name;
			Amount = i.Amount;
		}

		public override string ToString()
		{
			return Name;
		}
    }
}
