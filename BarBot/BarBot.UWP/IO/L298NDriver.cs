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

        private int STEPS_PER_REVOLUTION = 50;
        public int SleepTime { get; set; } 

        public L298NDriver(IIOPort stepper1, IIOPort stepper2, IIOPort stepper3, IIOPort stepper4, int sleepTime = 15)
        {
            portA = stepper1;
            portB = stepper2;
            portC = stepper3;
            portD = stepper4;

            SleepTime = sleepTime;
        }

        public void run(double revolutions)
        {
            Debug.WriteLine(string.Format("Running stepper for {0} revolutions", revolutions));

            for (int i = 0; i < revolutions * STEPS_PER_REVOLUTION; i++)
            {
                portA.write(GpioPinValue.Low);
                portB.write(GpioPinValue.High);

                Task.Delay(SleepTime).Wait();

                portD.write(GpioPinValue.Low);
                portC.write(GpioPinValue.High);

                Task.Delay(SleepTime).Wait();

                portB.write(GpioPinValue.Low);
                portA.write(GpioPinValue.High);

                Task.Delay(SleepTime).Wait();

                portC.write(GpioPinValue.Low);
                portD.write(GpioPinValue.High);

                Task.Delay(SleepTime).Wait();
            }

            portA.write(GpioPinValue.Low);
            portB.write(GpioPinValue.Low);
            portC.write(GpioPinValue.Low);
            portD.write(GpioPinValue.Low);

            Debug.WriteLine(string.Format("Finished running stepper"));
        }

        public void runBackwards(double revolutions)
        {
            Debug.WriteLine(string.Format("Running stepper for backwards {0} revolutions", revolutions));

            for (int i = 0; i < revolutions * STEPS_PER_REVOLUTION; i++)
            {
                portC.write(GpioPinValue.Low);
                portD.write(GpioPinValue.High);

                Task.Delay(SleepTime).Wait();

                portB.write(GpioPinValue.Low);
                portA.write(GpioPinValue.High);

                Task.Delay(SleepTime).Wait();

                portD.write(GpioPinValue.Low);
                portC.write(GpioPinValue.High);

                Task.Delay(SleepTime).Wait();

                portA.write(GpioPinValue.Low);
                portB.write(GpioPinValue.High);

                Task.Delay(SleepTime).Wait();
            }

            portA.write(GpioPinValue.Low);
            portB.write(GpioPinValue.Low);
            portC.write(GpioPinValue.Low);
            portD.write(GpioPinValue.Low);

            Debug.WriteLine(string.Format("Finished running stepper"));
        }
    }
}
