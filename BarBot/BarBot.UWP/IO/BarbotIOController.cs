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

        private const int MIXER_PUMP_FLUSH_TIME = 8000;

        private IOPort ledPort;

        public BarbotIOController(List<Database.Container> containers, 
            Database.IceHopper iceHopper, 
            Database.GarnishDispenser garnishDispenser, 
            Database.CupDispenser cupDispenser,
            List<Database.Pump> pumps)
        {
            // Initialize gpio controller
            gpio = GpioController.GetDefault();

            // Initialize i2c controllers
            mcp1 = new MCP23017();
            mcp2 = new MCP23017(2);

            // Initialize I2C Controllers
            initI2C();

            // Create ice hopper
            GpioPin FSR2Pin = gpio.OpenPin(iceHopper.fsr.address);
            IOPort FSR2 = new IOPort(FSR2Pin, GpioPinDriveMode.InputPullDown);
            IceHopper = new IceHopper(createIOPort(iceHopper.stepper1), createIOPort(iceHopper.stepper2), createIOPort(iceHopper.stepper3), createIOPort(iceHopper.stepper4), FSR2);

            // Create garnish dispenser
            GarnishDispenser = new GarnishDispenser(createIOPort(garnishDispenser.stepper1), createIOPort(garnishDispenser.stepper2), createIOPort(garnishDispenser.stepper3), createIOPort(garnishDispenser.stepper4));

            // Create cup dispenser
            GpioPin FSR1Pin = gpio.OpenPin(cupDispenser.fsr.address);
            IOPort FSR1 = new IOPort(FSR1Pin, GpioPinDriveMode.InputPullDown);
            CupDispenser = new CupDispenser(createIOPort(cupDispenser.stepper1), createIOPort(cupDispenser.stepper2), createIOPort(cupDispenser.stepper3),createIOPort(cupDispenser.stepper4), FSR1);

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

                    // Create IO port for the sensor. Sensors will always be on the main GPIO board (for now)
                    IOPort sensorPort = createIOPort(c.flowSensor.ioPort, GpioPinDriveMode.InputPullDown) as IOPort;

                    // Create the flow sensor and pump
                    FlowSensor flowSensor = new FlowSensor(sensorPort, c.flowSensor.calibrationFactor);
                    Pump pump = new Pump(pumpPort);
                    pump.IOPort.Name = c.pump.ioPort.name;
                    flowSensor.IoPort.Name = c.flowSensor.ioPort.name;
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

        public void PourDrinkSync(Dictionary<IContainer, double> ingredients, bool ice = false, bool garnish = false, bool cup = true)
        {
            // Turn on LED
            LEDOn();

            if (cup)
            {
                DispenseCup();

                CupCount--;
            }

            if (ice)
            {
                AddIce();
            }

            if(garnish)
            {
                AddGarnish();
            }

            // Start the udder pump
            Pump pump = Pumps.Where(x => x.IOPort.Name.Equals("mixer pump")).First();
            pump.StartPump();

            // Start pouring each ingredient
            for (int i = 0; i < ingredients.Count; i++)
            {
                // Get the container and the amount to pour
                Container container = ingredients.ElementAt(i).Key as Container;
                double amount = ingredients.ElementAt(i).Value;

                // Start pouring the ingredient
                container.Pump.StartPump();

                Debug.WriteLine(string.Format("Started pump {0}", container.Pump.IOPort.Name));

                // Wait for it to finish, shutting it down if the sensor times out
                Timeout((long)(TimeSpan.TicksPerSecond * (amount * container.FlowSensor.CalibrationFactor)), container.Pump);
            }

            // Wait 5 seconds for the udder to clear
            long start = DateTime.Now.Ticks;
            while (true)
            {
                if (DateTime.Now.Ticks - start > TimeSpan.TicksPerSecond * MIXER_PUMP_FLUSH_TIME)
                {
                    // Stop the udder
                    pump.StopPump();
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
                    pump.StopPump();
                    Debug.WriteLine(string.Format("stopping pump after {0} seconds", ticks / 10000000.0));
                    return;
                }
            }
        }

        public void AddIce()
        {
            IceHopper.AddIce();
        }

        public void AddGarnish()
        {
            GarnishDispenser.AddGarnish();
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
                return new I2CPort(mcp1, IOPort.address);
            }
            else if (IOPort.type == 3)
            {
                return new I2CPort(mcp2, IOPort.address);
            }

            return null;
        }
    }
}
