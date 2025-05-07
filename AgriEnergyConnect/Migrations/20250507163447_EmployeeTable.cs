using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AgriEnergyConnect.Migrations
{
    /// <inheritdoc />
    public partial class EmployeeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Farmers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Farmers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Farmers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId1",
                table: "Farmers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApplicationUserId1 = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employees_AspNetUsers_ApplicationUserId1",
                        column: x => x.ApplicationUserId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Farmers_ApplicationUserId1",
                table: "Farmers",
                column: "ApplicationUserId1",
                unique: true,
                filter: "[ApplicationUserId1] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ApplicationUserId",
                table: "Employees",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ApplicationUserId1",
                table: "Employees",
                column: "ApplicationUserId1",
                unique: true,
                filter: "[ApplicationUserId1] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Farmers_AspNetUsers_ApplicationUserId1",
                table: "Farmers",
                column: "ApplicationUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Farmers_AspNetUsers_ApplicationUserId1",
                table: "Farmers");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Farmers_ApplicationUserId1",
                table: "Farmers");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId1",
                table: "Farmers");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Farmers",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.InsertData(
                table: "Farmers",
                columns: new[] { "Id", "ApplicationUserId", "ContactInfo", "FullName", "Location" },
                values: new object[,]
                {
                    { 1, null, "082 843 2634", "Thabo Mokoena", "Free State" },
                    { 2, null, "082 691 0340", "Anika Jacobs", "Western Cape" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Category", "FarmerId", "Name", "ProductionDate" },
                values: new object[,]
                {
                    { 1, "Vegetables", 1, "Tomatoes", new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "Green Tech", 2, "Wind-Powered Water Pump", new DateTime(2025, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }
    }
}
