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

        /// <summary>
        /// IO ports for the second stepper motor
        /// </summary>
        public IOPort stepper5 { get; set; }
        public IOPort stepper6 { get; set; }
        public IOPort stepper7 { get; set; }
        public IOPort stepper8 { get; set; }

        /// <summary>
        /// IO port for the reed switch
        /// </summary>
        public IOPort reedSwitch { get; set; }
    }
}
