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
        public L298NDriver stepperDriver;

        public IOPort fsr1;

        public CupDispenser() { }

        public CupDispenser(IIOPort stepper1, IIOPort stepper2, IIOPort stepper3, IIOPort stepper4, IOPort fsr1)
        {
            stepperDriver = new L298NDriver(stepper1, stepper2, stepper3, stepper4, 7);
            this.fsr1 = fsr1;
        }

        public void DispenseCup()
        {
            Debug.WriteLine(string.Format("Running cup dispenser"));

            bool triggered = fsr1.GpioPin.Read().Equals(GpioPinValue.High);
            if (!triggered)
            {
                // Attempt to release a cup
                stepperDriver.run(1);

                while (!triggered)
                {
                    // Wait a bit for it to settle
                    Task.Delay(100);

                    // Check if the weight sensor has been triggered
                    triggered = fsr1.GpioPin.Read().Equals(GpioPinValue.High);
                }
            }
        }
    }
}
