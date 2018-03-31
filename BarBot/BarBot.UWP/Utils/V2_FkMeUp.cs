using BarBot.Core;
using BarBot.Core.Model;
using System;
using System.Collections.Generic;

namespace BarBot.UWP.Utils
{
    class V2_FkMeUp
    {

        public static Recipe FuckMeUp()
        {
            Recipe FuckMeUp = new Recipe();
            FuckMeUp.Name = "Fuck Me Up";
            FuckMeUp.Img = "barbotweb/public/img/recipe_images/dickbutt.png";
            FuckMeUp.Ingredients = new List<Ingredient>();

            return FuckMeUp;
        }

        public static List<Ingredient> LoadEmUpBoiz(List<Ingredient> AvailableIngredientList)
        {
            List<Ingredient> demIngredients = new List<Ingredient>();
            double totalQuantity = 0;
            if (AvailableIngredientList != null)
            {
                List<int> usedIngredients = new List<int>();
                Random r = new Random();
                int ingredientCount = r.Next(3, AvailableIngredientList.Count);
                for (int i = 0; i < ingredientCount; i++)
                {
                    bool ingredientChosen = false;
                    while (!ingredientChosen)
                    {
                        int ingredientID = r.Next(0, AvailableIngredientList.Count);
                        if (usedIngredients.IndexOf(ingredientID) < 0)
                        {
                            usedIngredients.Add(ingredientID);
                            Ingredient ingy = AvailableIngredientList[ingredientID];
                            double quant = r.Next(1, (int)Constants.MaxVolume / 2);
                            if (totalQuantity + quant > Constants.MaxVolume)
                            {
                                quant = Constants.MaxVolume - totalQuantity;
                            }
                            totalQuantity += quant;
                            ingy.Amount = quant;
                            demIngredients.Add(AvailableIngredientList[ingredientID]);
                            ingredientChosen = true;
                        }
                    }
                    if(totalQuantity >= Constants.MaxVolume)
                    {
                        break;
                    }
                }
            }
            return demIngredients;
        }
    }
}
