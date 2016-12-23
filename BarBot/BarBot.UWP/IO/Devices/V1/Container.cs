using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarBot.UWP.IO.Devices.V1
{
    public class Container : IContainer
    {
        public Container(FlowSensor flowSensor, Pump pump)
        {
            FlowSensor = flowSensor;
            Pump = pump;
        }

        public Pump Pump;
        public FlowSensor FlowSensor;
    }
}
