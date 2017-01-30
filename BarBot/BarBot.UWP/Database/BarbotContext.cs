using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace BarBot.UWP.Database
{
    /// <summary>
    /// Barbot sqlite database configuration
    /// </summary>
    public class BarbotContext : DbContext, INotifyPropertyChanged
    {
        public DbSet<BarbotConfig> BarbotConfigs { get; set; }

        public DbSet<Container> Containers { get; set; }

        public DbSet<Pump> Pumps { get; set; }

        public DbSet<FlowSensor> FlowSensors { get; set; }

        public DbSet<IOPort> IOPorts { get; set; }

        public DbSet<IceHopper> IceHoppers { get; set; }

        public DbSet<GarnishDispenser> GarnishDispensers { get; set; }

        public DbSet<CupDispenser> CupDispensers { get; set; }

        public DbSet<DrinkOrder> DrinkOrders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=barbot.db");
        }

        public List<Container> getContainers()
        {
            return Containers.Include(x => x.pump.ioPort).Include(x => x.flowSensor.ioPort).ToList();
        }

        public IceHopper getIceHopper()
        {
            return IceHoppers.Include(x => x.stepper1).Include(x => x.stepper2).Include(x => x.stepper3).Include(x => x.stepper4).
                Include(x => x.stepper5).Include(x => x.stepper6).Include(x => x.stepper7).Include(x => x.stepper8).
                Include(x => x.fsr).ToList().ElementAt(0);
        }

        public GarnishDispenser getGarnishDispenser()
        {
            return GarnishDispensers.Include(x => x.stepper1).Include(x => x.stepper2).Include(x => x.stepper3).Include(x => x.stepper4).ToList().ElementAt(0);
        }

        public CupDispenser getCupDispenser()
        {
            return CupDispensers.Include(x => x.stepper1).Include(x => x.stepper2).Include(x => x.stepper3).Include(x => x.stepper4).Include(x => x.fsr).ToList().ElementAt(0);
        }

        public List<Pump> getPumps()
        {
            return Pumps.Include(x => x.ioPort).ToList();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
