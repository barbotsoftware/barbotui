using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace BarBot.UWP.IO.Devices.V1
{
    /// <summary>
    /// V1 Cup dispenser implementation
    /// </summary>
    public class CupDispenser : ICupDispenser
    {
        public L298NDriver stepperDriver;

        public CupDispenser() { }

        public CupDispenser(IIOPort stepper1, IIOPort stepper2, IIOPort stepper3, IIOPort stepper4)
        {
            stepperDriver = new L298NDriver(stepper1, stepper2, stepper3, stepper4);
        }

        public void DispenseCup()
        {
            stepperDriver.run(1);
        }
    }
}
