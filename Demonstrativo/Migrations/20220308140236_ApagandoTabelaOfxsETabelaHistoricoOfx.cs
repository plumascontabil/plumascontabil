using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Demonstrativo.Migrations
{
    public partial class ApagandoTabelaOfxsETabelaHistoricoOfx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConstasCorrentes_BancoOfxs_BancoOfxId",
                table: "ConstasCorrentes");

            migrationBuilder.DropForeignKey(
                name: "FK_ConstasCorrentes_Empresas_EmpresaId",
                table: "ConstasCorrentes");

            migrationBuilder.DropForeignKey(
                name: "FK_Lotes_Ofxs_LancamentoOfxId",
                table: "Lotes");

            migrationBuilder.DropForeignKey(
                name: "FK_Ofxs_ConstasCorrentes_ContaCorrenteId",
                table: "Ofxs");

            migrationBuilder.DropForeignKey(
                name: "FK_Ofxs_HistoricosOfx_HistoricoOfxId",
                table: "Ofxs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ofxs",
                table: "Ofxs");

            migrationBuilder.DropIndex(
                name: "IX_Ofxs_HistoricoOfxId",
                table: "Ofxs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ConstasCorrentes",
                table: "ConstasCorrentes");

            migrationBuilder.DropColumn(
                name: "HistoricoOfxId",
                table: "Ofxs");

            migrationBuilder.RenameTable(
                name: "Ofxs",
                newName: "LancamentoOfx");

            migrationBuilder.RenameTable(
                name: "ConstasCorrentes",
                newName: "ContaCorrente");

            migrationBuilder.RenameIndex(
                name: "IX_Ofxs_ContaCorrenteId",
                table: "LancamentoOfx",
                newName: "IX_LancamentoOfx_ContaCorrenteId");

            migrationBuilder.RenameIndex(
                name: "IX_ConstasCorrentes_EmpresaId",
                table: "ContaCorrente",
                newName: "IX_ContaCorrente_EmpresaId");

            migrationBuilder.RenameIndex(
                name: "IX_ConstasCorrentes_BancoOfxId",
                table: "ContaCorrente",
                newName: "IX_ContaCorrente_BancoOfxId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LancamentoOfx",
                table: "LancamentoOfx",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContaCorrente",
                table: "ContaCorrente",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ComplementosOfxs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DescricaoComplemento = table.Column<string>(type: "varchar(70)", nullable: true),
                    HistoricoId = table.Column<int>(type: "int", nullable: false),
                    OfxId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplementosOfxs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComplementosOfxs_HistoricosOfx_HistoricoId",
                        column: x => x.HistoricoId,
                        principalTable: "HistoricosOfx",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComplementosOfxs_LancamentoOfx_OfxId",
                        column: x => x.OfxId,
                        principalTable: "LancamentoOfx",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SaldoConta",
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
                    table.PrimaryKey("PK_SaldoConta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaldoConta_ContasContabeis_ContaContabilId",
                        column: x => x.ContaContabilId,
                        principalTable: "ContasContabeis",
                        principalColumn: "Codigo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SaldoConta_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Codigo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComplementosOfxs_HistoricoId",
                table: "ComplementosOfxs",
                column: "HistoricoId");

            migrationBuilder.CreateIndex(
                name: "IX_ComplementosOfxs_OfxId",
                table: "ComplementosOfxs",
                column: "OfxId");

            migrationBuilder.CreateIndex(
                name: "IX_SaldoConta_ContaContabilId",
                table: "SaldoConta",
                column: "ContaContabilId");

            migrationBuilder.CreateIndex(
                name: "IX_SaldoConta_EmpresaId",
                table: "SaldoConta",
                column: "EmpresaId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContaCorrente_BancoOfxs_BancoOfxId",
                table: "ContaCorrente",
                column: "BancoOfxId",
                principalTable: "BancoOfxs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContaCorrente_Empresas_EmpresaId",
                table: "ContaCorrente",
                column: "EmpresaId",
                principalTable: "Empresas",
                principalColumn: "Codigo",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LancamentoOfx_ContaCorrente_ContaCorrenteId",
                table: "LancamentoOfx",
                column: "ContaCorrenteId",
                principalTable: "ContaCorrente",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lotes_LancamentoOfx_LancamentoOfxId",
                table: "Lotes",
                column: "LancamentoOfxId",
                principalTable: "LancamentoOfx",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContaCorrente_BancoOfxs_BancoOfxId",
                table: "ContaCorrente");

            migrationBuilder.DropForeignKey(
                name: "FK_ContaCorrente_Empresas_EmpresaId",
                table: "ContaCorrente");

            migrationBuilder.DropForeignKey(
                name: "FK_LancamentoOfx_ContaCorrente_ContaCorrenteId",
                table: "LancamentoOfx");

            migrationBuilder.DropForeignKey(
                name: "FK_Lotes_LancamentoOfx_LancamentoOfxId",
                table: "Lotes");

            migrationBuilder.DropTable(
                name: "ComplementosOfxs");

            migrationBuilder.DropTable(
                name: "SaldoConta");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LancamentoOfx",
                table: "LancamentoOfx");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContaCorrente",
                table: "ContaCorrente");

            migrationBuilder.RenameTable(
                name: "LancamentoOfx",
                newName: "Ofxs");

            migrationBuilder.RenameTable(
                name: "ContaCorrente",
                newName: "ConstasCorrentes");

            migrationBuilder.RenameIndex(
                name: "IX_LancamentoOfx_ContaCorrenteId",
                table: "Ofxs",
                newName: "IX_Ofxs_ContaCorrenteId");

            migrationBuilder.RenameIndex(
                name: "IX_ContaCorrente_EmpresaId",
                table: "ConstasCorrentes",
                newName: "IX_ConstasCorrentes_EmpresaId");

            migrationBuilder.RenameIndex(
                name: "IX_ContaCorrente_BancoOfxId",
                table: "ConstasCorrentes",
                newName: "IX_ConstasCorrentes_BancoOfxId");

            migrationBuilder.AddColumn<int>(
                name: "HistoricoOfxId",
                table: "Ofxs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ofxs",
                table: "Ofxs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConstasCorrentes",
                table: "ConstasCorrentes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Ofxs_HistoricoOfxId",
                table: "Ofxs",
                column: "HistoricoOfxId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConstasCorrentes_BancoOfxs_BancoOfxId",
                table: "ConstasCorrentes",
                column: "BancoOfxId",
                principalTable: "BancoOfxs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ConstasCorrentes_Empresas_EmpresaId",
                table: "ConstasCorrentes",
                column: "EmpresaId",
                principalTable: "Empresas",
                principalColumn: "Codigo",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lotes_Ofxs_LancamentoOfxId",
                table: "Lotes",
                column: "LancamentoOfxId",
                principalTable: "Ofxs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ofxs_ConstasCorrentes_ContaCorrenteId",
                table: "Ofxs",
                column: "ContaCorrenteId",
                principalTable: "ConstasCorrentes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ofxs_HistoricosOfx_HistoricoOfxId",
                table: "Ofxs",
                column: "HistoricoOfxId",
                principalTable: "HistoricosOfx",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
