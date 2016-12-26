using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarBot.Core.Model;

namespace BarBot.Core.WebSocket
{
    public class WebSocketEvents
    {
        public delegate void GetRecipesEventHandler(object sender, GetRecipesEventArgs args);

        public class GetRecipesEventArgs : EventArgs
        {
            private List<Recipe> recipes;

            public GetRecipesEventArgs(List<Recipe> recipes)
            {
                this.recipes = recipes;
            }

            public List<Recipe> Recipes
            {
                get { return recipes; }
            }
        }

        public delegate void GetRecipeDetailsEventHandler(object sender, GetRecipeDetailsEventArgs args);

        public class GetRecipeDetailsEventArgs : EventArgs
        {
            private Recipe recipe;

            public GetRecipeDetailsEventArgs(Recipe recipe)
            {
                this.recipe = recipe;
            }

            public Recipe Recipe
            {
                get { return recipe; }
            }
        }

        // Incoming Drink Orders
        public delegate void DrinkOrderedEventHandler(object sender, DrinkOrderedEventArgs args);

        public class DrinkOrderedEventArgs : EventArgs
        {
            private DrinkOrder drinkOrder;

            public DrinkOrderedEventArgs (DrinkOrder drinkOrder)
            {
                this.drinkOrder = drinkOrder;
            }

            public DrinkOrder DrinkOrder
            {
                get { return drinkOrder; }
            }
        }

    		public delegate void GetIngredientsEventHandler(object sender, GetIngredientsEventArgs args);

    		public class GetIngredientsEventArgs : EventArgs
    		{
    			private List<Ingredient> ingredients;

    			public GetIngredientsEventArgs(List<Ingredient> ingredients)
    			{
    				this.ingredients = ingredients;
    			}

    			public List<Ingredient> Ingredients
    			{
    				get { return ingredients; }
    			}
    		}

        // Send Order Drink Command
    		public delegate void OrderDrinkEventHandler(object sender, OrderDrinkEventArgs args);

    		public class OrderDrinkEventArgs : EventArgs
    		{
    			private string drinkOrderId;

    			public OrderDrinkEventArgs(string drinkOrderId)
    			{
    				this.drinkOrderId = drinkOrderId;
    			}

    			public string DrinkOrderId
    			{
    				get { return drinkOrderId; }
    			}
    		}
    }
}
