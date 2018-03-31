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

        public delegate void GetGarnishesEventHandler(object sender, GetGarnishesEventArgs args);

        public class GetGarnishesEventArgs : EventArgs
        {
            public List<Garnish> garnishes;

            public GetGarnishesEventArgs(List<Garnish> garnishes)
            {
                this.garnishes = garnishes;
            }

            public List<Garnish> Garnishes
            {
                get { return garnishes; }
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

		public delegate void CreateCustomRecipeEventHandler(object sender, CreateCustomRecipeEventArgs args);

		public class CreateCustomRecipeEventArgs : EventArgs
		{
			private string recipeId;

			public CreateCustomRecipeEventArgs(string recipeId)
			{
				this.recipeId = recipeId;
			}

			public string RecipeId
			{
				get { return recipeId; }
			}
		}

        public delegate void GetCategoriesEventHandler(object sender, GetCategoriesEventArgs args);

        public class GetCategoriesEventArgs : EventArgs
        {
            private List<Category> categories;

            public GetCategoriesEventArgs(List<Category> categories)
            {
                this.categories = categories;
            }

            public List<Category> Categories
            {
                get { return categories; }
            }
        }

        public delegate void GetCategoryEventHandler(object sender, GetCategoryEventArgs args);

        public class GetCategoryEventArgs : EventArgs
        {
            private Category category;

            public GetCategoryEventArgs(Category category)
            {
                this.category = category;
            }

            public Category Category
            {
                get { return category; }
            }
        }

        public delegate void GetContainersEventHandler(object sender, GetContainersEventArgs args);

        public class GetContainersEventArgs : EventArgs
        {
            private List<Container> containers;

            public GetContainersEventArgs(List<Container> containers)
            {
                this.containers = containers;
            }

            public List<Container> Containers
            {
                get { return containers; }
            }
        }

        public delegate void UpdateContainerEventHandler(object sender, UpdateContainerEventArgs args);

        public class UpdateContainerEventArgs : EventArgs
        {
            private Container container;

            public UpdateContainerEventArgs(Container container)
            {
                this.container = container;
            }

            public Container Container
            {
                get { return container; }
            }
        }

        public delegate void UpdateGarnishEventHandler(object sender, UpdateGarnishEventArgs args);

        public class UpdateGarnishEventArgs : EventArgs
        {
            private Garnish garnish;

            public UpdateGarnishEventArgs(Garnish garnish)
            {
                this.garnish = garnish;
            }

            public Garnish Garnish
            {
                get { return garnish; }
            }
        }
    }
}
