/*
 * IngredientBase.cs
 * BarBot
 * 
 * Created by Naveen Yadav on 9/22/16.
 * Copyright © 2016 BarBot. All rights reserved.
 */

namespace BarBot.Model
{
    public abstract class IngredientBase
	{
		public string IngredientId { get; set; }
		public string Name { get; set; }

        protected IngredientBase(string ingredientId, string name)
        {
            IngredientId = ingredientId;
            Name = name;
        }
    }
}
