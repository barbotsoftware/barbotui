using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarBot.UWP.IO.Devices
{
    public interface IPump
    {
        IIOPort IOPort { get; set; }
        IFlowSensor FlowSensor { get; set; }
        void StartPump();
        void StopPump();
    }
}
