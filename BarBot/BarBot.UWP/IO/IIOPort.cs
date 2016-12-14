using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace BarBot.UWP.IO.Devices
{
    public interface IIOPort
    {
        void write(GpioPinValue val);
    }
}
