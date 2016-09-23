/*
 * RecipeTileViewModel.cs
 * BarBot
 * 
 * Created by Naveen Yadav on 9/22/16.
 * Copyright © 2016 BarBot. All rights reserved.
 */

using System;
namespace BarBot.ViewModel
{
	public class RecipeTileViewModel : Model.RecipeBase
	{
        public RecipeTileViewModel(String recipeId, String name, String imageURL) : base(recipeId, name, imageURL)
        {
        }
	}
}

