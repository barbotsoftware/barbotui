using BarBot.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.UI.Xaml.Media;

namespace BarBot.UWP.Utils
{
    public class Helpers
    {
        public static Point GetPoint(int i, int width)
        {
            int margin = 25;
            int hexPadding = 12;

        int pos = i % 4;
            int r = i / 4;

            int top = 0;
            int left = 0;
            int height = (int)(2 * Math.Sqrt(Math.Pow(width / 2, 2) - Math.Pow(width / 4, 2)));

            // Top Left
            if (pos == 0)
            {
                top = 0;
                left = r * (width + (width / 2) + (hexPadding * 2));
            }
            // Bottom Left
            else if (pos == 1)
            {
                top = height + hexPadding;
                left = r * (width + (width / 2) + (hexPadding * 2));
            }
            // Top right (Diagonally down and right from top left)
            else if (pos == 2)
            {
                top = height / 2 + hexPadding / 2;
                left = (width - (width / 4) + hexPadding) + (r * (width + (width / 2) + (hexPadding * 2)));
            }
            // Bottom right (Diagonally down and right from bottom left)
            else if (pos == 3)
            {
                top = height + hexPadding + height / 2 + hexPadding / 2;
                left = (width - (width / 4) + hexPadding) + (r * (width + (width / 2) + (hexPadding * 2)));
            }

            return new Point(left, top + margin);
        }

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

        public static string CleanInput(string strIn)
        {
            // Replace invalid characters with empty strings.
            try
            {
                return Regex.Replace(strIn, @"[^a-zA-Z0-9 -]", "",
                                     RegexOptions.None, TimeSpan.FromSeconds(1.5));
            }
            // If we timeout when replacing invalid characters, 
            // we should return Empty.
            catch (RegexMatchTimeoutException)
            {
                return String.Empty;
            }
        }
    }
}
