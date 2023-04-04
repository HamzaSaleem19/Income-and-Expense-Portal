using Microsoft.EntityFrameworkCore.Migrations;

namespace Income_and_Expense.Data.Migrations
{
    public partial class Lentfield : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Lent",
                table: "Expenses",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Lent",
                table: "Expenses");
        }
    }
}
