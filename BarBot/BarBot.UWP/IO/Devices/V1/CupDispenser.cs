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
    /// V1 Cup dispenser implementation
    /// </summary>
    public class CupDispenser : ICupDispenser
    {
        private int EMPTY_CUP_THRESHOLD = 100;

        public L298NDriver stepperDriver;

        public MCP3008 mcp3008;

        public CupDispenser() { }

        public CupDispenser(IIOPort stepper1, IIOPort stepper2, IIOPort stepper3, IIOPort stepper4, MCP3008 mcp3008)
        {
            // initialize a new stepper driver
            stepperDriver = new L298NDriver(stepper1, stepper2, stepper3, stepper4, 4);

            // set the analog to digital converter for the force sensor
            this.mcp3008 = mcp3008;
        }

        public void DispenseCup()
        {
            Debug.WriteLine(string.Format("Running cup dispenser"));

            bool triggered = mcp3008.read(0) >= EMPTY_CUP_THRESHOLD; // TODO: hard coded channel #
            if (!triggered)
            {
                // Attempt to release a cup
                stepperDriver.run(1);

                // Jiggle 
                JiggleForPapi();

                while (!triggered)
                {
                    // Wait a bit for it to settle
                    Task.Delay(100);

                    // Check if the weight sensor has been triggered
                    triggered = mcp3008.read(0) >= EMPTY_CUP_THRESHOLD; // TODO hard coded channel #
                }
            }
        }

        public void JiggleForPapi()
        {
            stepperDriver.SleepTime = 2; // Papi likes it fast

            // jiggle jiggle jiggle
            stepperDriver.runBackwards(0.1);
            stepperDriver.run(0.1);

            stepperDriver.SleepTime = 4;
        }
    }
}
