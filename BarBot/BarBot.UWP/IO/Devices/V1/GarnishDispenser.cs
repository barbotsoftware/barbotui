using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace BarBot.UWP.IO.Devices.V1
{
    /// <summary>
    /// V1 garnish dispenser implementation 
    /// </summary>
    public class GarnishDispenser : IGarnishDispenser
    {
        IIOPort stepper1 { get; set; }
        IIOPort stepper2 { get; set; }
        IIOPort stepper3 { get; set; }
        IIOPort stepper4 { get; set; }

        public GarnishDispenser() { }

        public GarnishDispenser(IIOPort stepper1, IIOPort stepper2, IIOPort stepper3, IIOPort stepper4)
        {
            this.stepper1 = stepper1;
            this.stepper2 = stepper2;
            this.stepper3 = stepper3;
            this.stepper4 = stepper4;
        }

        public void AddGarnish()
        {

        }
    }
}
