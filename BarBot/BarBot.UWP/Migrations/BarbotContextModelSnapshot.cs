using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using BarBot.UWP.Database;

namespace BarBot.UWP.Migrations
{
    [DbContext(typeof(BarbotContext))]
    partial class BarbotContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752");

            modelBuilder.Entity("BarBot.UWP.Database.BarbotConfig", b =>
                {
                    b.Property<int>("barbotConfigId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("apiEndpoint");

                    b.Property<string>("barbotId");

                    b.HasKey("barbotConfigId");

                    b.ToTable("BarbotConfigs");
                });
        }
    }
}
