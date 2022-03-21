using Microsoft.EntityFrameworkCore.Migrations;

namespace Demonstrativo.Migrations
{
    public partial class AdddFKContasContabeisEmLancamentoPadraoEFKLancamentoPadraoEmOfxLancamentos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LancamentoPadrao_Categorias_CategoriaId",
                table: "LancamentoPadrao");

            migrationBuilder.DropForeignKey(
                name: "FK_LancamentoPadrao_TiposContas_TipoContaId",
                table: "LancamentoPadrao");

            migrationBuilder.DropForeignKey(
                name: "FK_Lancamentos_LancamentoPadrao_ContaId",
                table: "Lancamentos");

            migrationBuilder.DropForeignKey(
                name: "FK_OfxLancamentos_LancamentoPadrao_LancamentoPadraoId",
                table: "OfxLancamentos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LancamentoPadrao",
                table: "LancamentoPadrao");

            migrationBuilder.RenameTable(
                name: "LancamentoPadrao",
                newName: "LancamentosPradroes");

            migrationBuilder.RenameIndex(
                name: "IX_LancamentoPadrao_TipoContaId",
                table: "LancamentosPradroes",
                newName: "IX_LancamentosPradroes_TipoContaId");

            migrationBuilder.RenameIndex(
                name: "IX_LancamentoPadrao_CategoriaId",
                table: "LancamentosPradroes",
                newName: "IX_LancamentosPradroes_CategoriaId");

            migrationBuilder.AddColumn<int>(
                name: "ContaCreditoId",
                table: "LancamentosPradroes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ContaDebitoId",
                table: "LancamentosPradroes",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LancamentosPradroes",
                table: "LancamentosPradroes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_LancamentosPradroes_ContaCreditoId",
                table: "LancamentosPradroes",
                column: "ContaCreditoId");

            migrationBuilder.CreateIndex(
                name: "IX_LancamentosPradroes_ContaDebitoId",
                table: "LancamentosPradroes",
                column: "ContaDebitoId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropIndex(
                name: "IX_LancamentosPradroes_ContaCreditoId",
                table: "LancamentosPradroes");

            migrationBuilder.DropIndex(
                name: "IX_LancamentosPradroes_ContaDebitoId",
                table: "LancamentosPradroes");

            migrationBuilder.DropColumn(
                name: "ContaCreditoId",
                table: "LancamentosPradroes");

            migrationBuilder.DropColumn(
                name: "ContaDebitoId",
                table: "LancamentosPradroes");

            migrationBuilder.RenameTable(
                name: "LancamentosPradroes",
                newName: "LancamentoPadrao");

            migrationBuilder.RenameIndex(
                name: "IX_LancamentosPradroes_TipoContaId",
                table: "LancamentoPadrao",
                newName: "IX_LancamentoPadrao_TipoContaId");

            migrationBuilder.RenameIndex(
                name: "IX_LancamentosPradroes_CategoriaId",
                table: "LancamentoPadrao",
                newName: "IX_LancamentoPadrao_CategoriaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LancamentoPadrao",
                table: "LancamentoPadrao",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LancamentoPadrao_Categorias_CategoriaId",
                table: "LancamentoPadrao",
                column: "CategoriaId",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LancamentoPadrao_TiposContas_TipoContaId",
                table: "LancamentoPadrao",
                column: "TipoContaId",
                principalTable: "TiposContas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Lancamentos_LancamentoPadrao_ContaId",
                table: "Lancamentos",
                column: "ContaId",
                principalTable: "LancamentoPadrao",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OfxLancamentos_LancamentoPadrao_LancamentoPadraoId",
                table: "OfxLancamentos",
                column: "LancamentoPadraoId",
                principalTable: "LancamentoPadrao",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
