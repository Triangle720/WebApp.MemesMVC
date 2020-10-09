using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApp.MemesMVC.Migrations
{
    public partial class PicAndUserModelUpd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "BanExpireIn",
                table: "Users",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<bool>(
                name: "IsAccepted",
                table: "Pictures",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LocalPath",
                table: "Pictures",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAccepted",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "LocalPath",
                table: "Pictures");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BanExpireIn",
                table: "Users",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }
    }
}
