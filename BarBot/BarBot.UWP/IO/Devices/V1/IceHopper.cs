using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarBot.UWP.IO.Devices.V1
{
    /// <summary>
    /// V1 ice hopper implementation
    /// </summary>
    public class IceHopper : IIceHopper
    {
        public L298NDriver stepperDriver;

        public IceHopper() { }

        public IceHopper(IIOPort stepper1, IIOPort stepper2, IIOPort stepper3, IIOPort stepper4)
        {
            stepperDriver = new L298NDriver(stepper1, stepper2, stepper3, stepper4);
        }

        public void AddIce()
        {
            stepperDriver.run(1);
        }
    }
}
