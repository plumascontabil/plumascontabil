using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Demonstrativo.Migrations
{
    public partial class AddSaldoMensalFKContaCorrente : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SaldoId",
                table: "ContasCorrentes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "SaldoMensalCompetencia",
                table: "ContasCorrentes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SaldoMensal",
                columns: table => new
                {
                    Competencia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Saldo = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaldoMensal", x => x.Competencia);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContasCorrentes_SaldoMensalCompetencia",
                table: "ContasCorrentes",
                column: "SaldoMensalCompetencia");

            migrationBuilder.AddForeignKey(
                name: "FK_ContasCorrentes_SaldoMensal_SaldoMensalCompetencia",
                table: "ContasCorrentes",
                column: "SaldoMensalCompetencia",
                principalTable: "SaldoMensal",
                principalColumn: "Competencia",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContasCorrentes_SaldoMensal_SaldoMensalCompetencia",
                table: "ContasCorrentes");

            migrationBuilder.DropTable(
                name: "SaldoMensal");

            migrationBuilder.DropIndex(
                name: "IX_ContasCorrentes_SaldoMensalCompetencia",
                table: "ContasCorrentes");

            migrationBuilder.DropColumn(
                name: "SaldoId",
                table: "ContasCorrentes");

            migrationBuilder.DropColumn(
                name: "SaldoMensalCompetencia",
                table: "ContasCorrentes");
        }
    }
}
