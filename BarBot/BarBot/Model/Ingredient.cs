/*
 * Ingredient.cs
 * BarBot
 * 
 * Created by Naveen Yadav on 9/22/16.
 * Copyright © 2016 BarBot. All rights reserved.
 */

using System.ComponentModel;

namespace BarBot.Core.Model
{
	public class Ingredient : JsonModelObject, INotifyPropertyChanged
	{
		private string _ingredientId;
		private string _name;
		private double _quantity;

		public string IngredientId
		{
			get { return _ingredientId; }
			set
			{
				_ingredientId = value;
				OnPropertyChanged("IngredientId");
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

		public double Quantity
		{
			get { return _quantity; }
			set
			{
				_quantity = value;
				OnPropertyChanged("Quantity");
			}
		}

		public Ingredient()
		{
		}

        public Ingredient(string ingredientId, string name, double quantity)
        {
            IngredientId = ingredientId;
			Name = name;
			Quantity = quantity;
        }

		public Ingredient(string json)
		{
			var i = (Ingredient)parseJSON(json, typeof(Ingredient));
			IngredientId = i.IngredientId;
			Name = i.Name;
			Quantity = i.Quantity;
		}

		public override string ToString()
		{
			return Name;
		}
    }
}
