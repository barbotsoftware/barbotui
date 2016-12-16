using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BarBot.UWP.Database
{
    /// <summary>
    /// Barbot sqlite database configuration
    /// </summary>
    public class BarbotContext : DbContext
    {
        public DbSet<BarbotConfig> BarbotConfigs { get; set; }

        public DbSet<Container> Containers { get; set; }

        public DbSet<Pump> Pumps { get; set; }

        public DbSet<FlowSensor> FlowSensors { get; set; }

        public DbSet<IOPort> IOPorts { get; set; }

        public DbSet<IceHopper> IceHoppers { get; set; }

        public DbSet<GarnishDispenser> GarnishDispensers { get; set; }

        public DbSet<CupDispenser> CupDispensers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=barbot.db");
        }
    }
}
