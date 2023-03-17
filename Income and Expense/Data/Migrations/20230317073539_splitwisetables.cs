using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Income_and_Expense.Data.Migrations
{
    public partial class splitwisetables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Groupss",
                columns: table => new
                {
                    Group_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Group_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Group_Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groupss", x => x.Group_Id);
                });

            migrationBuilder.CreateTable(
                name: "Expenses",
                columns: table => new
                {
                    Expense_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Group_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.Expense_Id);
                    table.ForeignKey(
                        name: "FK_Expenses_Groupss_Group_Id",
                        column: x => x.Group_Id,
                        principalTable: "Groupss",
                        principalColumn: "Group_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserGroups",
                columns: table => new
                {
                    UserGroup_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Group_Id = table.Column<int>(type: "int", nullable: false),
                    User_Id = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroups", x => x.UserGroup_Id);
                    table.ForeignKey(
                        name: "FK_UserGroups_AspNetUsers_User_Id",
                        column: x => x.User_Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserGroups_Groupss_Group_Id",
                        column: x => x.Group_Id,
                        principalTable: "Groupss",
                        principalColumn: "Group_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ManageExpenses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Expense_Id = table.Column<int>(type: "int", nullable: false),
                    User_Id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Amount = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManageExpenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ManageExpenses_AspNetUsers_User_Id",
                        column: x => x.User_Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ManageExpenses_Expenses_Expense_Id",
                        column: x => x.Expense_Id,
                        principalTable: "Expenses",
                        principalColumn: "Expense_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_Group_Id",
                table: "Expenses",
                column: "Group_Id");

            migrationBuilder.CreateIndex(
                name: "IX_ManageExpenses_Expense_Id",
                table: "ManageExpenses",
                column: "Expense_Id");

            migrationBuilder.CreateIndex(
                name: "IX_ManageExpenses_User_Id",
                table: "ManageExpenses",
                column: "User_Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_Group_Id",
                table: "UserGroups",
                column: "Group_Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_User_Id",
                table: "UserGroups",
                column: "User_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ManageExpenses");

            migrationBuilder.DropTable(
                name: "UserGroups");

            migrationBuilder.DropTable(
                name: "Expenses");

            migrationBuilder.DropTable(
                name: "Groupss");
        }
    }
}
