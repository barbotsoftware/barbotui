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
        public L298NDriver stepperDriver;

        public L298NDriver stepperDriver2;

        public IOPort FSR2;

        public IceHopper() { }

        public IceHopper(IIOPort stepper1, IIOPort stepper2, IIOPort stepper3, IIOPort stepper4, IOPort fsr2)
        {
            // Create stepper drivers for both stepper motors
            stepperDriver = new L298NDriver(stepper1, stepper2, stepper3, stepper4);

            FSR2 = fsr2;
        }

        public void AddIce()
        {
            Debug.WriteLine(string.Format("Running ice hopper"));

            bool forward = true;
            bool triggered = FSR2.GpioPin.Read().Equals(GpioPinValue.High);
            while (!triggered)
            {
                if (forward)
                    stepperDriver.run(1);
                else
                    stepperDriver.runBackwards(1);

                forward = !forward;
                triggered = FSR2.GpioPin.Read().Equals(GpioPinValue.High);
            }

            Debug.WriteLine("Finished adding ice");
        }
    }
}
