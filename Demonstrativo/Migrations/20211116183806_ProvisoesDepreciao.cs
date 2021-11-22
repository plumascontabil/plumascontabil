using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Demonstrativo.Migrations
{
    public partial class ProvisoesDepreciao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProvisoesDepreciacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DecimoTerceiro = table.Column<decimal>(type: "decimal(11,2)", nullable: false),
                    Ferias = table.Column<decimal>(type: "decimal(11,2)", nullable: false),
                    Depreciacao = table.Column<decimal>(type: "decimal(11,2)", nullable: false),
                    DataCompetencia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmpresaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProvisoesDepreciacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProvisoesDepreciacoes_Competencias_DataCompetencia",
                        column: x => x.DataCompetencia,
                        principalTable: "Competencias",
                        principalColumn: "Data",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProvisoesDepreciacoes_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Codigo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProvisoesDepreciacoes_DataCompetencia",
                table: "ProvisoesDepreciacoes",
                column: "DataCompetencia");

            migrationBuilder.CreateIndex(
                name: "IX_ProvisoesDepreciacoes_EmpresaId",
                table: "ProvisoesDepreciacoes",
                column: "EmpresaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProvisoesDepreciacoes");
        }
    }
}
