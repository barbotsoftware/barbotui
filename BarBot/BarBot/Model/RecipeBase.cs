/*
 * RecipeBase.cs
 * BarBot
 * 
 * Created by Naveen Yadav on 9/22/16.
 * Copyright © 2016 BarBot. All rights reserved.
 */

namespace BarBot.Model
{
    public abstract class RecipeBase
    {
		public string RecipeId { get; set; }
		public string Name { get; set; }
		public string ImageURL { get; set; }

        protected RecipeBase(string recipeId, string name, string imageURL)
        {
			RecipeId = recipeId;
            Name = name;
            ImageURL = imageURL;
        }
    }
}
