using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarBot.UWP.Database
{
    public class GarnishDispenser
    {
        /// <summary>
        /// Unique ID of the garnish dispenser 
        /// </summary>
        public int garnishDispenserId { get; set; }

        /// <summary>
        /// IO ports for the stepper motors
        /// </summary>
        public IOPort stepper1 { get; set; }
        public IOPort stepper2 { get; set; }
        public IOPort stepper3 { get; set; }
        public IOPort stepper4 { get; set; }
        public IOPort stepper5 { get; set; }
        public IOPort stepper6 { get; set; }
        public IOPort stepper7 { get; set; }
        public IOPort stepper8 { get; set; }

    }
}
