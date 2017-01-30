using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarBot.UWP.Database
{
    public class CupDispenser
    {
        /// <summary>
        /// Unique ID of the cup dispenser
        /// </summary>
        public int cupDispenserId { get; set; }

        /// <summary>
        /// IO ports for the stepper motor
        /// </summary>
        public IOPort stepper1 { get; set; }
        public IOPort stepper2 { get; set; }
        public IOPort stepper3 { get; set; }
        public IOPort stepper4 { get; set; }

        /// <summary>
        /// IO port for the FSR
        /// </summary>
        public IOPort fsr { get; set; }
    }
}
