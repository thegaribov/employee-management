using Microsoft.EntityFrameworkCore.Migrations;

namespace EmployeeManagement.DataAccess.Migrations
{
    public partial class EmployeeMonthlyPayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MonthlyPayment",
                table: "Employees",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MonthlyPayment",
                table: "Employees");
        }
    }
}
