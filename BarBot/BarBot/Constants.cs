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
		public const string Response = "command_response";
		public const int Timeout = 10000;

        public const string CreateCustomRecipe = "create_custom_recipe";
        public const string GetContainersForBarbot = "get_containers_for_barbot";
		public const string GetIngredientsForBarbot = "get_ingredients_for_barbot";
        public const string GetRecipeDetails = "get_recipe_details";
        public const string GetRecipesForBarbot = "get_recipes_for_barbot";
		public const string OrderDrink = "order_drink";
        public const string SetContainersForBarbot = "set_containers_for_barbot";
        public const string DrinkOrderedEvent = "barbot.drink_ordered";
        public const string GetCategories = "get_categories";
        public const string GetCategory = "get_category";
        public const string UpdateContainer = "update_container";

		public const string IPAddress = "192.168.1.229";

		public const string HostName = "Exploding";

		public const string PortNumber = "8080";

		public const string BarBotId = "d9f064";

		public const string CustomRecipeId = "custom_recipe";

		public const string AddIngredientId = "add_ingredient";

		public const double MaxVolume = 12.0;

        public const string CustomRecipeName = "Custom Recipe";

        public const string AllRecipesCategoryName = "All Cocktails";

		#endregion

		// UI - Hexagon
		public const int HexagonWidth = 260;

		public enum BarbotStatus
		{
			STARTING,
			READY,
			BUSY
		}
	}
}
