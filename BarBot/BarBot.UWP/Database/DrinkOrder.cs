using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarBot.UWP.Database
{
    public class DrinkOrder
    {
        /// <summary>
        /// Unique id of the drink order
        /// </summary>
        public int drinkOrderId { get; set; }

        /// <summary>
        /// UID string of the drink order
        /// </summary>
        public string drinkOrderUID { get; set; }

        /// <summary>
        /// UID of the user who created the drink order
        /// </summary>
        public string userId { get; set; }

        /// <summary>
        /// Name of the user who created the drink order
        /// </summary>
        public string userName { get; set; }

        /// <summary>
        /// UID of the recipe
        /// </summary>
        public string recipeId { get; set; }

        /// <summary>
        /// Name of the recipe
        /// </summary>
        public string recipeName { get; set; }

        /// <summary>
        /// Timestamp of the creation time of the drink order
        /// </summary>
        public string timestamp { get; set; }

        /// <summary>
        /// Ice on/off
        /// </summary>
        public bool ice { get; set; }

        /// <summary>
        /// Garnish on/off
        /// </summary>
        public bool garnish { get; set; }
    }
}
