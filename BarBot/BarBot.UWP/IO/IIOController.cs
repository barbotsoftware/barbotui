using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarBot.UWP.IO.Devices;
using static BarBot.Core.Constants;

namespace BarBot.UWP.IO
{
    interface IIOController
    {
        void PourDrinkSync(Dictionary<IContainer, double> ingredients, bool ice = false, GarnishType garnish = GarnishType.NONE, bool cup = false);
        void AddIce();
        void AddGarnish(GarnishType garnishType);
    }
}
