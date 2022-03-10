using Microsoft.EntityFrameworkCore.Migrations;

namespace Demonstrativo.Migrations
{
    public partial class AlterandoNomesTabelasOfx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComplementosOfxs_HistoricosOfx_HistoricoId",
                table: "ComplementosOfxs");

            migrationBuilder.DropForeignKey(
                name: "FK_ComplementosOfxs_Ofxs_OfxId",
                table: "ComplementosOfxs");

            migrationBuilder.DropForeignKey(
                name: "FK_ConstasCorrentes_BancoOfxs_BancoOfxId",
                table: "ConstasCorrentes");

            migrationBuilder.DropForeignKey(
                name: "FK_HistoricosOfx_ContasContabeis_ContaCreditoId",
                table: "HistoricosOfx");

            migrationBuilder.DropForeignKey(
                name: "FK_HistoricosOfx_ContasContabeis_ContaDebitoId",
                table: "HistoricosOfx");

            migrationBuilder.DropForeignKey(
                name: "FK_Lotes_Ofxs_LancamentoOfxId",
                table: "Lotes");

            migrationBuilder.DropForeignKey(
                name: "FK_Ofxs_ConstasCorrentes_ContaCorrenteId",
                table: "Ofxs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ofxs",
                table: "Ofxs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HistoricosOfx",
                table: "HistoricosOfx");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ComplementosOfxs",
                table: "ComplementosOfxs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BancoOfxs",
                table: "BancoOfxs");

            migrationBuilder.RenameTable(
                name: "Ofxs",
                newName: "OfxLancamentos");

            migrationBuilder.RenameTable(
                name: "HistoricosOfx",
                newName: "OfxDescricoes");

            migrationBuilder.RenameTable(
                name: "ComplementosOfxs",
                newName: "OfxComplementos");

            migrationBuilder.RenameTable(
                name: "BancoOfxs",
                newName: "OfxBancos");

            migrationBuilder.RenameIndex(
                name: "IX_Ofxs_ContaCorrenteId",
                table: "OfxLancamentos",
                newName: "IX_OfxLancamentos_ContaCorrenteId");

            migrationBuilder.RenameIndex(
                name: "IX_HistoricosOfx_ContaDebitoId",
                table: "OfxDescricoes",
                newName: "IX_OfxDescricoes_ContaDebitoId");

            migrationBuilder.RenameIndex(
                name: "IX_HistoricosOfx_ContaCreditoId",
                table: "OfxDescricoes",
                newName: "IX_OfxDescricoes_ContaCreditoId");

            migrationBuilder.RenameIndex(
                name: "IX_ComplementosOfxs_OfxId",
                table: "OfxComplementos",
                newName: "IX_OfxComplementos_OfxId");

            migrationBuilder.RenameIndex(
                name: "IX_ComplementosOfxs_HistoricoId",
                table: "OfxComplementos",
                newName: "IX_OfxComplementos_HistoricoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OfxLancamentos",
                table: "OfxLancamentos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OfxDescricoes",
                table: "OfxDescricoes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OfxComplementos",
                table: "OfxComplementos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OfxBancos",
                table: "OfxBancos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ConstasCorrentes_OfxBancos_BancoOfxId",
                table: "ConstasCorrentes",
                column: "BancoOfxId",
                principalTable: "OfxBancos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lotes_OfxLancamentos_LancamentoOfxId",
                table: "Lotes",
                column: "LancamentoOfxId",
                principalTable: "OfxLancamentos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OfxComplementos_OfxDescricoes_HistoricoId",
                table: "OfxComplementos",
                column: "HistoricoId",
                principalTable: "OfxDescricoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OfxComplementos_OfxLancamentos_OfxId",
                table: "OfxComplementos",
                column: "OfxId",
                principalTable: "OfxLancamentos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OfxDescricoes_ContasContabeis_ContaCreditoId",
                table: "OfxDescricoes",
                column: "ContaCreditoId",
                principalTable: "ContasContabeis",
                principalColumn: "Codigo",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OfxDescricoes_ContasContabeis_ContaDebitoId",
                table: "OfxDescricoes",
                column: "ContaDebitoId",
                principalTable: "ContasContabeis",
                principalColumn: "Codigo",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OfxLancamentos_ConstasCorrentes_ContaCorrenteId",
                table: "OfxLancamentos",
                column: "ContaCorrenteId",
                principalTable: "ConstasCorrentes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConstasCorrentes_OfxBancos_BancoOfxId",
                table: "ConstasCorrentes");

            migrationBuilder.DropForeignKey(
                name: "FK_Lotes_OfxLancamentos_LancamentoOfxId",
                table: "Lotes");

            migrationBuilder.DropForeignKey(
                name: "FK_OfxComplementos_OfxDescricoes_HistoricoId",
                table: "OfxComplementos");

            migrationBuilder.DropForeignKey(
                name: "FK_OfxComplementos_OfxLancamentos_OfxId",
                table: "OfxComplementos");

            migrationBuilder.DropForeignKey(
                name: "FK_OfxDescricoes_ContasContabeis_ContaCreditoId",
                table: "OfxDescricoes");

            migrationBuilder.DropForeignKey(
                name: "FK_OfxDescricoes_ContasContabeis_ContaDebitoId",
                table: "OfxDescricoes");

            migrationBuilder.DropForeignKey(
                name: "FK_OfxLancamentos_ConstasCorrentes_ContaCorrenteId",
                table: "OfxLancamentos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OfxLancamentos",
                table: "OfxLancamentos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OfxDescricoes",
                table: "OfxDescricoes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OfxComplementos",
                table: "OfxComplementos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OfxBancos",
                table: "OfxBancos");

            migrationBuilder.RenameTable(
                name: "OfxLancamentos",
                newName: "Ofxs");

            migrationBuilder.RenameTable(
                name: "OfxDescricoes",
                newName: "HistoricosOfx");

            migrationBuilder.RenameTable(
                name: "OfxComplementos",
                newName: "ComplementosOfxs");

            migrationBuilder.RenameTable(
                name: "OfxBancos",
                newName: "BancoOfxs");

            migrationBuilder.RenameIndex(
                name: "IX_OfxLancamentos_ContaCorrenteId",
                table: "Ofxs",
                newName: "IX_Ofxs_ContaCorrenteId");

            migrationBuilder.RenameIndex(
                name: "IX_OfxDescricoes_ContaDebitoId",
                table: "HistoricosOfx",
                newName: "IX_HistoricosOfx_ContaDebitoId");

            migrationBuilder.RenameIndex(
                name: "IX_OfxDescricoes_ContaCreditoId",
                table: "HistoricosOfx",
                newName: "IX_HistoricosOfx_ContaCreditoId");

            migrationBuilder.RenameIndex(
                name: "IX_OfxComplementos_OfxId",
                table: "ComplementosOfxs",
                newName: "IX_ComplementosOfxs_OfxId");

            migrationBuilder.RenameIndex(
                name: "IX_OfxComplementos_HistoricoId",
                table: "ComplementosOfxs",
                newName: "IX_ComplementosOfxs_HistoricoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ofxs",
                table: "Ofxs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HistoricosOfx",
                table: "HistoricosOfx",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ComplementosOfxs",
                table: "ComplementosOfxs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BancoOfxs",
                table: "BancoOfxs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ComplementosOfxs_HistoricosOfx_HistoricoId",
                table: "ComplementosOfxs",
                column: "HistoricoId",
                principalTable: "HistoricosOfx",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ComplementosOfxs_Ofxs_OfxId",
                table: "ComplementosOfxs",
                column: "OfxId",
                principalTable: "Ofxs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ConstasCorrentes_BancoOfxs_BancoOfxId",
                table: "ConstasCorrentes",
                column: "BancoOfxId",
                principalTable: "BancoOfxs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HistoricosOfx_ContasContabeis_ContaCreditoId",
                table: "HistoricosOfx",
                column: "ContaCreditoId",
                principalTable: "ContasContabeis",
                principalColumn: "Codigo",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HistoricosOfx_ContasContabeis_ContaDebitoId",
                table: "HistoricosOfx",
                column: "ContaDebitoId",
                principalTable: "ContasContabeis",
                principalColumn: "Codigo",
                onDelete: ReferentialAction.Restrict);

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
        }
    }
}
