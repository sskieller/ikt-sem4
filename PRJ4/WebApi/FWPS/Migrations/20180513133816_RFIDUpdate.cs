using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace FWPS.Migrations
{
    public partial class RFIDUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Rfid",
                table: "LoginItems",
                newName: "RfidId2");

            migrationBuilder.AddColumn<string>(
                name: "RfidId1",
                table: "LoginItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RfidId1",
                table: "LoginItems");

            migrationBuilder.RenameColumn(
                name: "RfidId2",
                table: "LoginItems",
                newName: "Rfid");
        }
    }
}
