using Microsoft.EntityFrameworkCore.Migrations;

namespace Demonstrativo.Migrations
{
    public partial class DropContas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contas_Categorias_CategoriaId",
                table: "Contas");

            migrationBuilder.DropForeignKey(
                name: "FK_Contas_TiposContas_TipoContaId",
                table: "Contas");

            migrationBuilder.DropForeignKey(
                name: "FK_Lancamentos_Contas_ContaId",
                table: "Lancamentos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contas",
                table: "Contas");

            migrationBuilder.DropColumn(
                name: "LancamentoCredito",
                table: "Contas");

            migrationBuilder.DropColumn(
                name: "LancamentoDebito",
                table: "Contas");

            migrationBuilder.RenameTable(
                name: "Contas",
                newName: "LancamentoPadrao");

            migrationBuilder.RenameIndex(
                name: "IX_Contas_TipoContaId",
                table: "LancamentoPadrao",
                newName: "IX_LancamentoPadrao_TipoContaId");

            migrationBuilder.RenameIndex(
                name: "IX_Contas_CategoriaId",
                table: "LancamentoPadrao",
                newName: "IX_LancamentoPadrao_CategoriaId");

            migrationBuilder.AddColumn<int>(
                name: "LancamentoPadraoId",
                table: "OfxLancamentos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ContaContabilCodigo",
                table: "LancamentoPadrao",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ContaContabilCodigo1",
                table: "LancamentoPadrao",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LancamentoPadrao",
                table: "LancamentoPadrao",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Descricao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Observacao = table.Column<string>(type: "varchar(1000)", nullable: true),
                    VendaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Descricao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Descricao_Vendas_VendaId",
                        column: x => x.VendaId,
                        principalTable: "Vendas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OfxLancamentos_LancamentoPadraoId",
                table: "OfxLancamentos",
                column: "LancamentoPadraoId");

            migrationBuilder.CreateIndex(
                name: "IX_LancamentoPadrao_ContaContabilCodigo",
                table: "LancamentoPadrao",
                column: "ContaContabilCodigo");

            migrationBuilder.CreateIndex(
                name: "IX_LancamentoPadrao_ContaContabilCodigo1",
                table: "LancamentoPadrao",
                column: "ContaContabilCodigo1");

            migrationBuilder.CreateIndex(
                name: "IX_Descricao_VendaId",
                table: "Descricao",
                column: "VendaId");

            migrationBuilder.AddForeignKey(
                name: "FK_LancamentoPadrao_Categorias_CategoriaId",
                table: "LancamentoPadrao",
                column: "CategoriaId",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LancamentoPadrao_ContasContabeis_ContaContabilCodigo",
                table: "LancamentoPadrao",
                column: "ContaContabilCodigo",
                principalTable: "ContasContabeis",
                principalColumn: "Codigo",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LancamentoPadrao_ContasContabeis_ContaContabilCodigo1",
                table: "LancamentoPadrao",
                column: "ContaContabilCodigo1",
                principalTable: "ContasContabeis",
                principalColumn: "Codigo",
                onDelete: ReferentialAction.Restrict);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LancamentoPadrao_Categorias_CategoriaId",
                table: "LancamentoPadrao");

            migrationBuilder.DropForeignKey(
                name: "FK_LancamentoPadrao_ContasContabeis_ContaContabilCodigo",
                table: "LancamentoPadrao");

            migrationBuilder.DropForeignKey(
                name: "FK_LancamentoPadrao_ContasContabeis_ContaContabilCodigo1",
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

            migrationBuilder.DropTable(
                name: "Descricao");

            migrationBuilder.DropIndex(
                name: "IX_OfxLancamentos_LancamentoPadraoId",
                table: "OfxLancamentos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LancamentoPadrao",
                table: "LancamentoPadrao");

            migrationBuilder.DropIndex(
                name: "IX_LancamentoPadrao_ContaContabilCodigo",
                table: "LancamentoPadrao");

            migrationBuilder.DropIndex(
                name: "IX_LancamentoPadrao_ContaContabilCodigo1",
                table: "LancamentoPadrao");

            migrationBuilder.DropColumn(
                name: "LancamentoPadraoId",
                table: "OfxLancamentos");

            migrationBuilder.DropColumn(
                name: "ContaContabilCodigo",
                table: "LancamentoPadrao");

            migrationBuilder.DropColumn(
                name: "ContaContabilCodigo1",
                table: "LancamentoPadrao");

            migrationBuilder.RenameTable(
                name: "LancamentoPadrao",
                newName: "Contas");

            migrationBuilder.RenameIndex(
                name: "IX_LancamentoPadrao_TipoContaId",
                table: "Contas",
                newName: "IX_Contas_TipoContaId");

            migrationBuilder.RenameIndex(
                name: "IX_LancamentoPadrao_CategoriaId",
                table: "Contas",
                newName: "IX_Contas_CategoriaId");

            migrationBuilder.AddColumn<int>(
                name: "LancamentoCredito",
                table: "Contas",
                type: "int",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LancamentoDebito",
                table: "Contas",
                type: "int",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contas",
                table: "Contas",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Contas_Categorias_CategoriaId",
                table: "Contas",
                column: "CategoriaId",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contas_TiposContas_TipoContaId",
                table: "Contas",
                column: "TipoContaId",
                principalTable: "TiposContas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Lancamentos_Contas_ContaId",
                table: "Lancamentos",
                column: "ContaId",
                principalTable: "Contas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
