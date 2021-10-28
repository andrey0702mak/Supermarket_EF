using Microsoft.EntityFrameworkCore.Migrations;

namespace Supermarket_EF.Migrations
{
    public partial class EmployeeUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Departments_Deparment_ID",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_Deparment_ID",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Deparment_ID",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Deparment_Id",
                table: "Employees");

            migrationBuilder.AddColumn<bool>(
                name: "IsVaccinated",
                table: "Employees",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVaccinated",
                table: "Employees");

            migrationBuilder.AddColumn<int>(
                name: "Deparment_ID",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Deparment_Id",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Deparment_ID",
                table: "Employees",
                column: "Deparment_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Departments_Deparment_ID",
                table: "Employees",
                column: "Deparment_ID",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
