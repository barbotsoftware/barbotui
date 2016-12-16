using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using System.Collections.Concurrent;
using BarBot.UWP.IO.Devices;
using BarBot.UWP.IO.Devices.V1;

namespace BarBot.UWP.IO
{
    /// <summary>
    /// V1 GPIO Controller implementation
    /// </summary>
    public class BarbotIOController : IIOController
    {
        public List<Container> Containers { get; set; }

        IceHopper IceHopper { get; set; }

        GarnishDispenser GarnishDispenser { get; set; }

        CupDispenser CupDispenser { get; set; }

        ConcurrentDictionary<GpioPin, int> sensorTicks;

        ConcurrentDictionary<GpioPin, int> maxTicks;

        ConcurrentDictionary<GpioPin, FlowSensor> pinSensorMapping;

        GpioController gpio;

        public BarbotIOController(List<Database.Container> containers, Database.IceHopper iceHopper, Database.GarnishDispenser garnishDispenser)
        {
            // Initialize gpio controller
            gpio = GpioController.GetDefault();

            // Create ice hopper
            IceHopper = new IceHopper();

            // Create garnish dispenser
            GarnishDispenser = new GarnishDispenser();

            // Create cup dispenser
            CupDispenser = new CupDispenser();

            Containers.Clear();
            foreach(Database.Container c in containers)
            {
                // Get pump pin number
                GpioPin pumpPin = gpio.OpenPin(c.pump.ioPort.address);
                IOPort pumpPort = new IOPort(pumpPin, GpioPinDriveMode.Output);

                // Get flow sensor pin number
                GpioPin sensorPin = gpio.OpenPin(c.flowSensor.ioPort.address);
                IOPort sensorPort = new IOPort(sensorPin, GpioPinDriveMode.InputPullDown);

                FlowSensor flowSensor = new FlowSensor(sensorPort, c.flowSensor.calibrationFactor);
                Pump pump = new Pump(pumpPort);

                pump.FlowSensor = flowSensor;
                flowSensor.Pump = pump;

                Containers.Add(new Container(flowSensor, pump));
            }
        }

        public void PourDrink(List<IContainer> containers, bool ice = false, bool garnish = false)
        {
            
        }

        public void AddIce()
        {
            IceHopper.AddIce();
        }

        public void AddGarnish()
        {
            GarnishDispenser.AddGarnish();
        }
    }
}
