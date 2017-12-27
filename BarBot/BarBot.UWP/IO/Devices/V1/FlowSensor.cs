using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarBot.UWP.IO.Devices.V1
{
    /// <summary>
    /// V1 flow sensor implementation
    /// </summary>
    public class FlowSensor : IFlowSensor
    {
        public FlowSensor(IOPort ioPort, double calibrationFactor)
        {
            IoPort = ioPort;
            CalibrationFactor = calibrationFactor;
        }

        public IOPort IoPort;
        public Pump Pump;
        public double CalibrationFactor { get; set; }
    }
}
