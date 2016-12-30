using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarBot.UWP.IO.Devices;
using Windows.Devices.Gpio;
using System.Diagnostics;

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

        private int STEPS_PER_REVOLUTION = 60;
        private int SLEEP_TIME = 10; 

        public L298NDriver(IIOPort stepper1, IIOPort stepper2, IIOPort stepper3, IIOPort stepper4)
        {
            portA = stepper1;
            portB = stepper2;
            portC = stepper3;
            portD = stepper4;
        }

        public void run(int revolutions)
        {
            Debug.WriteLine(string.Format("Running stepper for {0} revolutions", revolutions));

            for (int i = 0; i < revolutions * STEPS_PER_REVOLUTION; i++)
            {
                portA.write(GpioPinValue.Low);
                portB.write(GpioPinValue.High);

                Task.Delay(SLEEP_TIME).Wait();

                portD.write(GpioPinValue.Low);
                portC.write(GpioPinValue.High);

                Task.Delay(SLEEP_TIME).Wait();

                portB.write(GpioPinValue.Low);
                portA.write(GpioPinValue.High);

                Task.Delay(SLEEP_TIME).Wait();

                portC.write(GpioPinValue.Low);
                portD.write(GpioPinValue.High);

                Task.Delay(SLEEP_TIME).Wait();
            }

            portA.write(GpioPinValue.Low);
            portB.write(GpioPinValue.Low);
            portC.write(GpioPinValue.Low);
            portD.write(GpioPinValue.Low);

            Debug.WriteLine(string.Format("Finished running stepper"));
        }

        public void runBackwards(int revolutions)
        {
            Debug.WriteLine(string.Format("Running stepper for backwards {0} revolutions", revolutions));

            for (int i = 0; i < revolutions * STEPS_PER_REVOLUTION; i++)
            {
                portA.write(GpioPinValue.Low);
                portB.write(GpioPinValue.High);

                Task.Delay(SLEEP_TIME).Wait();

                portC.write(GpioPinValue.Low);
                portD.write(GpioPinValue.High);

                Task.Delay(SLEEP_TIME).Wait();

                portB.write(GpioPinValue.Low);
                portA.write(GpioPinValue.High);

                Task.Delay(SLEEP_TIME).Wait();

                portD.write(GpioPinValue.Low);
                portC.write(GpioPinValue.High);

                Task.Delay(SLEEP_TIME).Wait();
            }

            portA.write(GpioPinValue.Low);
            portB.write(GpioPinValue.Low);
            portC.write(GpioPinValue.Low);
            portD.write(GpioPinValue.Low);

            Debug.WriteLine(string.Format("Finished running stepper"));
        }
    }
}
