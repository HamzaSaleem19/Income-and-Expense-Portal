using Microsoft.EntityFrameworkCore.Migrations;

namespace Income_and_Expense.Data.Migrations
{
    public partial class expensefields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Equallysplited",
                table: "Expenses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Paidby",
                table: "Expenses",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Equallysplited",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "Paidby",
                table: "Expenses");
        }
    }
}
