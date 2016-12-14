using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarBot.UWP.Database
{
    public class IOPort
    {
        /// <summary>
        /// Unique ID of the io port
        /// </summary>
        public int ioPortId { get; set; }

        /// <summary>
        /// IOPort type (1 = Board, 2 = I2C 1, 3 = I2C 2)
        /// </summary>
        public int type { get; set; }

        /// <summary>
        /// IO Port address
        /// </summary>
        public int address { get; set; }

        /// <summary>
        /// Name/alias of the io port
        /// </summary>
        public string name { get; set; }
    }
}
