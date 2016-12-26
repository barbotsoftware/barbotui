using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarBot.UWP.IO.Devices
{
    public interface IPump
    {
        void StartPump();
        void StopPump();
    }
}
