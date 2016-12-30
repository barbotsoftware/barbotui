using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using System.Diagnostics;

namespace BarBot.UWP.IO.Devices.V1
{
    /// <summary>
    /// V1 garnish dispenser implementation 
    /// </summary>
    public class GarnishDispenser : IGarnishDispenser
    {
        public L298NDriver stepperDriver;

        public GarnishDispenser() { }

        public GarnishDispenser(IIOPort stepper1, IIOPort stepper2, IIOPort stepper3, IIOPort stepper4)
        {
            stepperDriver = new L298NDriver(stepper1, stepper2, stepper3, stepper4);
        }

        public void AddGarnish()
        {
            Debug.WriteLine(string.Format("Running garnish dispenser"));

            stepperDriver.run(1);
        }
    }
}
