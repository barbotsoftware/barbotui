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

        public Ingredient(string ingredientId, double quantity)
        {
            IngredientId = ingredientId;
			Quantity = quantity;
        }

		public Ingredient(string json)
		{
			var i = (Ingredient)parseJSON(json, typeof(Ingredient));
			IngredientId = i.IngredientId;
			Quantity = i.Quantity;
		}
    }
}
