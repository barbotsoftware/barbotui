using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BarBot.UWP.Migrations
{
    public partial class AddDrinkOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DrinkOrders",
                columns: table => new
                {
                    drinkOrderId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    drinkOrderUID = table.Column<string>(nullable: true),
                    garnish = table.Column<bool>(nullable: false),
                    ice = table.Column<bool>(nullable: false),
                    recipeId = table.Column<string>(nullable: true),
                    recipeName = table.Column<string>(nullable: true),
                    timestamp = table.Column<string>(nullable: true),
                    userId = table.Column<string>(nullable: true),
                    userName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrinkOrders", x => x.drinkOrderId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DrinkOrders");
        }
    }
}
