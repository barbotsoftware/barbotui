using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarBot.UWP.Database
{
    public class IceHopper
    {
        /// <summary>
        /// Unique ID of the ice hopper
        /// </summary>
        public int iceHopperId { get; set; }

        /// <summary>
        /// IO ports for the stepper motor
        /// </summary>
        public IOPort stepper1 { get; set; }
        public IOPort stepper2 { get; set; }
        public IOPort stepper3 { get; set; }
        public IOPort stepper4 { get; set; }
    }
}
