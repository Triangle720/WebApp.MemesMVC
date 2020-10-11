using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApp.MemesMVC.Migrations
{
    public partial class BanReasonAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BanReason",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BanReason",
                table: "Users");
        }
    }
}
