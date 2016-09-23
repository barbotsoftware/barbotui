/*
 * RecipeBase.cs
 * BarBot
 * 
 * Created by Naveen Yadav on 9/22/16.
 * Copyright © 2016 BarBot. All rights reserved.
 */

using System;
namespace BarBot.Model
{
    public abstract class RecipeBase
    {
        private String recipeId;
        private String name;
        private String imageURL;

        public RecipeBase(String recipeId, String name, String imageURL)
        {
            this.RecipeId = recipeId;
            this.Name = name;
            this.ImageURL = imageURL;
        }

        public string RecipeId
        {
            get
            {
                return recipeId;
            }

            set
            {
                recipeId = value;
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

        public string ImageURL
        {
            get
            {
                return imageURL;
            }

            set
            {
                imageURL = value;
            }
        }
    }
}
