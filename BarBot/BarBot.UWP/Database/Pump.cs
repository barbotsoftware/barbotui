using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarBot.UWP.Database
{
    public class Pump
    {
        /// <summary>
        /// Unique ID of the pump
        /// </summary>
        public int pumpId { get; set; }

        /// <summary>
        /// IO Port the pump is connected to
        /// </summary>
        public IOPort ioPort { get; set; }

        /// <summary>
        /// Container the pump is associated with
        /// </summary>
        public int containerId { get; set; }
        public Container container { get; set; }
    }
}
