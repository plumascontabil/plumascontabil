using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Demonstrativo.Migrations
{
    public partial class FixSaldoMensalFKContaCorrente : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContasCorrentes_SaldoMensal_SaldoMensalCompetencia",
                table: "ContasCorrentes");

            migrationBuilder.DropIndex(
                name: "IX_ContasCorrentes_SaldoMensalCompetencia",
                table: "ContasCorrentes");

            migrationBuilder.DropColumn(
                name: "SaldoId",
                table: "ContasCorrentes");

            migrationBuilder.DropColumn(
                name: "SaldoMensalCompetencia",
                table: "ContasCorrentes");

            migrationBuilder.AddColumn<int>(
                name: "ContaCorrenteId",
                table: "SaldoMensal",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SaldoMensal_ContaCorrenteId",
                table: "SaldoMensal",
                column: "ContaCorrenteId");

            migrationBuilder.AddForeignKey(
                name: "FK_SaldoMensal_ContasCorrentes_ContaCorrenteId",
                table: "SaldoMensal",
                column: "ContaCorrenteId",
                principalTable: "ContasCorrentes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SaldoMensal_ContasCorrentes_ContaCorrenteId",
                table: "SaldoMensal");

            migrationBuilder.DropIndex(
                name: "IX_SaldoMensal_ContaCorrenteId",
                table: "SaldoMensal");

            migrationBuilder.DropColumn(
                name: "ContaCorrenteId",
                table: "SaldoMensal");

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
    }
}
