using Microsoft.EntityFrameworkCore.Migrations;

namespace Demonstrativo.Migrations
{
    public partial class AlterLancamentosNulos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "LancamentoHistorico",
                table: "Contas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "LancamentoDebito",
                table: "Contas",
                type: "int",
                maxLength: 5,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 5);

            migrationBuilder.AlterColumn<int>(
                name: "LancamentoCredito",
                table: "Contas",
                type: "int",
                maxLength: 5,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 5);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "LancamentoHistorico",
                table: "Contas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "LancamentoDebito",
                table: "Contas",
                type: "int",
                maxLength: 5,
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 5,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "LancamentoCredito",
                table: "Contas",
                type: "int",
                maxLength: 5,
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 5,
                oldNullable: true);
        }
    }
}
