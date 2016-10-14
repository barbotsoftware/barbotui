using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarBot.Model
{
    class IODevice : JsonModelObject
    {
        private int gpio_port;
        private int io_device_type_id;
        private string name;

        public int Gpio_port
        {
            get
            {
                return gpio_port;
            }

            set
            {
                gpio_port = value;
                OnPropertyChanged("Gpio_port");
            }
        }

        public int Io_device_type_id
        {
            get
            {
                return io_device_type_id;
            }

            set
            {
                io_device_type_id = value;
                OnPropertyChanged("Io_device_type_id");
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public IODevice() { }

        public IODevice(int gpio_port, int io_device_type_id, string name)
        {
            Gpio_port = gpio_port;
            Io_device_type_id = io_device_type_id;
            Name = name;
        }

        public IODevice(string json)
        {
            IODevice i = (IODevice)parseJSON(json, typeof(IODevice));
            Gpio_port = i.gpio_port;
            Io_device_type_id = i.Io_device_type_id;
            Name = i.Name;
        }
    }
}
