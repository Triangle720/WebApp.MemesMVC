using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApp.MemesMVC.Migrations
{
    public partial class PicModelAcceptCol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAccepted",
                table: "Pictures",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "AccountCreationTime",
                value: new DateTime(2020, 10, 16, 4, 3, 16, 866, DateTimeKind.Local).AddTicks(2539));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "AccountCreationTime",
                value: new DateTime(2020, 10, 16, 4, 3, 16, 866, DateTimeKind.Local).AddTicks(3615));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAccepted",
                table: "Pictures");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "AccountCreationTime",
                value: new DateTime(2020, 10, 15, 6, 11, 53, 218, DateTimeKind.Local).AddTicks(6635));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "AccountCreationTime",
                value: new DateTime(2020, 10, 15, 6, 11, 53, 218, DateTimeKind.Local).AddTicks(7738));
        }
    }
}
