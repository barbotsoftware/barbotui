using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarBot.UWP.IO.Devices;
using Windows.Devices.Gpio;

namespace BarBot.UWP.IO
{

    /// <summary>
    /// L298N stepper motor driver
    /// </summary>
    public class L298NDriver
    {
        public IIOPort portA;
        public IIOPort portB;
        public IIOPort portC;
        public IIOPort portD;

        private const int STEPS_PER_REVOLUTION = 60;

        public L298NDriver(IIOPort stepper1, IIOPort stepper2, IIOPort stepper3, IIOPort stepper4)
        {
            portA = stepper1;
            portB = stepper2;
            portC = stepper3;
            portD = stepper4;
        }

        public void run(int revolutions)
        {
            for(int i = 0; i < revolutions * STEPS_PER_REVOLUTION; i++)
            {
                portA.write(GpioPinValue.Low);
                portB.write(GpioPinValue.High);

                Task.Delay(1);

                portD.write(GpioPinValue.Low);
                portC.write(GpioPinValue.High);

                Task.Delay(1);

                portB.write(GpioPinValue.Low);
                portA.write(GpioPinValue.High);

                Task.Delay(1);

                portC.write(GpioPinValue.Low);
                portD.write(GpioPinValue.High);

                Task.Delay(1);
            }

            portA.write(GpioPinValue.Low);
            portB.write(GpioPinValue.Low);
            portC.write(GpioPinValue.Low);
            portD.write(GpioPinValue.Low);
        }
    }
}
