using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using BarBot.UWP.Database;
using BarBot.UWP.IO;
using Windows.Devices.Gpio;
using System.Diagnostics;
using System.Threading.Tasks;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace BarBot.UWP.TestClient
{
    public sealed partial class TestClient : UserControl
    {
        public BarbotContext barbotDb;

        public BarbotIOController ioController;

        private List<IO.Devices.V1.Container> containers;

        //HX711 weightSensor;

        private int CALIBRATION_FACTOR = 0;
        private int MAX_WEIGHT = 100;

        private bool isOpen = false;

        GpioPin pin;

        public TestClient()
        {
            this.InitializeComponent();
        }

        public TestClient(BarbotContext db, BarbotIOController controller)
        {
            this.InitializeComponent();

            barbotDb = db;
            ioController = controller;

            containers = controller.Containers;

            Init();
        }

        public void Init()
        {
            int i = 0;
            foreach(IO.Devices.V1.Container c in containers)
            {
                Button btn = new Button();
                btn.Content = "Container " + i;
                btn.Foreground = new SolidColorBrush(Windows.UI.Colors.White);
                btn.BorderBrush = new SolidColorBrush(Windows.UI.Colors.White);
                btn.Tag = c;
                btn.Click += Btn_Click;
                ContainersPanel.Children.Add(btn);
                btn.FontSize = 35;
                i++;
            }

            //GpioPin dt = ioController.gpio.OpenPin(6);
            //GpioPin clk = ioController.gpio.OpenPin(13);
            //weightSensor = new HX711(clk, dt);
            //weightSensor.PowerOn();

            //calibrate();
        }

        private void Pin_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs args)
        {
            isOpen = args.Edge.Equals(GpioPinEdge.RisingEdge);
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            IO.Devices.V1.Container c = (IO.Devices.V1.Container)((Button)sender).Tag;
            Dictionary<IO.Devices.IContainer, double> recipe = new Dictionary<IO.Devices.IContainer, double>();
            recipe.Add(c, 1);
            
            ioController.PourDrinkSync(recipe, false, false, false);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            ioController.AddIce();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            ioController.DispenseCup();
        }

        private void ReadSensorButton_Click(object sender, RoutedEventArgs e)
        {
            ioController.AddGarnish();
        }

        private void calibrate()
        {
            /*CALIBRATION_FACTOR = weightSensor.Read();
            for(int i = 0; i < 10; i++)
            {
                CALIBRATION_FACTOR = (CALIBRATION_FACTOR + weightSensor.Read()) / 2;
            }

            Debug.WriteLine("Calibration factor set to " + CALIBRATION_FACTOR / 1000 + " grams.");*/
        }
    }
}
