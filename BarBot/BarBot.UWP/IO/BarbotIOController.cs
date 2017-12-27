using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using System.Collections.Concurrent;
using BarBot.UWP.IO.Devices;
using BarBot.UWP.IO.Devices.V1;
using Windows.Devices.I2c;
using System.Diagnostics;
using static BarBot.Core.Constants;

namespace BarBot.UWP.IO
{
    /// <summary>
    /// V1 GPIO Controller implementation
    /// </summary>
    public class BarbotIOController : IIOController
    {
        public bool Initialized { get; set; }

        public List<Container> Containers { get; set; }

        public List<Pump> Pumps { get; set; }

        public IceHopper IceHopper { get; set; }

        public GarnishDispenser GarnishDispenser { get; set; }

        public CupDispenser CupDispenser { get; set; }

        public GpioController gpio;

        public int CupCount = 45;

        private MCP23017 mcp1;

        private MCP23017 mcp2;

        private MCP3008 mcp3008 = new MCP3008();

        private const int UDDER_VALVE_FLUSH_TIME = 2; // Hold the valve open for 2 seconds after all pumps are finished

        private IOPort ledPort;

        public BarbotIOController(List<Database.Container> containers, 
            Database.IceHopper iceHopper, 
            Database.GarnishDispenser garnishDispenser, 
            Database.CupDispenser cupDispenser,
            List<Database.Pump> pumps)
        {
            Debug.WriteLine("Initializing IO controller");

            // Initialize gpio controller
            gpio = GpioController.GetDefault();

            // Initialize i2c controllers
            mcp1 = new MCP23017(1);
            mcp2 = new MCP23017(2);

            // Initialize I2C Controllers
            initI2C();

            // Connect to MCP3008 analog to digital converter (for force sensor and temperature sensor)
            mcp3008.connect();

            // Create ice hopper
            GpioPin AugerPin = gpio.OpenPin(iceHopper.fsr.address);
            IOPort AugerIOPort = new IOPort(AugerPin);
            IceHopper = new IceHopper(createIOPort(iceHopper.stepper1), createIOPort(iceHopper.stepper2), createIOPort(iceHopper.stepper3), createIOPort(iceHopper.stepper4), AugerIOPort, mcp3008);

            Debug.WriteLine("Auger motor is running on pin " + iceHopper.fsr.address + " board type " + iceHopper.fsr.type);

            // Create garnish dispenser
            GarnishDispenser = new GarnishDispenser(createIOPort(garnishDispenser.stepper1), createIOPort(garnishDispenser.stepper2), createIOPort(garnishDispenser.stepper3), createIOPort(garnishDispenser.stepper4));

            // Create cup dispenser
            CupDispenser = new CupDispenser(createIOPort(cupDispenser.stepper1), createIOPort(cupDispenser.stepper2), createIOPort(cupDispenser.stepper3),createIOPort(cupDispenser.stepper4), mcp3008);

            Pumps = new List<Pump>();
            foreach(Database.Pump pump in pumps)
            {
                Pump p = new Pump(createIOPort(pump.ioPort));
                p.IOPort.Name = pump.ioPort.name;
                Pumps.Add(p);
            }

            Containers = new List<Container>();
            foreach(Database.Container c in containers)
            {
                if (c.flowSensor != null && c.pump != null)
                {
                    // Create IO Port for the pump
                    IIOPort pumpPort = createIOPort(c.pump.ioPort);

                    // Create the flow sensor and pump
                    FlowSensor flowSensor = new FlowSensor(null, c.flowSensor.calibrationFactor);
                    Pump pump = new Pump(pumpPort);
                    pump.IOPort.Name = c.pump.ioPort.name;
                    pump.FlowSensor = flowSensor;
                    flowSensor.Pump = pump;

                    // Create the new container
                    Containers.Add(new Container(flowSensor, pump, c.ingredientId));
                }
            }

            // Initialize LED
            GpioPin ioPort = gpio.OpenPin(21);
            ledPort = new IOPort(ioPort);
        }

        public async void initI2C()
        {
            await mcp1.Init();

            Debug.WriteLine("Initialized MCP23017 1");

            await mcp2.Init();

            Debug.WriteLine("Initialized MCP23017 2");

            Initialized = true;
        }

        public void PourDrinkSync(Dictionary<IContainer, double> ingredients, bool ice = false, GarnishType garnish = GarnishType.NONE, bool cup = true)
        {
            // Turn on LED
            LEDOn();

            // Add cup and decrement cup count
            if (cup)
            {
                DispenseCup();

                CupCount--;
            }

            // Add ice
            if (ice)
            {
                AddIce();
            }

            // Add garnish
            AddGarnish(garnish);

            // Open the udder valve -- using it as a "pump" type because the IO logic is the same
            Pump udder = Pumps.Where(x => x.IOPort.Name.Equals("udder valve")).First();
            udder.StartPump();

            // Start pouring each ingredient
            for (int i = 0; i < ingredients.Count; i++)
            {
                // Get the container and the amount to pour
                Container container = ingredients.ElementAt(i).Key as Container;
                double amount = ingredients.ElementAt(i).Value;

                // Start pouring the ingredient
                container.Pump.StartPump();

                // Wait for it to finish, shutting it down if the sensor times out
                Timeout((long)(TimeSpan.TicksPerSecond * (amount * container.FlowSensor.CalibrationFactor)), container.Pump);
            }

            // Wait 5 seconds for the udder to clear
            long start = DateTime.Now.Ticks;
            while (true)
            {
                if (DateTime.Now.Ticks - start > TimeSpan.TicksPerSecond * UDDER_VALVE_FLUSH_TIME)
                {
                    // Stop the udder
                    udder.StopPump();
                    break;
                }
            }

            // Turn LED off
            LEDOff();
        }

        private void Timeout(long ticks, Pump pump)
        {
            long start = DateTime.Now.Ticks;
            while(true)
            {
                if(DateTime.Now.Ticks - start > ticks)
                {
                    Debug.WriteLine(string.Format("stopping {0} after {1} seconds...", pump.IOPort.Name, ticks / 10000000.0));
                    pump.StopPump();
                    return;
                }
            }
        }

        public void AddIce()
        {
            IceHopper.AddIce();
        }

        public void AddGarnish(GarnishType garnishType)
        {
            switch (garnishType) {
                case GarnishType.GARNISH1:
                    GarnishDispenser.AddGarnish(1);
                    break;
                case GarnishType.GARNISH2:
                    GarnishDispenser.AddGarnish(2);
                    break;
                case GarnishType.BOTH:
                    GarnishDispenser.AddGarnish(1);
                    GarnishDispenser.AddGarnish(2);
                    break;
            }
        }

        public void DispenseCup()
        {
            CupDispenser.DispenseCup();
        }

        public void LEDOn()
        {
            ledPort.write(GpioPinValue.High);
        }

        public void LEDOff()
        {
            ledPort.write(GpioPinValue.Low);
        }

        private IIOPort createIOPort(Database.IOPort IOPort, GpioPinDriveMode driveMode = GpioPinDriveMode.Output)
        {
            if(IOPort.type == 1)
            {
                try
                {
                    Debug.WriteLine("Creating IO port for " + IOPort.name + " on RPi pin " + IOPort.address);
                    GpioPin pumpPin = gpio.OpenPin(IOPort.address);
                    return new IOPort(pumpPin, driveMode);
                }
                catch (Exception ignored)
                {
                    return null;
                }
            }
            else if(IOPort.type == 2)
            {
                Debug.WriteLine("Creating IO port for " + IOPort.name + " on extender 1 pin " + IOPort.address);
                return new I2CPort(mcp1, IOPort.address);
            }
            else if (IOPort.type == 3)
            {
                Debug.WriteLine("Creating IO port for " + IOPort.name + " on extender 2 pin " + IOPort.address);
                return new I2CPort(mcp2, IOPort.address);
            }

            return null;
        }
    }
}
