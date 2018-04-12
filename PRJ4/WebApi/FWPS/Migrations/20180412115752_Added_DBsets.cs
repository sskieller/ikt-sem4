using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace FWPS.Migrations
{
    public partial class Added_DBsets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IpItems");

            migrationBuilder.DropTable(
                name: "LightItems");

            migrationBuilder.DropTable(
                name: "LoginItems");

            migrationBuilder.DropTable(
                name: "MailItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SnapBoxItems",
                table: "SnapBoxItems");

            migrationBuilder.RenameTable(
                name: "SnapBoxItems",
                newName: "ItemBases");

            migrationBuilder.AlterColumn<bool>(
                name: "MailReceived",
                table: "ItemBases",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AddColumn<string>(
                name: "Command",
                table: "ItemBases",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HumidityLevel",
                table: "ItemBases",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsHeaterOn",
                table: "ItemBases",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRun",
                table: "ItemBases",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsVentilationOn",
                table: "ItemBases",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxHumidity",
                table: "ItemBases",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxTemperature",
                table: "ItemBases",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinHumidity",
                table: "ItemBases",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinTemperature",
                table: "ItemBases",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TemperatureLevel",
                table: "ItemBases",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurtainItem_Command",
                table: "ItemBases",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CurtainItem_IsRun",
                table: "ItemBases",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LightIntensity",
                table: "ItemBases",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxLightIntensity",
                table: "ItemBases",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "ItemBases",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HodoorItem_Command",
                table: "ItemBases",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HodoorItem_IsRun",
                table: "ItemBases",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "OpenStatus",
                table: "ItemBases",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ip",
                table: "ItemBases",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "ItemBases",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LightItem_Command",
                table: "ItemBases",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsOn",
                table: "ItemBases",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "LightItem_IsRun",
                table: "ItemBases",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SleepTime",
                table: "ItemBases",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "WakeUpTime",
                table: "ItemBases",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "ItemBases",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "ItemBases",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Body",
                table: "ItemBases",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "From",
                table: "ItemBases",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "ItemBases",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "To",
                table: "ItemBases",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CleaningTime",
                table: "ItemBases",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PoombaItem_Command",
                table: "ItemBases",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PoombaItem_IsRun",
                table: "ItemBases",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemBases",
                table: "ItemBases",
                column: "Id");

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
                        name: "FK_Room_ItemBases_PoombaItemId",
                        column: x => x.PoombaItemId,
                        principalTable: "ItemBases",
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
                name: "Room");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemBases",
                table: "ItemBases");

            migrationBuilder.DropColumn(
                name: "Command",
                table: "ItemBases");

            migrationBuilder.DropColumn(
                name: "HumidityLevel",
                table: "ItemBases");

            migrationBuilder.DropColumn(
                name: "IsHeaterOn",
                table: "ItemBases");

            migrationBuilder.DropColumn(
                name: "IsRun",
                table: "ItemBases");

            migrationBuilder.DropColumn(
                name: "IsVentilationOn",
                table: "ItemBases");

            migrationBuilder.DropColumn(
                name: "MaxHumidity",
                table: "ItemBases");

            migrationBuilder.DropColumn(
                name: "MaxTemperature",
                table: "ItemBases");

            migrationBuilder.DropColumn(
                name: "MinHumidity",
                table: "ItemBases");

            migrationBuilder.DropColumn(
                name: "MinTemperature",
                table: "ItemBases");

            migrationBuilder.DropColumn(
                name: "TemperatureLevel",
                table: "ItemBases");

            migrationBuilder.DropColumn(
                name: "CurtainItem_Command",
                table: "ItemBases");

            migrationBuilder.DropColumn(
                name: "CurtainItem_IsRun",
                table: "ItemBases");

            migrationBuilder.DropColumn(
                name: "LightIntensity",
                table: "ItemBases");

            migrationBuilder.DropColumn(
                name: "MaxLightIntensity",
                table: "ItemBases");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ItemBases");

            migrationBuilder.DropColumn(
                name: "HodoorItem_Command",
                table: "ItemBases");

            migrationBuilder.DropColumn(
                name: "HodoorItem_IsRun",
                table: "ItemBases");

            migrationBuilder.DropColumn(
                name: "OpenStatus",
                table: "ItemBases");

            migrationBuilder.DropColumn(
                name: "Ip",
                table: "ItemBases");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "ItemBases");

            migrationBuilder.DropColumn(
                name: "LightItem_Command",
                table: "ItemBases");

            migrationBuilder.DropColumn(
                name: "IsOn",
                table: "ItemBases");

            migrationBuilder.DropColumn(
                name: "LightItem_IsRun",
                table: "ItemBases");

            migrationBuilder.DropColumn(
                name: "SleepTime",
                table: "ItemBases");

            migrationBuilder.DropColumn(
                name: "WakeUpTime",
                table: "ItemBases");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "ItemBases");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "ItemBases");

            migrationBuilder.DropColumn(
                name: "Body",
                table: "ItemBases");

            migrationBuilder.DropColumn(
                name: "From",
                table: "ItemBases");

            migrationBuilder.DropColumn(
                name: "Subject",
                table: "ItemBases");

            migrationBuilder.DropColumn(
                name: "To",
                table: "ItemBases");

            migrationBuilder.DropColumn(
                name: "CleaningTime",
                table: "ItemBases");

            migrationBuilder.DropColumn(
                name: "PoombaItem_Command",
                table: "ItemBases");

            migrationBuilder.DropColumn(
                name: "PoombaItem_IsRun",
                table: "ItemBases");

            migrationBuilder.RenameTable(
                name: "ItemBases",
                newName: "SnapBoxItems");

            migrationBuilder.AlterColumn<bool>(
                name: "MailReceived",
                table: "SnapBoxItems",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SnapBoxItems",
                table: "SnapBoxItems",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "IpItems",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Ip = table.Column<string>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IpItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LightItems",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Command = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    IsOn = table.Column<bool>(nullable: false),
                    IsRun = table.Column<bool>(nullable: false),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    SleepTime = table.Column<DateTime>(nullable: false),
                    WakeUpTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LightItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LoginItems",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    Password = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MailItems",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Body = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    From = table.Column<string>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    Subject = table.Column<string>(nullable: true),
                    To = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MailItems", x => x.Id);
                });
        }
    }
}
