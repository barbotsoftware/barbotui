using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using BarBot.UWP.Database;

namespace BarBot.UWP.Migrations
{
    [DbContext(typeof(BarbotContext))]
    [Migration("20170126063933_AddFSRFields")]
    partial class AddFSRFields
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

            modelBuilder.Entity("BarBot.UWP.Database.Container", b =>
                {
                    b.Property<int>("containerId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ingredientId");

                    b.HasKey("containerId");

                    b.ToTable("Containers");
                });

            modelBuilder.Entity("BarBot.UWP.Database.CupDispenser", b =>
                {
                    b.Property<int>("cupDispenserId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("fsrioPortId");

                    b.Property<int?>("stepper1ioPortId");

                    b.Property<int?>("stepper2ioPortId");

                    b.Property<int?>("stepper3ioPortId");

                    b.Property<int?>("stepper4ioPortId");

                    b.HasKey("cupDispenserId");

                    b.HasIndex("fsrioPortId");

                    b.HasIndex("stepper1ioPortId");

                    b.HasIndex("stepper2ioPortId");

                    b.HasIndex("stepper3ioPortId");

                    b.HasIndex("stepper4ioPortId");

                    b.ToTable("CupDispensers");
                });

            modelBuilder.Entity("BarBot.UWP.Database.DrinkOrder", b =>
                {
                    b.Property<int>("drinkOrderId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("drinkOrderUID");

                    b.Property<bool>("garnish");

                    b.Property<bool>("ice");

                    b.Property<string>("recipeId");

                    b.Property<string>("recipeName");

                    b.Property<string>("timestamp");

                    b.Property<string>("userId");

                    b.Property<string>("userName");

                    b.HasKey("drinkOrderId");

                    b.ToTable("DrinkOrders");
                });

            modelBuilder.Entity("BarBot.UWP.Database.FlowSensor", b =>
                {
                    b.Property<int>("flowSensorId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("calibrationFactor");

                    b.Property<int>("containerId");

                    b.Property<int?>("ioPortId");

                    b.HasKey("flowSensorId");

                    b.HasIndex("containerId")
                        .IsUnique();

                    b.HasIndex("ioPortId");

                    b.ToTable("FlowSensors");
                });

            modelBuilder.Entity("BarBot.UWP.Database.GarnishDispenser", b =>
                {
                    b.Property<int>("garnishDispenserId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("stepper1ioPortId");

                    b.Property<int?>("stepper2ioPortId");

                    b.Property<int?>("stepper3ioPortId");

                    b.Property<int?>("stepper4ioPortId");

                    b.HasKey("garnishDispenserId");

                    b.HasIndex("stepper1ioPortId");

                    b.HasIndex("stepper2ioPortId");

                    b.HasIndex("stepper3ioPortId");

                    b.HasIndex("stepper4ioPortId");

                    b.ToTable("GarnishDispensers");
                });

            modelBuilder.Entity("BarBot.UWP.Database.IceHopper", b =>
                {
                    b.Property<int>("iceHopperId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("fsrioPortId");

                    b.Property<int?>("reedSwitchioPortId");

                    b.Property<int?>("stepper1ioPortId");

                    b.Property<int?>("stepper2ioPortId");

                    b.Property<int?>("stepper3ioPortId");

                    b.Property<int?>("stepper4ioPortId");

                    b.Property<int?>("stepper5ioPortId");

                    b.Property<int?>("stepper6ioPortId");

                    b.Property<int?>("stepper7ioPortId");

                    b.Property<int?>("stepper8ioPortId");

                    b.HasKey("iceHopperId");

                    b.HasIndex("fsrioPortId");

                    b.HasIndex("reedSwitchioPortId");

                    b.HasIndex("stepper1ioPortId");

                    b.HasIndex("stepper2ioPortId");

                    b.HasIndex("stepper3ioPortId");

                    b.HasIndex("stepper4ioPortId");

                    b.HasIndex("stepper5ioPortId");

                    b.HasIndex("stepper6ioPortId");

                    b.HasIndex("stepper7ioPortId");

                    b.HasIndex("stepper8ioPortId");

                    b.ToTable("IceHoppers");
                });

            modelBuilder.Entity("BarBot.UWP.Database.IOPort", b =>
                {
                    b.Property<int>("ioPortId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("address");

                    b.Property<string>("name");

                    b.Property<int>("type");

                    b.HasKey("ioPortId");

                    b.ToTable("IOPorts");
                });

            modelBuilder.Entity("BarBot.UWP.Database.Pump", b =>
                {
                    b.Property<int>("pumpId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("containerId");

                    b.Property<int?>("ioPortId");

                    b.HasKey("pumpId");

                    b.HasIndex("containerId")
                        .IsUnique();

                    b.HasIndex("ioPortId");

                    b.ToTable("Pumps");
                });

            modelBuilder.Entity("BarBot.UWP.Database.CupDispenser", b =>
                {
                    b.HasOne("BarBot.UWP.Database.IOPort", "fsr")
                        .WithMany()
                        .HasForeignKey("fsrioPortId");

                    b.HasOne("BarBot.UWP.Database.IOPort", "stepper1")
                        .WithMany()
                        .HasForeignKey("stepper1ioPortId");

                    b.HasOne("BarBot.UWP.Database.IOPort", "stepper2")
                        .WithMany()
                        .HasForeignKey("stepper2ioPortId");

                    b.HasOne("BarBot.UWP.Database.IOPort", "stepper3")
                        .WithMany()
                        .HasForeignKey("stepper3ioPortId");

                    b.HasOne("BarBot.UWP.Database.IOPort", "stepper4")
                        .WithMany()
                        .HasForeignKey("stepper4ioPortId");
                });

            modelBuilder.Entity("BarBot.UWP.Database.FlowSensor", b =>
                {
                    b.HasOne("BarBot.UWP.Database.Container", "container")
                        .WithOne("flowSensor")
                        .HasForeignKey("BarBot.UWP.Database.FlowSensor", "containerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BarBot.UWP.Database.IOPort", "ioPort")
                        .WithMany()
                        .HasForeignKey("ioPortId");
                });

            modelBuilder.Entity("BarBot.UWP.Database.GarnishDispenser", b =>
                {
                    b.HasOne("BarBot.UWP.Database.IOPort", "stepper1")
                        .WithMany()
                        .HasForeignKey("stepper1ioPortId");

                    b.HasOne("BarBot.UWP.Database.IOPort", "stepper2")
                        .WithMany()
                        .HasForeignKey("stepper2ioPortId");

                    b.HasOne("BarBot.UWP.Database.IOPort", "stepper3")
                        .WithMany()
                        .HasForeignKey("stepper3ioPortId");

                    b.HasOne("BarBot.UWP.Database.IOPort", "stepper4")
                        .WithMany()
                        .HasForeignKey("stepper4ioPortId");
                });

            modelBuilder.Entity("BarBot.UWP.Database.IceHopper", b =>
                {
                    b.HasOne("BarBot.UWP.Database.IOPort", "fsr")
                        .WithMany()
                        .HasForeignKey("fsrioPortId");

                    b.HasOne("BarBot.UWP.Database.IOPort", "reedSwitch")
                        .WithMany()
                        .HasForeignKey("reedSwitchioPortId");

                    b.HasOne("BarBot.UWP.Database.IOPort", "stepper1")
                        .WithMany()
                        .HasForeignKey("stepper1ioPortId");

                    b.HasOne("BarBot.UWP.Database.IOPort", "stepper2")
                        .WithMany()
                        .HasForeignKey("stepper2ioPortId");

                    b.HasOne("BarBot.UWP.Database.IOPort", "stepper3")
                        .WithMany()
                        .HasForeignKey("stepper3ioPortId");

                    b.HasOne("BarBot.UWP.Database.IOPort", "stepper4")
                        .WithMany()
                        .HasForeignKey("stepper4ioPortId");

                    b.HasOne("BarBot.UWP.Database.IOPort", "stepper5")
                        .WithMany()
                        .HasForeignKey("stepper5ioPortId");

                    b.HasOne("BarBot.UWP.Database.IOPort", "stepper6")
                        .WithMany()
                        .HasForeignKey("stepper6ioPortId");

                    b.HasOne("BarBot.UWP.Database.IOPort", "stepper7")
                        .WithMany()
                        .HasForeignKey("stepper7ioPortId");

                    b.HasOne("BarBot.UWP.Database.IOPort", "stepper8")
                        .WithMany()
                        .HasForeignKey("stepper8ioPortId");
                });

            modelBuilder.Entity("BarBot.UWP.Database.Pump", b =>
                {
                    b.HasOne("BarBot.UWP.Database.Container", "container")
                        .WithOne("pump")
                        .HasForeignKey("BarBot.UWP.Database.Pump", "containerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BarBot.UWP.Database.IOPort", "ioPort")
                        .WithMany()
                        .HasForeignKey("ioPortId");
                });
        }
    }
}
