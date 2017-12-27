using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarBot.UWP.Database
{
    public class FlowSensor
    {
        /// <summary>
        /// Unique ID of the flow sensor
        /// </summary>
        public int flowSensorId { get; set; }

        /// <summary>
        /// IO Port the sensor is connected to
        /// </summary>
        public IOPort ioPort { get; set; }

        /// <summary>
        /// Container the sensor is associated with
        /// </summary>
        public int containerId { get; set; }
        public Container container { get; set; }

        /// <summary>
        /// Number of ticks per ounce for this sensor (default = 32)
        /// </summary>
        public double calibrationFactor { get; set; }
    }
}
