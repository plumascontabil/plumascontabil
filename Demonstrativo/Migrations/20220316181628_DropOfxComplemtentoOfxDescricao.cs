using Microsoft.EntityFrameworkCore.Migrations;

namespace Demonstrativo.Migrations
{
    public partial class DropOfxComplemtentoOfxDescricao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "FK_OfxLotes_OfxLancamentos_LancamentoOfxId",
                table: "OfxLotes");

            migrationBuilder.DropForeignKey(
                name: "FK_OfxSaldoContas_ContasContabeis_ContaContabilId",
                table: "OfxSaldoContas");

            migrationBuilder.DropForeignKey(
                name: "FK_OfxSaldoContas_Empresas_EmpresaId",
                table: "OfxSaldoContas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OfxSaldoContas",
                table: "OfxSaldoContas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OfxLotes",
                table: "OfxLotes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OfxDescricoes",
                table: "OfxDescricoes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OfxComplementos",
                table: "OfxComplementos");

            migrationBuilder.DropIndex(
                name: "IX_OfxComplementos_OfxId",
                table: "OfxComplementos");

            migrationBuilder.DropColumn(
                name: "OfxId",
                table: "OfxComplementos");

            migrationBuilder.RenameTable(
                name: "OfxSaldoContas",
                newName: "OfxSaldoConta");

            migrationBuilder.RenameTable(
                name: "OfxLotes",
                newName: "OfxLoteLancamento");

            migrationBuilder.RenameTable(
                name: "OfxDescricoes",
                newName: "OfxDescricao");

            migrationBuilder.RenameTable(
                name: "OfxComplementos",
                newName: "OfxComplemento");

            migrationBuilder.RenameIndex(
                name: "IX_OfxSaldoContas_EmpresaId",
                table: "OfxSaldoConta",
                newName: "IX_OfxSaldoConta_EmpresaId");

            migrationBuilder.RenameIndex(
                name: "IX_OfxSaldoContas_ContaContabilId",
                table: "OfxSaldoConta",
                newName: "IX_OfxSaldoConta_ContaContabilId");

            migrationBuilder.RenameIndex(
                name: "IX_OfxLotes_LancamentoOfxId",
                table: "OfxLoteLancamento",
                newName: "IX_OfxLoteLancamento_LancamentoOfxId");

            migrationBuilder.RenameIndex(
                name: "IX_OfxDescricoes_ContaDebitoId",
                table: "OfxDescricao",
                newName: "IX_OfxDescricao_ContaDebitoId");

            migrationBuilder.RenameIndex(
                name: "IX_OfxDescricoes_ContaCreditoId",
                table: "OfxDescricao",
                newName: "IX_OfxDescricao_ContaCreditoId");

            migrationBuilder.RenameIndex(
                name: "IX_OfxComplementos_HistoricoId",
                table: "OfxComplemento",
                newName: "IX_OfxComplemento_HistoricoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OfxSaldoConta",
                table: "OfxSaldoConta",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OfxLoteLancamento",
                table: "OfxLoteLancamento",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OfxDescricao",
                table: "OfxDescricao",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OfxComplemento",
                table: "OfxComplemento",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OfxComplemento_OfxDescricao_HistoricoId",
                table: "OfxComplemento",
                column: "HistoricoId",
                principalTable: "OfxDescricao",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OfxDescricao_ContasContabeis_ContaCreditoId",
                table: "OfxDescricao",
                column: "ContaCreditoId",
                principalTable: "ContasContabeis",
                principalColumn: "Codigo",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OfxDescricao_ContasContabeis_ContaDebitoId",
                table: "OfxDescricao",
                column: "ContaDebitoId",
                principalTable: "ContasContabeis",
                principalColumn: "Codigo",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OfxLoteLancamento_OfxLancamentos_LancamentoOfxId",
                table: "OfxLoteLancamento",
                column: "LancamentoOfxId",
                principalTable: "OfxLancamentos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OfxSaldoConta_ContasContabeis_ContaContabilId",
                table: "OfxSaldoConta",
                column: "ContaContabilId",
                principalTable: "ContasContabeis",
                principalColumn: "Codigo",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OfxSaldoConta_Empresas_EmpresaId",
                table: "OfxSaldoConta",
                column: "EmpresaId",
                principalTable: "Empresas",
                principalColumn: "Codigo",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfxComplemento_OfxDescricao_HistoricoId",
                table: "OfxComplemento");

            migrationBuilder.DropForeignKey(
                name: "FK_OfxDescricao_ContasContabeis_ContaCreditoId",
                table: "OfxDescricao");

            migrationBuilder.DropForeignKey(
                name: "FK_OfxDescricao_ContasContabeis_ContaDebitoId",
                table: "OfxDescricao");

            migrationBuilder.DropForeignKey(
                name: "FK_OfxLoteLancamento_OfxLancamentos_LancamentoOfxId",
                table: "OfxLoteLancamento");

            migrationBuilder.DropForeignKey(
                name: "FK_OfxSaldoConta_ContasContabeis_ContaContabilId",
                table: "OfxSaldoConta");

            migrationBuilder.DropForeignKey(
                name: "FK_OfxSaldoConta_Empresas_EmpresaId",
                table: "OfxSaldoConta");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OfxSaldoConta",
                table: "OfxSaldoConta");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OfxLoteLancamento",
                table: "OfxLoteLancamento");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OfxDescricao",
                table: "OfxDescricao");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OfxComplemento",
                table: "OfxComplemento");

            migrationBuilder.RenameTable(
                name: "OfxSaldoConta",
                newName: "OfxSaldoContas");

            migrationBuilder.RenameTable(
                name: "OfxLoteLancamento",
                newName: "OfxLotes");

            migrationBuilder.RenameTable(
                name: "OfxDescricao",
                newName: "OfxDescricoes");

            migrationBuilder.RenameTable(
                name: "OfxComplemento",
                newName: "OfxComplementos");

            migrationBuilder.RenameIndex(
                name: "IX_OfxSaldoConta_EmpresaId",
                table: "OfxSaldoContas",
                newName: "IX_OfxSaldoContas_EmpresaId");

            migrationBuilder.RenameIndex(
                name: "IX_OfxSaldoConta_ContaContabilId",
                table: "OfxSaldoContas",
                newName: "IX_OfxSaldoContas_ContaContabilId");

            migrationBuilder.RenameIndex(
                name: "IX_OfxLoteLancamento_LancamentoOfxId",
                table: "OfxLotes",
                newName: "IX_OfxLotes_LancamentoOfxId");

            migrationBuilder.RenameIndex(
                name: "IX_OfxDescricao_ContaDebitoId",
                table: "OfxDescricoes",
                newName: "IX_OfxDescricoes_ContaDebitoId");

            migrationBuilder.RenameIndex(
                name: "IX_OfxDescricao_ContaCreditoId",
                table: "OfxDescricoes",
                newName: "IX_OfxDescricoes_ContaCreditoId");

            migrationBuilder.RenameIndex(
                name: "IX_OfxComplemento_HistoricoId",
                table: "OfxComplementos",
                newName: "IX_OfxComplementos_HistoricoId");

            migrationBuilder.AddColumn<int>(
                name: "OfxId",
                table: "OfxComplementos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OfxSaldoContas",
                table: "OfxSaldoContas",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OfxLotes",
                table: "OfxLotes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OfxDescricoes",
                table: "OfxDescricoes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OfxComplementos",
                table: "OfxComplementos",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_OfxComplementos_OfxId",
                table: "OfxComplementos",
                column: "OfxId");

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
                name: "FK_OfxLotes_OfxLancamentos_LancamentoOfxId",
                table: "OfxLotes",
                column: "LancamentoOfxId",
                principalTable: "OfxLancamentos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OfxSaldoContas_ContasContabeis_ContaContabilId",
                table: "OfxSaldoContas",
                column: "ContaContabilId",
                principalTable: "ContasContabeis",
                principalColumn: "Codigo",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OfxSaldoContas_Empresas_EmpresaId",
                table: "OfxSaldoContas",
                column: "EmpresaId",
                principalTable: "Empresas",
                principalColumn: "Codigo",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
