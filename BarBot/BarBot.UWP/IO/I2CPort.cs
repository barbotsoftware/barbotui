using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace BarBot.UWP.IO.Devices
{
    /// <summary>
    /// I2C extender port
    /// </summary>
    public class I2CPort : IIOPort
    {
        int address { get; set; }

        GpioPin gpioPin { get; set; }

        public void write(GpioPinValue val)
        {

        }
    }
}
