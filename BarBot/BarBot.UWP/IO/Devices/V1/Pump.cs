using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using System.Diagnostics;

namespace BarBot.UWP.IO.Devices.V1
{
    /// <summary>
    /// V1 pump implementation
    /// </summary>
    public class Pump : IPump
    {
        public IIOPort IOPort;

        public IFlowSensor FlowSensor;

        public Pump(IIOPort ioPort)
        {
            IOPort = ioPort;
        }

        /// <summary>
        /// Opens (writes HIGH) to the given port
        /// </summary>
        public void StartPump()
        {
            IOPort.write(GpioPinValue.High);

            Debug.WriteLine(string.Format("Started {0}", IOPort.Name));
        }

        /// <summary>
        /// Closes (writes LOW) to the given port
        /// </summary>
        public void StopPump()
        {
            IOPort.write(GpioPinValue.Low);

            Debug.WriteLine(string.Format("Stopped {0}", IOPort.Name));
        }
    }
}
