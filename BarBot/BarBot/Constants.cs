/*
 * Constants.cs
 * BarBot
 * 
 * Created by Naveen Yadav on 10/02/16.
 * Copyright © 2016 BarBot Inc. All rights reserved.
 */

namespace BarBot.Core
{
	public class Constants
	{
        #region Websocket

        public const string GetRecipesForBarbot = "get_recipes_for_barbot";
        public const string GetRecipeDetails = "get_recipe_details";
        public const string GetIngredientsForBarbot = "get_ingredients_for_barbot";

		//public const string EndpointURL = "ws://10.0.0.3:8000";

		// 192.168.0.27 = BiloNet
		// 80 = ExtraDirtyTruffleButter
		// 36 = Anchor
		// 10.0.0.3 = 2TurntUp
		public const string EndpointURL = "ws://10.0.0.3:8000";
        
		public const string BarBotId = "barbot_805d2a";

        #endregion

        public const string Command = "command";
        public const string Response = "response";
        public const int Timeout = 10000;
    }
}

