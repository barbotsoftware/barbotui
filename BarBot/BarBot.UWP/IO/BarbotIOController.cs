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

        IceHopper IceHopper { get; set; }

        GarnishDispenser GarnishDispenser { get; set; }

        CupDispenser CupDispenser { get; set; }

        Dictionary<GpioPin, int> sensorTicks = new Dictionary<GpioPin, int>();

        Dictionary<GpioPin, int> maxTicks = new Dictionary<GpioPin, int>();

        Dictionary<GpioPin, Pump> pinPumpMapping = new Dictionary<GpioPin, Pump>();

        public GpioController gpio;

        MCP23017 mcp1;

        MCP23017 mcp2;

        private const int TIMEOUT = 5;

        private const int MIXER_PUMP_FLUSH_TIME = 8000;

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

            // Create ice hopper
            GpioPin reedSwitch = gpio.OpenPin(iceHopper.reedSwitch.address);
            IOPort reedSwitchIOPort = new IOPort(reedSwitch, GpioPinDriveMode.InputPullDown);
            IceHopper = new IceHopper(createIOPort(iceHopper.stepper1), createIOPort(iceHopper.stepper2), createIOPort(iceHopper.stepper3), createIOPort(iceHopper.stepper4),
                createIOPort(iceHopper.stepper5), createIOPort(iceHopper.stepper6), createIOPort(iceHopper.stepper7), createIOPort(iceHopper.stepper8),
                reedSwitchIOPort);

            // Create garnish dispenser
            GarnishDispenser = new GarnishDispenser(createIOPort(garnishDispenser.stepper1), createIOPort(garnishDispenser.stepper2), createIOPort(garnishDispenser.stepper3), createIOPort(garnishDispenser.stepper4));

            // Create cup dispenser
            CupDispenser = new CupDispenser(createIOPort(cupDispenser.stepper1), createIOPort(cupDispenser.stepper2), createIOPort(cupDispenser.stepper3),createIOPort(cupDispenser.stepper4));

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

            // Initialize I2C Controllers
            initI2C();
        }

        public async void initI2C()
        {
            await mcp1.Init();

            Debug.WriteLine("Initialized MCP23017 1");

            await mcp2.Init();

            Debug.WriteLine("Initialized MCP23017 2");

            Initialized = true;
        }
        
        private void Input_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs args)
        {
            int curr;
            int max = 0;
            bool success = sensorTicks.TryGetValue(sender, out curr) && maxTicks.TryGetValue(sender, out max);
            
            if(success)
            {
                if (curr < max)
                {
                    sensorTicks[sender] =  curr + 1;
                }
                else
                {
                    pinPumpMapping[sender].StopPump();
                    sensorTicks.Remove(sender);
                    maxTicks.Remove(sender);
                    pinPumpMapping.Remove(sender);
                }
            }
        }

        public void PourDrink(Dictionary<IContainer, double> ingredients, bool ice = false, bool garnish = false, bool cup = false)
        {
            sensorTicks = new Dictionary<GpioPin, int>();
            maxTicks = new Dictionary<GpioPin, int>();
            pinPumpMapping = new Dictionary<GpioPin, Pump>();

            if(cup)
            {
                DispenseCup();
            }

            if(ice)
            {
                AddIce();
            }

            // Start pouring each ingredient
            for(int i = 0; i < ingredients.Count; i++)
            {
                Container container = ingredients.ElementAt(i).Key as Container;
                double amount = ingredients.ElementAt(i).Value;

                PourIngredient(container, amount);
            }

            // Wait for pumps to finish, but stop them if they run too long
            Timeout(TimeSpan.TicksPerSecond * TIMEOUT);

            // Run the flush pump
            FlushMixer();

            if(garnish)
            {
                AddGarnish();
            }
        }

        public void PourIngredient(Container container, double amount)
        {
            GpioPin pin = container.FlowSensor.IoPort.GpioPin;

            sensorTicks.Add(pin, 0);
            maxTicks.Add(pin, container.FlowSensor.CalibrationFactor * Convert.ToInt32(amount));
            pinPumpMapping.Add(pin, container.Pump);

            pin.SetDriveMode(GpioPinDriveMode.Input);
            pin.DebounceTimeout = TimeSpan.FromMilliseconds(1);
            pin.ValueChanged += Input_ValueChanged;

            container.Pump.StartPump();

            Debug.WriteLine(string.Format("Started flow sensor {0} on pin {1}", container.FlowSensor.IoPort.Name, pin.PinNumber));
        }

        private void Timeout(long ticks)
        {
            long start = DateTime.Now.Ticks;
            while(true && sensorTicks.Count > 0)
            {
                if(DateTime.Now.Ticks - start > ticks)
                {
                    Debug.WriteLine(string.Format("Some pumps timed out after {0} seconds", TIMEOUT));
                    StopAllPumps();
                    return;
                }
            }
        }

        private void StopAllPumps()
        {
            for(int i = 0; i < pinPumpMapping.Count; i++) {
                pinPumpMapping.ElementAt(i).Value.StopPump();
                pinPumpMapping.ElementAt(i).Key.ValueChanged -= Input_ValueChanged;

                Debug.WriteLine(string.Format("{0} was still running.", pinPumpMapping.ElementAt(i).Value.IOPort.Name));
            }

            pinPumpMapping = new Dictionary<GpioPin, Pump>();
            sensorTicks = new Dictionary<GpioPin, int>();
            maxTicks = new Dictionary<GpioPin, int>();
        }

        public void FlushMixer()
        {
            try
            {
                Pump pump = Pumps.Where(x => x.IOPort.Name.Equals("mixer pump")).First();

                pump.StartPump();

                Task.Delay(MIXER_PUMP_FLUSH_TIME).Wait();

                pump.StopPump();
            }
            catch (Exception e)
            {
                return;
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

        private IIOPort createIOPort(Database.IOPort IOPort, GpioPinDriveMode driveMode = GpioPinDriveMode.Output)
        {
            if(IOPort.type == 1)
            {
                try
                {
                    GpioPin pumpPin = gpio.OpenPin(IOPort.address);
                    return new IOPort(pumpPin, driveMode);
                }
                catch (Exception e)
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
