/*
 * Recipe.cs
 * BarBot
 * 
 * Created by Naveen Yadav on 9/22/16.
 * Copyright © 2016 BarBot. All rights reserved.
 */

using System.Collections.Generic;
using System.ComponentModel;

using Newtonsoft.Json;

namespace BarBot.Core.Model
{
	public class Recipe : JsonModelObject, INotifyPropertyChanged
	{
        string recipeId;
        string name;
        string img;
        List<Ingredient> ingredients;

        [JsonProperty("recipe_id")]
		public string RecipeId
        {
            get { return recipeId; }
            set
            {
                recipeId = value;
                OnPropertyChanged("RecipeId");
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

        [JsonProperty("img")]
		public string Img
        {
            get { return img; }
            set
            {
                img = value;
                OnPropertyChanged("Img");
            }
        }

        [JsonProperty("ingredients")]
		public List<Ingredient> Ingredients
        {
            get { return ingredients; }
            set
            {
                ingredients = value;
                OnPropertyChanged("Ingredients");
            }
        }

		public Recipe()
		{
		}

		public Recipe(string recipeId, string name, string img, List<Ingredient> ingredients)
		{
			RecipeId = recipeId;
			Name = name;
			Img = img;
			Ingredients = ingredients;
		}

		public Recipe(string json)
		{
			var r = (Recipe)parseJSON(json, typeof(Recipe));
			RecipeId = r.RecipeId;
			Name = r.Name;
			Img = r.Img;
			Ingredients = r.Ingredients;
		}

		public double GetVolume()
		{
			var volume = 0.0;
			foreach (var i in Ingredients)
			{
				volume += i.Amount;
			}
			return volume;
		}

        public static Recipe CustomRecipe()
        {
            var customRecipe = new Recipe();
            customRecipe.Name = Constants.CustomRecipeName;
            customRecipe.Ingredients = new List<Ingredient>();
            customRecipe.Img = "custom_recipe.png";
            return customRecipe;
        }
    }
}
