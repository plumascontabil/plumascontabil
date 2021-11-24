using Microsoft.EntityFrameworkCore.Migrations;

namespace Demonstrativo.Migrations
{
    public partial class ProvisoesDepreciacoesAlterNulo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "SaldoPrejuizo",
                table: "ProvisoesDepreciacoes",
                type: "decimal(11,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(11,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Ferias",
                table: "ProvisoesDepreciacoes",
                type: "decimal(11,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(11,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Depreciacao",
                table: "ProvisoesDepreciacoes",
                type: "decimal(11,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(11,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "DecimoTerceiro",
                table: "ProvisoesDepreciacoes",
                type: "decimal(11,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(11,2)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "SaldoPrejuizo",
                table: "ProvisoesDepreciacoes",
                type: "decimal(11,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(11,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Ferias",
                table: "ProvisoesDepreciacoes",
                type: "decimal(11,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(11,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Depreciacao",
                table: "ProvisoesDepreciacoes",
                type: "decimal(11,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(11,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "DecimoTerceiro",
                table: "ProvisoesDepreciacoes",
                type: "decimal(11,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(11,2)",
                oldNullable: true);
        }
    }
}
