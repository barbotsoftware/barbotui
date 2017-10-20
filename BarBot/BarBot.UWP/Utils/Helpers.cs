using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarBot.Core.Model;
using BarBot.UWP.IO;

namespace BarBot.UWP.Utils
{
    public class Helpers
    {
        public static Dictionary<IO.Devices.IContainer, double> GetContainersFromRecipe(Recipe recipe, List<IO.Devices.V1.Container> containers)
        {
            Dictionary<IO.Devices.IContainer, double> res = new Dictionary<IO.Devices.IContainer, double>();
            foreach(Ingredient ingredient in recipe.Ingredients)
            {
                try
                {
                    IO.Devices.IContainer container = containers.Where(x => x.IngredientId.Equals(ingredient.IngredientId)).First();
                    //res.Add(container, ingredient.Quantity);
                } 
                catch (Exception e)
                {
                    // Ingredient not found
                }
            }
            return res;
        }

        /// <summary>
        /// Accepts two lists of Ingredients as parameters, one with Ingredient objects
        /// that have names and ingredient IDs (Global Ingredient List) and one with only
        /// ingredient IDs (Recipe Ingredients). Sets the names on these Ingredient objects 
        /// and returns that list.
        /// </summary>
        /// <param name="IngredientsWithNames"></param>
        /// <param name="IngredientsInRecipe"></param>
        /// <returns></returns>
        public static List<Ingredient> GetIngredientsWithNames(List<Ingredient> IngredientsWithNames, List<Ingredient> IngredientsInRecipe)
        {
            foreach (Ingredient ingredient in IngredientsInRecipe)
            {
                Ingredient foundIngredient = IngredientsWithNames.Where(x => x.IngredientId.Equals(ingredient.IngredientId)).First();

                if (foundIngredient != null)
                {
                    ingredient.Name = foundIngredient.Name;
                }
            }

            return IngredientsInRecipe;
        }

        public static Ingredient GetIngredientByIngredientId(List<Ingredient> ingredients, string ingredientId)
        {
            if (ingredients.Count > 0)
            {
                return ingredients.Where(x => x.IngredientId.Equals(ingredientId)).First();
            }

            return null;
        }

        public static string UppercaseWords(string value)
        {
            char[] array = value.ToCharArray();
            // Handle the first letter in the string.
            if (array.Length >= 1)
            {
                if (char.IsLower(array[0]))
                {
                    array[0] = char.ToUpper(array[0]);
                }
            }
            // Scan through the letters, checking for spaces.
            // ... Uppercase the lowercase letters following spaces.
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i - 1] == ' ')
                {
                    if (char.IsLower(array[i]))
                    {
                        array[i] = char.ToUpper(array[i]);
                    }
                }
            }
            return new string(array);
        }
    }
}
