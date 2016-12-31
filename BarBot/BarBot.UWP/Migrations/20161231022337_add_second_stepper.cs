using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BarBot.UWP.Migrations
{
    public partial class add_second_stepper : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "reedSwitchioPortId",
                table: "IceHoppers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "stepper5ioPortId",
                table: "IceHoppers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "stepper6ioPortId",
                table: "IceHoppers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "stepper7ioPortId",
                table: "IceHoppers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "stepper8ioPortId",
                table: "IceHoppers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IceHoppers_reedSwitchioPortId",
                table: "IceHoppers",
                column: "reedSwitchioPortId");

            migrationBuilder.CreateIndex(
                name: "IX_IceHoppers_stepper5ioPortId",
                table: "IceHoppers",
                column: "stepper5ioPortId");

            migrationBuilder.CreateIndex(
                name: "IX_IceHoppers_stepper6ioPortId",
                table: "IceHoppers",
                column: "stepper6ioPortId");

            migrationBuilder.CreateIndex(
                name: "IX_IceHoppers_stepper7ioPortId",
                table: "IceHoppers",
                column: "stepper7ioPortId");

            migrationBuilder.CreateIndex(
                name: "IX_IceHoppers_stepper8ioPortId",
                table: "IceHoppers",
                column: "stepper8ioPortId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_IceHoppers_reedSwitchioPortId",
                table: "IceHoppers");

            migrationBuilder.DropIndex(
                name: "IX_IceHoppers_stepper5ioPortId",
                table: "IceHoppers");

            migrationBuilder.DropIndex(
                name: "IX_IceHoppers_stepper6ioPortId",
                table: "IceHoppers");

            migrationBuilder.DropIndex(
                name: "IX_IceHoppers_stepper7ioPortId",
                table: "IceHoppers");

            migrationBuilder.DropIndex(
                name: "IX_IceHoppers_stepper8ioPortId",
                table: "IceHoppers");

            migrationBuilder.DropColumn(
                name: "reedSwitchioPortId",
                table: "IceHoppers");

            migrationBuilder.DropColumn(
                name: "stepper5ioPortId",
                table: "IceHoppers");

            migrationBuilder.DropColumn(
                name: "stepper6ioPortId",
                table: "IceHoppers");

            migrationBuilder.DropColumn(
                name: "stepper7ioPortId",
                table: "IceHoppers");

            migrationBuilder.DropColumn(
                name: "stepper8ioPortId",
                table: "IceHoppers");
        }
    }
}
