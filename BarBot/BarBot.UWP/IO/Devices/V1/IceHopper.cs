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

        public IOPort ReedSwitch;

        public IceHopper() { }

        private bool ReedSwitchIsOpen = false;

        public IceHopper(IIOPort stepper1, IIOPort stepper2, IIOPort stepper3, IIOPort stepper4,
            IIOPort stepper5, IIOPort stepper6, IIOPort stepper7, IIOPort stepper8,
            IOPort reedSwitch)
        {
            // Create stepper drivers for both stepper motors
            stepperDriver = new L298NDriver(stepper1, stepper2, stepper3, stepper4);
            stepperDriver2 = new L298NDriver(stepper5, stepper6, stepper7, stepper8);

            // Initialize reed switch
            ReedSwitch = reedSwitch;
            ReedSwitch.GpioPin.DebounceTimeout = TimeSpan.FromMilliseconds(15);
            ReedSwitch.GpioPin.ValueChanged += GpioPin_ValueChanged;
        }

        private void GpioPin_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs args)
        {
            ReedSwitchIsOpen = args.Edge.Equals(GpioPinEdge.RisingEdge);
        }

        public void AddIce()
        {
            Debug.WriteLine(string.Format("Running ice hopper"));

            bool forward = true;
            while (!ReedSwitchIsOpen)
            {
                if (forward)
                {
                    stepperDriver.run(1);
                }
                else
                {
                    stepperDriver.runBackwards(1);
                }

                forward = !forward;
            }

            stepperDriver2.SleepTime = 7;

            stepperDriver2.run(0.25);

            stepperDriver2.runBackwards(0.25);

            Debug.WriteLine("Finished adding ice");
        }
    }
}
