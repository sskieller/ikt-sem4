using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace FWPS.Migrations
{
    public partial class addedChecksumAndIDToSnapBoxItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Checksum",
                table: "SnapBoxItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SnapBoxId",
                table: "SnapBoxItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Checksum",
                table: "SnapBoxItems");

            migrationBuilder.DropColumn(
                name: "SnapBoxId",
                table: "SnapBoxItems");
        }
    }
}
