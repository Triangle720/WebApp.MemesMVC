using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApp.MemesMVC.Migrations
{
    public partial class PictureConstraitUpdt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_Users_UserModelId",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Pictures");

            migrationBuilder.AlterColumn<int>(
                name: "UserModelId",
                table: "Pictures",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Pictures",
                maxLength: 100,
                nullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_Users_UserModelId",
                table: "Pictures",
                column: "UserModelId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_Users_UserModelId",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Pictures");

            migrationBuilder.AlterColumn<int>(
                name: "UserModelId",
                table: "Pictures",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Pictures",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Pictures",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
                column: "AccountCreationTime",
                value: new DateTime(2020, 10, 13, 18, 53, 9, 472, DateTimeKind.Local).AddTicks(8072));

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_Users_UserModelId",
                table: "Pictures",
                column: "UserModelId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
