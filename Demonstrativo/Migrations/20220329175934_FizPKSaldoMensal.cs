using Microsoft.EntityFrameworkCore.Migrations;

namespace Demonstrativo.Migrations
{
    public partial class FizPKSaldoMensal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SaldoMensal",
                table: "SaldoMensal");

            migrationBuilder.DropColumn(
                name: "Complemento",
                table: "OfxLancamentos");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "SaldoMensal",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SaldoMensal",
                table: "SaldoMensal",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SaldoMensal",
                table: "SaldoMensal");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "SaldoMensal");

            migrationBuilder.AddColumn<string>(
                name: "Complemento",
                table: "OfxLancamentos",
                type: "varchar(150)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SaldoMensal",
                table: "SaldoMensal",
                column: "Competencia");
        }
    }
}
