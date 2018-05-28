using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace FWPS.Migrations
{
    public partial class PoombaTimeandIsOn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOn",
                table: "PoombaItems",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "SleepTime",
                table: "PoombaItems",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "WakeUpTime",
                table: "PoombaItems",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOn",
                table: "PoombaItems");

            migrationBuilder.DropColumn(
                name: "SleepTime",
                table: "PoombaItems");

            migrationBuilder.DropColumn(
                name: "WakeUpTime",
                table: "PoombaItems");
        }
    }
}
