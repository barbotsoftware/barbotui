using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarBot.UWP.IO.Devices;

namespace BarBot.UWP.IO
{
    interface IIOController
    {
        void PourDrinkSync(Dictionary<IContainer, double> ingredients, bool ice = false, int garnish = 0, bool cup = false);
        void AddIce();
        void AddGarnish(int garnishType = 0);
    }
}
