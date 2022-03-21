using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Demonstrativo.Migrations
{
    public partial class FixCodigoContasContabeisAutoIncrementoFalse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContasContabeis",
                columns: table => new
                {
                    Codigo = table.Column<int>(type: "int", nullable: false),
                    Classificacao = table.Column<int>(type: "int", nullable: false),
                    Historico = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContasContabeis", x => x.Codigo);
                });

            migrationBuilder.CreateTable(
                name: "Ofxs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoLancamento = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HistoricoOfx = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValorOfx = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ContaContabilId = table.Column<int>(type: "int", nullable: true),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ofxs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ofxs_ContasContabeis_ContaContabilId",
                        column: x => x.ContaContabilId,
                        principalTable: "ContasContabeis",
                        principalColumn: "Codigo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ofxs_ContaContabilId",
                table: "Ofxs",
                column: "ContaContabilId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ofxs");

            migrationBuilder.DropTable(
                name: "ContasContabeis");
        }
    }
}
