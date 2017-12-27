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

        private List<IO.Devices.V1.Container> containers = new List<IO.Devices.V1.Container>();

        MCP3008 mcp3008 = new MCP3008();

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

            mcp3008.connect();
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            IO.Devices.V1.Container c = (IO.Devices.V1.Container)((Button)sender).Tag;
            Dictionary<IO.Devices.IContainer, double> recipe = new Dictionary<IO.Devices.IContainer, double>();
            recipe.Add(c, 1);
            
            ioController.PourDrinkSync(recipe, false, Core.Constants.GarnishType.NONE, false);
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
            Debug.WriteLine("MCP3008: " + mcp3008.read(0));
        }

        private void RunGarnish1Button_Click(object sender, RoutedEventArgs e)
        {
            ioController.AddGarnish(Core.Constants.GarnishType.GARNISH1);
        }

        private void RunGarnish2Button_Click(object sender, RoutedEventArgs e)
        {
            ioController.AddGarnish(Core.Constants.GarnishType.GARNISH2);
        }
    }
}
