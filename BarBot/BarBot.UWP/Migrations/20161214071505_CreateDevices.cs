using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BarBot.UWP.Migrations
{
    public partial class CreateDevices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Containers",
                columns: table => new
                {
                    containerId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ingredientId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Containers", x => x.containerId);
                });

            migrationBuilder.CreateTable(
                name: "IOPorts",
                columns: table => new
                {
                    ioPortId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    address = table.Column<int>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IOPorts", x => x.ioPortId);
                });

            migrationBuilder.CreateTable(
                name: "CupDispensers",
                columns: table => new
                {
                    cupDispenserId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    stepper1ioPortId = table.Column<int>(nullable: true),
                    stepper2ioPortId = table.Column<int>(nullable: true),
                    stepper3ioPortId = table.Column<int>(nullable: true),
                    stepper4ioPortId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CupDispensers", x => x.cupDispenserId);
                    table.ForeignKey(
                        name: "FK_CupDispensers_IOPorts_stepper1ioPortId",
                        column: x => x.stepper1ioPortId,
                        principalTable: "IOPorts",
                        principalColumn: "ioPortId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CupDispensers_IOPorts_stepper2ioPortId",
                        column: x => x.stepper2ioPortId,
                        principalTable: "IOPorts",
                        principalColumn: "ioPortId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CupDispensers_IOPorts_stepper3ioPortId",
                        column: x => x.stepper3ioPortId,
                        principalTable: "IOPorts",
                        principalColumn: "ioPortId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CupDispensers_IOPorts_stepper4ioPortId",
                        column: x => x.stepper4ioPortId,
                        principalTable: "IOPorts",
                        principalColumn: "ioPortId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FlowSensors",
                columns: table => new
                {
                    flowSensorId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    calibrationFactor = table.Column<int>(nullable: false),
                    containerId = table.Column<int>(nullable: false),
                    ioPortId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlowSensors", x => x.flowSensorId);
                    table.ForeignKey(
                        name: "FK_FlowSensors_Containers_containerId",
                        column: x => x.containerId,
                        principalTable: "Containers",
                        principalColumn: "containerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FlowSensors_IOPorts_ioPortId",
                        column: x => x.ioPortId,
                        principalTable: "IOPorts",
                        principalColumn: "ioPortId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GarnishDispensers",
                columns: table => new
                {
                    garnishDispenserId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    stepper1ioPortId = table.Column<int>(nullable: true),
                    stepper2ioPortId = table.Column<int>(nullable: true),
                    stepper3ioPortId = table.Column<int>(nullable: true),
                    stepper4ioPortId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarnishDispensers", x => x.garnishDispenserId);
                    table.ForeignKey(
                        name: "FK_GarnishDispensers_IOPorts_stepper1ioPortId",
                        column: x => x.stepper1ioPortId,
                        principalTable: "IOPorts",
                        principalColumn: "ioPortId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GarnishDispensers_IOPorts_stepper2ioPortId",
                        column: x => x.stepper2ioPortId,
                        principalTable: "IOPorts",
                        principalColumn: "ioPortId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GarnishDispensers_IOPorts_stepper3ioPortId",
                        column: x => x.stepper3ioPortId,
                        principalTable: "IOPorts",
                        principalColumn: "ioPortId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GarnishDispensers_IOPorts_stepper4ioPortId",
                        column: x => x.stepper4ioPortId,
                        principalTable: "IOPorts",
                        principalColumn: "ioPortId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IceHoppers",
                columns: table => new
                {
                    iceHopperId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    stepper1ioPortId = table.Column<int>(nullable: true),
                    stepper2ioPortId = table.Column<int>(nullable: true),
                    stepper3ioPortId = table.Column<int>(nullable: true),
                    stepper4ioPortId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IceHoppers", x => x.iceHopperId);
                    table.ForeignKey(
                        name: "FK_IceHoppers_IOPorts_stepper1ioPortId",
                        column: x => x.stepper1ioPortId,
                        principalTable: "IOPorts",
                        principalColumn: "ioPortId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IceHoppers_IOPorts_stepper2ioPortId",
                        column: x => x.stepper2ioPortId,
                        principalTable: "IOPorts",
                        principalColumn: "ioPortId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IceHoppers_IOPorts_stepper3ioPortId",
                        column: x => x.stepper3ioPortId,
                        principalTable: "IOPorts",
                        principalColumn: "ioPortId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IceHoppers_IOPorts_stepper4ioPortId",
                        column: x => x.stepper4ioPortId,
                        principalTable: "IOPorts",
                        principalColumn: "ioPortId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pumps",
                columns: table => new
                {
                    pumpId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    containerId = table.Column<int>(nullable: false),
                    ioPortId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pumps", x => x.pumpId);
                    table.ForeignKey(
                        name: "FK_Pumps_Containers_containerId",
                        column: x => x.containerId,
                        principalTable: "Containers",
                        principalColumn: "containerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pumps_IOPorts_ioPortId",
                        column: x => x.ioPortId,
                        principalTable: "IOPorts",
                        principalColumn: "ioPortId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CupDispensers_stepper1ioPortId",
                table: "CupDispensers",
                column: "stepper1ioPortId");

            migrationBuilder.CreateIndex(
                name: "IX_CupDispensers_stepper2ioPortId",
                table: "CupDispensers",
                column: "stepper2ioPortId");

            migrationBuilder.CreateIndex(
                name: "IX_CupDispensers_stepper3ioPortId",
                table: "CupDispensers",
                column: "stepper3ioPortId");

            migrationBuilder.CreateIndex(
                name: "IX_CupDispensers_stepper4ioPortId",
                table: "CupDispensers",
                column: "stepper4ioPortId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowSensors_containerId",
                table: "FlowSensors",
                column: "containerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FlowSensors_ioPortId",
                table: "FlowSensors",
                column: "ioPortId");

            migrationBuilder.CreateIndex(
                name: "IX_GarnishDispensers_stepper1ioPortId",
                table: "GarnishDispensers",
                column: "stepper1ioPortId");

            migrationBuilder.CreateIndex(
                name: "IX_GarnishDispensers_stepper2ioPortId",
                table: "GarnishDispensers",
                column: "stepper2ioPortId");

            migrationBuilder.CreateIndex(
                name: "IX_GarnishDispensers_stepper3ioPortId",
                table: "GarnishDispensers",
                column: "stepper3ioPortId");

            migrationBuilder.CreateIndex(
                name: "IX_GarnishDispensers_stepper4ioPortId",
                table: "GarnishDispensers",
                column: "stepper4ioPortId");

            migrationBuilder.CreateIndex(
                name: "IX_IceHoppers_stepper1ioPortId",
                table: "IceHoppers",
                column: "stepper1ioPortId");

            migrationBuilder.CreateIndex(
                name: "IX_IceHoppers_stepper2ioPortId",
                table: "IceHoppers",
                column: "stepper2ioPortId");

            migrationBuilder.CreateIndex(
                name: "IX_IceHoppers_stepper3ioPortId",
                table: "IceHoppers",
                column: "stepper3ioPortId");

            migrationBuilder.CreateIndex(
                name: "IX_IceHoppers_stepper4ioPortId",
                table: "IceHoppers",
                column: "stepper4ioPortId");

            migrationBuilder.CreateIndex(
                name: "IX_Pumps_containerId",
                table: "Pumps",
                column: "containerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pumps_ioPortId",
                table: "Pumps",
                column: "ioPortId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CupDispensers");

            migrationBuilder.DropTable(
                name: "FlowSensors");

            migrationBuilder.DropTable(
                name: "GarnishDispensers");

            migrationBuilder.DropTable(
                name: "IceHoppers");

            migrationBuilder.DropTable(
                name: "Pumps");

            migrationBuilder.DropTable(
                name: "Containers");

            migrationBuilder.DropTable(
                name: "IOPorts");
        }
    }
}
