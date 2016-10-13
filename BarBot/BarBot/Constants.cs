/*
 * Constants.cs
 * BarBot
 * 
 * Created by Naveen Yadav on 10/02/16.
 * Copyright © 2016 BarBot Inc. All rights reserved.
 */

namespace BarBot
{
	public class Constants
	{
        #region Websocket Commands

        public const string GetRecipesForBarbot = "get_recipes_for_barbot";
        public const string GetRecipeDetails = "get_recipe_details";
        public const string GetIngredientsForBarbot = "get_ingredients_for_barbot";

        #endregion


        public const string Command = "command";
        public const string Response = "response";
        public const int Timeout = 10000;
    }
}

