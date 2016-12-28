using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace BarBot.UWP.IO.Devices
{
    /// <summary>
    /// Normal IO pin directly on machine (not I2C)
    /// </summary>
    public class IOPort : IIOPort
    {
        public GpioPin GpioPin;

        public IOPort(GpioPin pin, GpioPinDriveMode mode = GpioPinDriveMode.Output)
        {
            GpioPin = pin;
            pin.Write(GpioPinValue.Low);
            pin.SetDriveMode(mode);
        }

        public void write(GpioPinValue val)
        {
            GpioPin.Write(val);
        }
    }
}
