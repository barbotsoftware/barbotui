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
		public const string Command = "command";
		public const string Event = "event";
		public const string Response = "response";
		public const int Timeout = 10000;

		public const string GetRecipesForBarbot = "get_recipes_for_barbot";
		public const string GetRecipeDetails = "get_recipe_details";
		public const string GetIngredientsForBarbot = "get_ingredients_for_barbot";
		public const string OrderDrink = "order_drink";
		public const string CreateCustomDrink = "create_custom_drink";
		public const string DrinkOrderedEvent = "barbot.drink_ordered";

		public const string IPAddress = "192.168.1.196";

		public const string HostName = "barbotweb";

		public const string PortNumber = "8000";

		public const string BarBotId = "barbot_a0a627";

		public const string CustomRecipeId = "recipe_custom";

		public const double MaxVolume = 12.0;

		#endregion

		// UI - Hexagon
		public const int HexagonWidth = 390;

		public enum BarbotStatus
		{
			STARTING,
			READY,
			BUSY
		}
	}
}
