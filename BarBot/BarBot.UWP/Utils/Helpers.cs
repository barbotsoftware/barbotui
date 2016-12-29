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
                    res.Add(container, ingredient.Quantity);
                } 
                catch (Exception e)
                {
                    // Ingredient not found
                }
            }
            return res;
        }
    }
}
