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
        IIOPort stepper1 { get; set; }
        IIOPort stepper2 { get; set; }
        IIOPort stepper3 { get; set; }
        IIOPort stepper4 { get; set; }


        public IceHopper() { }

        public IceHopper(IIOPort stepper1, IIOPort stepper2, IIOPort stepper3, IIOPort stepper4)
        {
            this.stepper1 = stepper1;
            this.stepper2 = stepper2;
            this.stepper3 = stepper3;
            this.stepper4 = stepper4;
        }

        public void AddIce()
        {

        }
    }
}
