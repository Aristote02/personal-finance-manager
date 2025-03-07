using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalFinanceManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Budgets_Users_BudgetId",
                table: "Budgets");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Users_NotificationId",
                table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "SavingsGoalId",
                table: "SavingsGoals",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "NotificationId",
                table: "Notifications",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "IncomeId",
                table: "Incomes",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ExpenseId",
                table: "Expenses",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "BudgetId",
                table: "Budgets",
                newName: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Budgets_Users_Id",
                table: "Budgets",
                column: "Id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Users_Id",
                table: "Notifications",
                column: "Id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Budgets_Users_Id",
                table: "Budgets");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Users_Id",
                table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "SavingsGoals",
                newName: "SavingsGoalId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Notifications",
                newName: "NotificationId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Incomes",
                newName: "IncomeId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Expenses",
                newName: "ExpenseId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Budgets",
                newName: "BudgetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Budgets_Users_BudgetId",
                table: "Budgets",
                column: "BudgetId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Users_NotificationId",
                table: "Notifications",
                column: "NotificationId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
