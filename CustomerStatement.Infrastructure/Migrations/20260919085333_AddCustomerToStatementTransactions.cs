using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerStatement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerToStatementTransactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StatementTransactions_AccountStatements_AccountStatementId",
                table: "StatementTransactions");

            migrationBuilder.AlterColumn<int>(
                name: "AccountStatementId",
                table: "StatementTransactions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
             name: "CustomerId",
             table: "StatementTransactions",
             type: "int",
             nullable: true);

                    migrationBuilder.Sql("""
            UPDATE st
            SET st.CustomerId = ast.CustomerId
            FROM StatementTransactions st
            INNER JOIN AccountStatements ast
                ON st.AccountStatementId = ast.Id;
            """);

            migrationBuilder.AlterColumn<int>(
            name: "CustomerId",
            table: "StatementTransactions",
            type: "int",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "int",
            oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StatementTransactions_CustomerId",
                table: "StatementTransactions",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_StatementTransactions_AccountStatements_AccountStatementId",
                table: "StatementTransactions",
                column: "AccountStatementId",
                principalTable: "AccountStatements",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_StatementTransactions_Customers_CustomerId",
                table: "StatementTransactions",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StatementTransactions_AccountStatements_AccountStatementId",
                table: "StatementTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_StatementTransactions_Customers_CustomerId",
                table: "StatementTransactions");

            migrationBuilder.DropIndex(
                name: "IX_StatementTransactions_CustomerId",
                table: "StatementTransactions");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "StatementTransactions");

            migrationBuilder.AlterColumn<int>(
                name: "AccountStatementId",
                table: "StatementTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StatementTransactions_AccountStatements_AccountStatementId",
                table: "StatementTransactions",
                column: "AccountStatementId",
                principalTable: "AccountStatements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
