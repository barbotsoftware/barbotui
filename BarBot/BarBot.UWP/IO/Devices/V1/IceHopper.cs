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

        public IceHopper() { }

        public IOPort ReedSwitch;

        private bool ReedSwitchIsOpen = false;

        public IceHopper(IIOPort stepper1, IIOPort stepper2, IIOPort stepper3, IIOPort stepper4, IOPort reedSwitch)
        {
            stepperDriver = new L298NDriver(stepper1, stepper2, stepper3, stepper4);

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

            while (!ReedSwitchIsOpen)
            {
                stepperDriver.run(1);

                stepperDriver.runBackwards(1);
            }

            Debug.WriteLine("Finished adding ice");
        }
    }
}
