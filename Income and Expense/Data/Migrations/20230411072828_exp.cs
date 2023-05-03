using Microsoft.EntityFrameworkCore.Migrations;

namespace Income_and_Expense.Data.Migrations
{
    public partial class exp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Groupss_Group_Id",
                table: "Expenses");

            migrationBuilder.AlterColumn<int>(
                name: "Group_Id",
                table: "Expenses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Groupss_Group_Id",
                table: "Expenses",
                column: "Group_Id",
                principalTable: "Groupss",
                principalColumn: "Group_Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Groupss_Group_Id",
                table: "Expenses");

            migrationBuilder.AlterColumn<int>(
                name: "Group_Id",
                table: "Expenses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Groupss_Group_Id",
                table: "Expenses",
                column: "Group_Id",
                principalTable: "Groupss",
                principalColumn: "Group_Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
