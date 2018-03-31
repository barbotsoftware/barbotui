using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Windows.Devices.Gpio;

namespace BarBot.UWP.IO.Devices.V1
{
    /// <summary>
    /// V1 ice hopper implementation
    /// </summary>
    public class IceHopper : IIceHopper
    {
        private const int THRESHOLD_FULL = 350;

        public L298NDriver stepperDriver;

        public L298NDriver stepperDriver2;

        public MCP3008 mcp3008 = new MCP3008();

        IOPort augerMotor;

        private double RUN_LENGTH = 3; // run ice hopper in 3 second increments

        private int MAX_CYCLES = 5; // maximum number of times the ice hopper should run its cycle, in case it gets stuck or is empty it won't keep goin

        public IceHopper() { }

        public IceHopper(IIOPort stepper1, IIOPort stepper2, IIOPort stepper3, IIOPort stepper4, IOPort augerMotor, MCP3008 mcp3008)
        {
            // Create stepper drivers for the stepper motor for the door
            stepperDriver = new L298NDriver(stepper1, stepper2, stepper3, stepper4);

            // set the analog to digital converter for the force sensor
            this.mcp3008 = mcp3008;

            // set the IO port to use for the auger
            this.augerMotor = augerMotor;
        }

        public void AddIce()
        {
            Debug.WriteLine(string.Format("Running ice hopper"));

            int cycles = 1;
            while (mcp3008.read(0, 5) <= THRESHOLD_FULL && cycles <= MAX_CYCLES)
            {
                Debug.WriteLine("Opening ice hopper door...");

                // Open the door (1/4 rev)
                stepperDriver.runBackwards(0.22);

                Debug.WriteLine("Running auger...");
                // Start the auger motor
                augerMotor.write(GpioPinValue.High);

                // Wait for the auger to run for a bit
                delay(RUN_LENGTH);

                Debug.WriteLine("Stopping auger after " + RUN_LENGTH + " seconds");
                // Stop the auger motor
                augerMotor.write(GpioPinValue.Low);

                Debug.WriteLine("Closing ice hopper door...");
                // Close the door (1/4 rev)
                stepperDriver.run(0.22);

                // wait for it to settle
                delay(1);

                // increment cycles count
                cycles++;
            }

            Debug.WriteLine("Finished adding ice");
        }

        private void delay(double seconds)
        {
            long start = DateTime.Now.Ticks;
            while (true)
            {
                if (DateTime.Now.Ticks - start > TimeSpan.TicksPerSecond * seconds)
                {
                    break;
                }
            }
        }
    }
}
