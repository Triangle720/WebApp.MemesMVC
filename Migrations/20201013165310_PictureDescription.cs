using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApp.MemesMVC.Migrations
{
    public partial class PictureDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAccepted",
                table: "Pictures");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Pictures",
                maxLength: 100,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "AccountCreationTime",
                value: new DateTime(2020, 10, 13, 18, 53, 9, 472, DateTimeKind.Local).AddTicks(6917));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AccountCreationTime", "Role" },
                values: new object[] { new DateTime(2020, 10, 13, 18, 53, 9, 472, DateTimeKind.Local).AddTicks(8072), 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Pictures");

            migrationBuilder.AddColumn<bool>(
                name: "IsAccepted",
                table: "Pictures",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "AccountCreationTime",
                value: new DateTime(2020, 10, 13, 2, 31, 5, 320, DateTimeKind.Local).AddTicks(2298));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AccountCreationTime", "Role" },
                values: new object[] { new DateTime(2020, 10, 13, 2, 31, 5, 320, DateTimeKind.Local).AddTicks(3902), 2 });
        }
    }
}
