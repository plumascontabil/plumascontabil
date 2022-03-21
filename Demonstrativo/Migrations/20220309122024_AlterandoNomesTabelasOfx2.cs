using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Demonstrativo.Migrations
{
    public partial class AlterandoNomesTabelasOfx2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Lotes");

            migrationBuilder.DropTable(
                name: "SaldoContas");

            migrationBuilder.CreateTable(
                name: "OfxLotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Descricao = table.Column<string>(type: "varchar(70)", nullable: false),
                    LancamentoOfxId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfxLotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfxLotes_OfxLancamentos_LancamentoOfxId",
                        column: x => x.LancamentoOfxId,
                        principalTable: "OfxLancamentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OfxSaldoContas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataFim = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Saldo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ContaContabilId = table.Column<int>(type: "int", nullable: false),
                    EmpresaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfxSaldoContas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfxSaldoContas_ContasContabeis_ContaContabilId",
                        column: x => x.ContaContabilId,
                        principalTable: "ContasContabeis",
                        principalColumn: "Codigo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OfxSaldoContas_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Codigo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OfxLotes_LancamentoOfxId",
                table: "OfxLotes",
                column: "LancamentoOfxId");

            migrationBuilder.CreateIndex(
                name: "IX_OfxSaldoContas_ContaContabilId",
                table: "OfxSaldoContas",
                column: "ContaContabilId");

            migrationBuilder.CreateIndex(
                name: "IX_OfxSaldoContas_EmpresaId",
                table: "OfxSaldoContas",
                column: "EmpresaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OfxLotes");

            migrationBuilder.DropTable(
                name: "OfxSaldoContas");

            migrationBuilder.CreateTable(
                name: "Lotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "varchar(70)", nullable: false),
                    LancamentoOfxId = table.Column<int>(type: "int", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lotes_OfxLancamentos_LancamentoOfxId",
                        column: x => x.LancamentoOfxId,
                        principalTable: "OfxLancamentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SaldoContas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContaContabilId = table.Column<int>(type: "int", nullable: false),
                    DataFim = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmpresaId = table.Column<int>(type: "int", nullable: false),
                    Saldo = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaldoContas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaldoContas_ContasContabeis_ContaContabilId",
                        column: x => x.ContaContabilId,
                        principalTable: "ContasContabeis",
                        principalColumn: "Codigo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SaldoContas_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Codigo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lotes_LancamentoOfxId",
                table: "Lotes",
                column: "LancamentoOfxId");

            migrationBuilder.CreateIndex(
                name: "IX_SaldoContas_ContaContabilId",
                table: "SaldoContas",
                column: "ContaContabilId");

            migrationBuilder.CreateIndex(
                name: "IX_SaldoContas_EmpresaId",
                table: "SaldoContas",
                column: "EmpresaId");
        }
    }
}
