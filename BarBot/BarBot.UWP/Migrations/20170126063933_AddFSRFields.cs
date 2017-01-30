using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BarBot.UWP.Migrations
{
    public partial class AddFSRFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "fsrioPortId",
                table: "IceHoppers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "fsrioPortId",
                table: "CupDispensers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IceHoppers_fsrioPortId",
                table: "IceHoppers",
                column: "fsrioPortId");

            migrationBuilder.CreateIndex(
                name: "IX_CupDispensers_fsrioPortId",
                table: "CupDispensers",
                column: "fsrioPortId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_IceHoppers_fsrioPortId",
                table: "IceHoppers");

            migrationBuilder.DropIndex(
                name: "IX_CupDispensers_fsrioPortId",
                table: "CupDispensers");

            migrationBuilder.DropColumn(
                name: "fsrioPortId",
                table: "IceHoppers");

            migrationBuilder.DropColumn(
                name: "fsrioPortId",
                table: "CupDispensers");
        }
    }
}
