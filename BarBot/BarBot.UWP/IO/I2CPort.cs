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
        MCP23017.Pin Pin { get; set; }

        public MCP23017 MCP;

        public I2CPort() { }

        public I2CPort(MCP23017 mcp, int address)
        {
            Pin = (MCP23017.Pin) address; 
            MCP = mcp;
        }

        public void write(GpioPinValue val)
        {
            MCP.SetDriveMode(Pin, MCP23017.PinMode.Output);
            MCP.Write(Pin, (MCP23017.PinValue) val);
        }
    }
}
