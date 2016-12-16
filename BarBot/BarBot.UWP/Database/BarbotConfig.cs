using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarBot.UWP.Database
{
    /// <summary>
    /// Miscellaneous barbot-specific configuration options
    /// </summary>
    public class BarbotConfig
    {
        public int barbotConfigId { get; set; }

        /// <summary>
        /// Unique ID of the barbot machine
        /// </summary>
        public string barbotId { get; set; }

        /// <summary>
        /// Fullyu qualified url of the websocket endpoint 
        /// </summary>
        public string apiEndpoint { get; set; }
    }
}
