using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace BarBot.UWP.IO.Devices.V1
{
    /// <summary>
    /// V1 pump implementation
    /// </summary>
    public class Pump : IPump
    {
        public Pump(IOPort ioPort)
        {
            IOPort = ioPort;
        }

        public IIOPort IOPort { get; set; }

        public IFlowSensor FlowSensor { get; set; }

        /// <summary>
        /// Opens (writes HIGH) to the given port
        /// </summary>
        public void StartPump()
        {
            IOPort.write(GpioPinValue.High);
        }

        /// <summary>
        /// Closes (writes LOW) to the given port
        /// </summary>
        public void StopPump()
        {
            IOPort.write(GpioPinValue.Low);
        }
    }
}
