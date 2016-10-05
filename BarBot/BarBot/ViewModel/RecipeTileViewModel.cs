/*
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
        public RecipeTileViewModel(string recipeId, string name, string imageURL) : base(recipeId, name, imageURL)
        {
        }

		public RecipeTileViewModel(string json) : base(json)
		{
		}
	}
}

