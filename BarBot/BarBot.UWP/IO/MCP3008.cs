using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.Spi;
using System.Diagnostics;

namespace BarBot.UWP.IO
{
    /// <summary>
    /// Analog to digital converter
    /// </summary>
    public class MCP3008
    {
        private const int CLOCK_FREQUENCY = 3600000; // 3.6 MHz

        private SpiDevice _mcp3008;

        public async void connect(int spiBus = 0)
        {
            var spiSettings = new SpiConnectionSettings(spiBus);
            spiSettings.ClockFrequency = CLOCK_FREQUENCY;
            spiSettings.Mode = SpiMode.Mode0;

            string spiQuery = SpiDevice.GetDeviceSelector("SPI0");
            var deviceInfo = await DeviceInformation.FindAllAsync(spiQuery);
            if (deviceInfo != null && deviceInfo.Count > 0)
            {
                _mcp3008 = await SpiDevice.FromIdAsync(deviceInfo[0].Id, spiSettings);
            }
            else
            {
                Debug.WriteLine("MCP3008 device not found...");
            }
        }

        /// <summary>
        /// Read the value from the specified channel on the MCP3008. Must be 0 - 7.
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        public int read(int channel)
        {
            //From data sheet -- 1 byte selector for channel 0 on the ADC
            //First Byte sends the Start bit for SPI
            //Second Byte is the Configuration Bit
            //1 - single ended 
            //0 - d2
            //0 - d1
            //0 - d0
            //             S321XXXX <-- single-ended channel selection configure bits
            // Channel 0 = 10000000 = 0x80 OR (8+channel) << 4
            int channelByte = (8 + channel) << 4;
            var transmitBuffer = new byte[3] { 1, (byte)channelByte, 0x00 };
            var receiveBuffer = new byte[3];

            _mcp3008.TransferFullDuplex(transmitBuffer, receiveBuffer);

            //first byte returned is 0 (00000000), 
            //second byte returned we are only interested in the last 2 bits 00000011 (mask of &3) 
            //then shift result 8 bits to make room for the data from the 3rd byte (makes 10 bits total)
            //third byte, need all bits, simply add it to the above result 
            var result = ((receiveBuffer[1] & 3) << 8) + receiveBuffer[2];

            Debug.WriteLine("Read value from MCP3008: " + result);

            return result;
        }

        public int read(int channel, int samples)
        {
            int val = 0;
            for(int i = 0; i < samples; i++)
            {
                val += read(channel);
                Task.Delay(10);
            }

            return val / samples;
        }
    }
}
