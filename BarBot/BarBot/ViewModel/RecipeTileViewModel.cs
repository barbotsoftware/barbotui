﻿/*
 * RecipeTileViewModel.cs
 * BarBot
 * 
 * Created by Naveen Yadav on 9/22/16.
 * Copyright © 2016 BarBot. All rights reserved.
 */

namespace BarBot.ViewModel
{
	public class RecipeTileViewModel : Model.Recipe
	{
        public RecipeTileViewModel(string recipeId, string name, string imageURL, Model.Ingredient[] ingredients) 
			: base(recipeId, name, imageURL, ingredients)
        {
        }

		public RecipeTileViewModel(string json) : base(json)
		{
		}
	}
}

