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

        IceHopper IceHopper { get; set; }

        GarnishDispenser GarnishDispenser { get; set; }

        CupDispenser CupDispenser { get; set; }

        Dictionary<GpioPin, int> sensorTicks = new Dictionary<GpioPin, int>();

        Dictionary<GpioPin, int> maxTicks = new Dictionary<GpioPin, int>();

        Dictionary<GpioPin, Pump> pinPumpMapping = new Dictionary<GpioPin, Pump>();

        GpioController gpio;

        MCP23017 mcp1;

        MCP23017 mcp2;

        public BarbotIOController(List<Database.Container> containers, Database.IceHopper iceHopper, Database.GarnishDispenser garnishDispenser, Database.CupDispenser cupDispenser)
        {
            // Initialize gpio controller
            gpio = GpioController.GetDefault();

            // Initialize i2c controllers
            mcp1 = new MCP23017();
            mcp2 = new MCP23017(2);

            // Create ice hopper
            IceHopper = new IceHopper(createIOPort(iceHopper.stepper1), createIOPort(iceHopper.stepper2), createIOPort(iceHopper.stepper3), createIOPort(iceHopper.stepper4));

            // Create garnish dispenser
            GarnishDispenser = new GarnishDispenser(createIOPort(garnishDispenser.stepper1), createIOPort(garnishDispenser.stepper2), createIOPort(garnishDispenser.stepper3), createIOPort(garnishDispenser.stepper4));

            // Create cup dispenser
            CupDispenser = new CupDispenser(createIOPort(cupDispenser.stepper1), createIOPort(cupDispenser.stepper2), createIOPort(cupDispenser.stepper3),createIOPort(cupDispenser.stepper4));

            Containers = new List<Container>();
            foreach(Database.Container c in containers)
            {
                // Create IO Port for the pump
                IIOPort pumpPort = createIOPort(c.pump.ioPort);

                // Create IO port for the sensor. Sensors will always be on the main GPIO board (for now)
                IOPort sensorPort = createIOPort(c.flowSensor.ioPort, GpioPinDriveMode.InputPullDown) as IOPort;

                // Create the flow sensor and pump
                FlowSensor flowSensor = new FlowSensor(sensorPort, c.flowSensor.calibrationFactor);
                Pump pump = new Pump(pumpPort);
                pump.FlowSensor = flowSensor;
                flowSensor.Pump = pump;

                // Create the new container
                Containers.Add(new Container(flowSensor, pump));
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

        public void PourDrink(Dictionary<IContainer, int> ingredients, bool ice = false, bool garnish = false, bool cup = false)
        {
            if(cup)
            {
                DispenseCup();
            }

            if(ice)
            {
                AddIce();
            }

            for(int i = 0; i < ingredients.Count; i++)
            {
                Container container = ingredients.ElementAt(i).Key as Container;
                int amount = ingredients.ElementAt(i).Value;

                GpioPin pin = container.FlowSensor.IoPort.GpioPin;

                sensorTicks.Add(pin, 0);
                maxTicks.Add(pin, container.FlowSensor.CalibrationFactor * amount);
                pinPumpMapping.Add(pin, container.Pump);

                pin.SetDriveMode(GpioPinDriveMode.Input);
                pin.DebounceTimeout = TimeSpan.FromMilliseconds(1);
                pin.ValueChanged += Input_ValueChanged;

                container.Pump.StartPump();
            }

            if(garnish)
            {
                AddGarnish();
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
