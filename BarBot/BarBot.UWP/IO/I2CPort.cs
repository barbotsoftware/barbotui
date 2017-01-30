using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using System.Diagnostics;

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

        public string Name { get; set; }

        public I2CPort(MCP23017 mcp, int address)
        {
            Pin = (MCP23017.Pin) address; 
            MCP = mcp;
        }

        public void write(GpioPinValue val)
        {
            try
            {
                MCP.SetDriveMode(Pin, MCP23017.PinMode.Output);
                MCP.Write(Pin, (MCP23017.PinValue)val);
            }
            catch (Exception e)
            {
                Debug.WriteLine(string.Format("Failed to write to pin {0} : {1}", Pin, e.Message));
            }
        }
    }
}
