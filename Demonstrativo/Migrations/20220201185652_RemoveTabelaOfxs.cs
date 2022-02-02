using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Demonstrativo.Migrations
{
    public partial class RemoveTabelaOfxs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ofxs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ofxs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmpresaId = table.Column<int>(type: "int", nullable: false),
                    HistoricoOfxId = table.Column<int>(type: "int", nullable: false),
                    TipoLancamento = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValorOfx = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ofxs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ofxs_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Codigo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ofxs_HistoricosOfx_HistoricoOfxId",
                        column: x => x.HistoricoOfxId,
                        principalTable: "HistoricosOfx",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ofxs_EmpresaId",
                table: "Ofxs",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Ofxs_HistoricoOfxId",
                table: "Ofxs",
                column: "HistoricoOfxId");
        }
    }
}
