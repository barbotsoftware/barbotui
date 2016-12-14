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
        void PourDrink(List<IContainer> ingredients, bool ice = false, bool garnish = false);
        void AddIce();
        void AddGarnish();
    }
}
