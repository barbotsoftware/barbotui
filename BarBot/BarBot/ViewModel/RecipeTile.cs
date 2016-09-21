/*
 * RecipeTile.cs
 * BarBot
 * 
 * Created by Naveen Yadav on 9/11/16.
 * Copyright © 2016 BarBot. All rights reserved.
 */

using System;
namespace ViewModel
{
	public class RecipeTile
	{
		private String recipeId;
		private String name;
		private String imageURL;

		public RecipeTile() : this("", "", "")
		{
		}

		public RecipeTile(String recipeId, String name, String imageURL)
		{
			setRecipeId(recipeId);
			setName(name);
			setImageURL(imageURL);
		}

		public String getRecipeId()
		{
			return this.recipeId;
		}

		public void setRecipeId(String recipeId)
		{
			this.recipeId = recipeId;
		}

		public String getName()
		{
			return this.name;
		}

		public void setName(String name)
		{
			this.name = name;
		}

		public String getImageURL()
		{
			return this.imageURL;
		}

		public void setImageURL(String imageURL)
		{
			this.imageURL = imageURL;
		}
	}
}

