/*
 * Constants.cs
 * BarBot
 * 
 * Created by Naveen Yadav on 10/02/16.
 * Copyright © 2016 BarBot Inc. All rights reserved.
 */

namespace BarBot.Core
{
	public static class Constants
	{
		#region Websocket

		public const string GetRecipesForBarbot = "get_recipes_for_barbot";
		public const string GetRecipeDetails = "get_recipe_details";
		public const string GetIngredientsForBarbot = "get_ingredients_for_barbot";
		public const string OrderDrink = "order_drink";

		// BiloNet
		//public const string EndpointURL = "ws://192.168.0.19:8000";

		// Anchor
		//public const string EndpointURL = "ws://192.168.1.36:8000";

		// 2TurntUp
		//public const string EndpointURL = "ws://10.0.0.3:8000";

		// Horstmann
		public const string EndpointURL = "ws://192.168.1.234:8000";

		public const string BarBotId = "barbot_805d2a";

		public const string UserId = "user_348604";

		#endregion

		public const string Command = "command";
		public const string Response = "response";
		public const int Timeout = 10000;
	}
}

