using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Storage.Streams;

namespace BarBot.UWP.Bluetooth
{
    /// <summary>
    /// Bluetooth low-energy broadcaster. Advertises company ID and barbot ID. 
    /// </summary>
    public class BLEPublisher
    {
        private BluetoothLEAdvertisementPublisher publisher;

        public BLEPublisher(string barbotID)
        {
            publisher = new BluetoothLEAdvertisementPublisher();

            var manufacturerData = new BluetoothLEManufacturerData();

            manufacturerData.CompanyId = 0x45;

            var writer = new DataWriter();
            UInt32 uuidData = Convert.ToUInt32(barbotID, 16);
            writer.WriteUInt32(uuidData);

            manufacturerData.Data = writer.DetachBuffer();

            publisher.Advertisement.ManufacturerData.Add(manufacturerData);
        }

        public void Start()
        {
            publisher.Start();
        }

        public void Stop()
        {
            publisher.Stop();
        }
    }
}
