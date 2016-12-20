/*
 * Recipe.cs
 * BarBot
 * 
 * Created by Naveen Yadav on 9/22/16.
 * Copyright © 2016 BarBot. All rights reserved.
 */

using System.ComponentModel;
using System.Collections.Generic;

namespace BarBot.Core.Model
{
	public class Recipe : JsonModelObject, INotifyPropertyChanged
	{
        private string _id;
        private string _name;
        private string _img;
        private List<Ingredient> _ingredients;

		public string Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

		public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

		public string Img
        {
            get { return _img; }
            set
            {
                _img = value;
                OnPropertyChanged("Img");
            }
        }
		public List<Ingredient> Ingredients
        {
            get { return _ingredients; }
            set
            {
                _ingredients = value;
                OnPropertyChanged("Steps");
            }
        }

		public Recipe()
		{
		}

		public Recipe(string id, string name, string img, Ingredient[] ingredients)
		{
			Id = id;
			Name = name;
			Img = img;
			foreach (Ingredient i in ingredients)
			{
				_ingredients.Add(i);
			}
		}

		public Recipe(string json)
		{
			var r = (Recipe)parseJSON(json, typeof(Recipe));
			Id = r.Id;
			Name = r.Name;
			Img = r.Img;
			Ingredients = r.Ingredients;

			// To-do: query available ingredients to match IngredientId, add to Ingredients array
		}
    }
}
