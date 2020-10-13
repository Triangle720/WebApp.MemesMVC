using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApp.MemesMVC.Migrations
{
    public partial class x : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "BanReason",
                table: "Users",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccountCreationTime", "BanExpireIn", "BanReason", "Email", "IsBanned", "Login", "Nickname", "Password", "Role" },
                values: new object[] { 1, new DateTime(2020, 10, 13, 2, 31, 5, 320, DateTimeKind.Local).AddTicks(2298), null, "", "admin@admin.net", false, "admin", "DefinitlyNotAnAdmin", "62F04A011FBB80030BB0A13701C20B41", 2 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccountCreationTime", "BanExpireIn", "BanReason", "Email", "IsBanned", "Login", "Nickname", "Password", "Role" },
                values: new object[] { 2, new DateTime(2020, 10, 13, 2, 31, 5, 320, DateTimeKind.Local).AddTicks(3902), null, "", "mod@mod.net", false, "moderator", "moderator", "0408F3C997F309C03B08BF3A4BC7B730", 2 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.AlterColumn<string>(
                name: "BanReason",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);
        }
    }
}
