using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BarBot.UWP.Migrations
{
    public partial class CreateBarbotDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BarbotConfigs",
                columns: table => new
                {
                    barbotConfigId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    apiEndpoint = table.Column<string>(nullable: true),
                    barbotId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BarbotConfigs", x => x.barbotConfigId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BarbotConfigs");
        }
    }
}
