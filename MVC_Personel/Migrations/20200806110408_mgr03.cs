using Microsoft.EntityFrameworkCore.Migrations;

namespace MVC_Personel.Migrations
{
    public partial class mgr03 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "tblEmployee");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "tblEmployee",
                nullable: false,
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "tblEmployee");

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "tblEmployee",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }
    }
}
