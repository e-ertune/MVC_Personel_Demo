using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MVC_Personel.Migrations
{
    public partial class mgr01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblDepartment",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblDepartment", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "tblPosition",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Authority = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblPosition", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "tblEmployee",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastName = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    IdentityNumber = table.Column<string>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: false),
                    Gender = table.Column<string>(maxLength: 1, nullable: false),
                    Username = table.Column<string>(nullable: false),
                    Password = table.Column<string>(maxLength: 20, nullable: false),
                    Address = table.Column<string>(nullable: false),
                    RegistirationNumber = table.Column<string>(nullable: false),
                    DateOfStart = table.Column<DateTime>(type: "date", nullable: false),
                    Active = table.Column<bool>(nullable: false, defaultValue: true),
                    DateOfLeave = table.Column<DateTime>(type: "date", nullable: false),
                    DepartmentID = table.Column<int>(nullable: false),
                    PositionID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblEmployee", x => x.ID);
                    table.ForeignKey(
                        name: "FK_tblEmployee_tblDepartment_DepartmentID",
                        column: x => x.DepartmentID,
                        principalTable: "tblDepartment",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblEmployee_tblPosition_PositionID",
                        column: x => x.PositionID,
                        principalTable: "tblPosition",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "uk_tblDepartment_name",
                table: "tblDepartment",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tblEmployee_DepartmentID",
                table: "tblEmployee",
                column: "DepartmentID");

            migrationBuilder.CreateIndex(
                name: "uk_tblEmployee_identity",
                table: "tblEmployee",
                column: "IdentityNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uk_tblEmployee_phone",
                table: "tblEmployee",
                column: "PhoneNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tblEmployee_PositionID",
                table: "tblEmployee",
                column: "PositionID");

            migrationBuilder.CreateIndex(
                name: "uk_tblEmployee_reg",
                table: "tblEmployee",
                column: "RegistirationNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uk_tblEmployee_username",
                table: "tblEmployee",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uk_tblPosition_name",
                table: "tblPosition",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblEmployee");

            migrationBuilder.DropTable(
                name: "tblDepartment");

            migrationBuilder.DropTable(
                name: "tblPosition");
        }
    }
}
