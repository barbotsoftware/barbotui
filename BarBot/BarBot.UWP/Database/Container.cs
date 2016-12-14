using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarBot.UWP.Database
{
    public class Container
    {
        /// <summary>
        /// Unique ID
        /// </summary>
        public int containerId { get; set; }

        /// <summary>
        /// Unique ID string of the ingredient (from remote DB)
        /// </summary>
        public string ingredientId { get; set; }

        /// <summary>
        /// Pump associated with this container
        /// </summary>
        public Pump pump { get; set; }

        /// <summary>
        /// Flow sensor associated with this container
        /// </summary>
        public FlowSensor flowSensor { get; set; }
    }
}
