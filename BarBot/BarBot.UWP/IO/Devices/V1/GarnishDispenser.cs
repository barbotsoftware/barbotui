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

        // 1/3 rev = 120 degrees
        private double revolutions = 0.33;

        public GarnishDispenser(IIOPort stepper1, IIOPort stepper2, IIOPort stepper3, IIOPort stepper4)
        {
            stepperDriver = new L298NDriver(stepper1, stepper2, stepper3, stepper4, 10);
        }

        public void AddGarnish(int garnishType = 1)
        {
            Debug.WriteLine(string.Format("Running garnish dispenser"));

            if (garnishType == 1) // first garnish type, run in one direction
            {
                // Rotate once to dispense garnish
                stepperDriver.runBackwards(revolutions);

                // Return to starting position
                stepperDriver.run(revolutions);
            }
            else if(garnishType == 2) // second garnish type, run in the other direction
            {
                // Rotate once to dispense garnish
                stepperDriver.run(revolutions);

                //Return to startin position
                stepperDriver.runBackwards(revolutions);
            }
        }
    }
}
