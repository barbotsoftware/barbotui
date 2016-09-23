/*
 * IngredientBase.cs
 * BarBot
 * 
 * Created by Naveen Yadav on 9/22/16.
 * Copyright © 2016 BarBot. All rights reserved.
 */

using System;
namespace BarBot.Model
{
    public abstract class IngredientBase
    {
        private String ingredientId;
        private String name;

        public IngredientBase(String ingredientId, String name)
        {
            this.IngredientId = ingredientId;
            this.Name = name;
        }

        public string IngredientId
        {
            get
            {
                return ingredientId;
            }

            set
            {
                ingredientId = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }
    }
}
