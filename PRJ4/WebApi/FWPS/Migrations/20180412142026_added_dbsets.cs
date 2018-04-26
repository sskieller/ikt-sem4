using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace FWPS.Migrations
{
    public partial class added_dbsets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClimateControlItems",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Command = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    HumidityLevel = table.Column<int>(nullable: false),
                    IsHeaterOn = table.Column<bool>(nullable: false),
                    IsRun = table.Column<bool>(nullable: false),
                    IsVentilationOn = table.Column<bool>(nullable: false),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    MaxHumidity = table.Column<int>(nullable: false),
                    MaxTemperature = table.Column<int>(nullable: false),
                    MinHumidity = table.Column<int>(nullable: false),
                    MinTemperature = table.Column<int>(nullable: false),
                    TemperatureLevel = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClimateControlItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CurtainItems",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Command = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    IsRun = table.Column<bool>(nullable: false),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    LightIntensity = table.Column<int>(nullable: false),
                    MaxLightIntensity = table.Column<int>(nullable: false),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurtainItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HodoorItems",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Command = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    IsRun = table.Column<bool>(nullable: false),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    OpenStatus = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HodoorItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PoombaItems",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CleaningTime = table.Column<DateTime>(nullable: false),
                    Command = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    IsRun = table.Column<bool>(nullable: false),
                    LastModifiedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoombaItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Room",
                columns: table => new
                {
                    RoomName = table.Column<string>(nullable: false),
                    PoombaItemId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Room", x => x.RoomName);
                    table.ForeignKey(
                        name: "FK_Room_PoombaItems_PoombaItemId",
                        column: x => x.PoombaItemId,
                        principalTable: "PoombaItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Room_PoombaItemId",
                table: "Room",
                column: "PoombaItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClimateControlItems");

            migrationBuilder.DropTable(
                name: "CurtainItems");

            migrationBuilder.DropTable(
                name: "HodoorItems");

            migrationBuilder.DropTable(
                name: "Room");

            migrationBuilder.DropTable(
                name: "PoombaItems");
        }
    }
}
