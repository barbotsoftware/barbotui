using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BarBot.UWP.Database
{
    public class BarbotContext : DbContext
    {
        public DbSet<BarbotConfig> BarbotConfigs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=barbot.db");
        }
    }
}
