using Microsoft.EntityFrameworkCore.Migrations;

namespace Demonstrativo.Migrations
{
    public partial class FixLancamentosPadroes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lancamentos_LancamentosPradroes_ContaId",
                table: "Lancamentos");

            migrationBuilder.DropForeignKey(
                name: "FK_LancamentosPradroes_Categorias_CategoriaId",
                table: "LancamentosPradroes");

            migrationBuilder.DropForeignKey(
                name: "FK_LancamentosPradroes_ContasContabeis_ContaCreditoId",
                table: "LancamentosPradroes");

            migrationBuilder.DropForeignKey(
                name: "FK_LancamentosPradroes_ContasContabeis_ContaDebitoId",
                table: "LancamentosPradroes");

            migrationBuilder.DropForeignKey(
                name: "FK_LancamentosPradroes_TiposContas_TipoContaId",
                table: "LancamentosPradroes");

            migrationBuilder.DropForeignKey(
                name: "FK_OfxLancamentos_LancamentosPradroes_LancamentoPadraoId",
                table: "OfxLancamentos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LancamentosPradroes",
                table: "LancamentosPradroes");

            migrationBuilder.RenameTable(
                name: "LancamentosPradroes",
                newName: "LancamentosPadroes");

            migrationBuilder.RenameIndex(
                name: "IX_LancamentosPradroes_TipoContaId",
                table: "LancamentosPadroes",
                newName: "IX_LancamentosPadroes_TipoContaId");

            migrationBuilder.RenameIndex(
                name: "IX_LancamentosPradroes_ContaDebitoId",
                table: "LancamentosPadroes",
                newName: "IX_LancamentosPadroes_ContaDebitoId");

            migrationBuilder.RenameIndex(
                name: "IX_LancamentosPradroes_ContaCreditoId",
                table: "LancamentosPadroes",
                newName: "IX_LancamentosPadroes_ContaCreditoId");

            migrationBuilder.RenameIndex(
                name: "IX_LancamentosPradroes_CategoriaId",
                table: "LancamentosPadroes",
                newName: "IX_LancamentosPadroes_CategoriaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LancamentosPadroes",
                table: "LancamentosPadroes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lancamentos_LancamentosPadroes_ContaId",
                table: "Lancamentos",
                column: "ContaId",
                principalTable: "LancamentosPadroes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LancamentosPadroes_Categorias_CategoriaId",
                table: "LancamentosPadroes",
                column: "CategoriaId",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LancamentosPadroes_ContasContabeis_ContaCreditoId",
                table: "LancamentosPadroes",
                column: "ContaCreditoId",
                principalTable: "ContasContabeis",
                principalColumn: "Codigo",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LancamentosPadroes_ContasContabeis_ContaDebitoId",
                table: "LancamentosPadroes",
                column: "ContaDebitoId",
                principalTable: "ContasContabeis",
                principalColumn: "Codigo",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LancamentosPadroes_TiposContas_TipoContaId",
                table: "LancamentosPadroes",
                column: "TipoContaId",
                principalTable: "TiposContas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OfxLancamentos_LancamentosPadroes_LancamentoPadraoId",
                table: "OfxLancamentos",
                column: "LancamentoPadraoId",
                principalTable: "LancamentosPadroes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lancamentos_LancamentosPadroes_ContaId",
                table: "Lancamentos");

            migrationBuilder.DropForeignKey(
                name: "FK_LancamentosPadroes_Categorias_CategoriaId",
                table: "LancamentosPadroes");

            migrationBuilder.DropForeignKey(
                name: "FK_LancamentosPadroes_ContasContabeis_ContaCreditoId",
                table: "LancamentosPadroes");

            migrationBuilder.DropForeignKey(
                name: "FK_LancamentosPadroes_ContasContabeis_ContaDebitoId",
                table: "LancamentosPadroes");

            migrationBuilder.DropForeignKey(
                name: "FK_LancamentosPadroes_TiposContas_TipoContaId",
                table: "LancamentosPadroes");

            migrationBuilder.DropForeignKey(
                name: "FK_OfxLancamentos_LancamentosPadroes_LancamentoPadraoId",
                table: "OfxLancamentos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LancamentosPadroes",
                table: "LancamentosPadroes");

            migrationBuilder.RenameTable(
                name: "LancamentosPadroes",
                newName: "LancamentosPradroes");

            migrationBuilder.RenameIndex(
                name: "IX_LancamentosPadroes_TipoContaId",
                table: "LancamentosPradroes",
                newName: "IX_LancamentosPradroes_TipoContaId");

            migrationBuilder.RenameIndex(
                name: "IX_LancamentosPadroes_ContaDebitoId",
                table: "LancamentosPradroes",
                newName: "IX_LancamentosPradroes_ContaDebitoId");

            migrationBuilder.RenameIndex(
                name: "IX_LancamentosPadroes_ContaCreditoId",
                table: "LancamentosPradroes",
                newName: "IX_LancamentosPradroes_ContaCreditoId");

            migrationBuilder.RenameIndex(
                name: "IX_LancamentosPadroes_CategoriaId",
                table: "LancamentosPradroes",
                newName: "IX_LancamentosPradroes_CategoriaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LancamentosPradroes",
                table: "LancamentosPradroes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lancamentos_LancamentosPradroes_ContaId",
                table: "Lancamentos",
                column: "ContaId",
                principalTable: "LancamentosPradroes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LancamentosPradroes_Categorias_CategoriaId",
                table: "LancamentosPradroes",
                column: "CategoriaId",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LancamentosPradroes_ContasContabeis_ContaCreditoId",
                table: "LancamentosPradroes",
                column: "ContaCreditoId",
                principalTable: "ContasContabeis",
                principalColumn: "Codigo",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LancamentosPradroes_ContasContabeis_ContaDebitoId",
                table: "LancamentosPradroes",
                column: "ContaDebitoId",
                principalTable: "ContasContabeis",
                principalColumn: "Codigo",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LancamentosPradroes_TiposContas_TipoContaId",
                table: "LancamentosPradroes",
                column: "TipoContaId",
                principalTable: "TiposContas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OfxLancamentos_LancamentosPradroes_LancamentoPadraoId",
                table: "OfxLancamentos",
                column: "LancamentoPadraoId",
                principalTable: "LancamentosPradroes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
