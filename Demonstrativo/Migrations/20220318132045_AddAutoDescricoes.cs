using Microsoft.EntityFrameworkCore.Migrations;

namespace Demonstrativo.Migrations
{
    public partial class AddAutoDescricoes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConstasCorrentes_Empresas_EmpresaId",
                table: "ConstasCorrentes");

            migrationBuilder.DropForeignKey(
                name: "FK_ConstasCorrentes_OfxBancos_BancoOfxId",
                table: "ConstasCorrentes");

            migrationBuilder.DropForeignKey(
                name: "FK_OfxLancamentos_ConstasCorrentes_ContaCorrenteId",
                table: "OfxLancamentos");

            migrationBuilder.DropTable(
                name: "Descricao");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ConstasCorrentes",
                table: "ConstasCorrentes");

            migrationBuilder.RenameTable(
                name: "ConstasCorrentes",
                newName: "ContasCorrentes");

            migrationBuilder.RenameIndex(
                name: "IX_ConstasCorrentes_EmpresaId",
                table: "ContasCorrentes",
                newName: "IX_ContasCorrentes_EmpresaId");

            migrationBuilder.RenameIndex(
                name: "IX_ConstasCorrentes_BancoOfxId",
                table: "ContasCorrentes",
                newName: "IX_ContasCorrentes_BancoOfxId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContasCorrentes",
                table: "ContasCorrentes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AutoDescricoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Descricao = table.Column<string>(type: "varchar(1000)", nullable: true),
                    LancamentoPadraoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutoDescricoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AutoDescricoes_LancamentosPadroes_LancamentoPadraoId",
                        column: x => x.LancamentoPadraoId,
                        principalTable: "LancamentosPadroes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AutoDescricoes_LancamentoPadraoId",
                table: "AutoDescricoes",
                column: "LancamentoPadraoId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContasCorrentes_Empresas_EmpresaId",
                table: "ContasCorrentes",
                column: "EmpresaId",
                principalTable: "Empresas",
                principalColumn: "Codigo",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContasCorrentes_OfxBancos_BancoOfxId",
                table: "ContasCorrentes",
                column: "BancoOfxId",
                principalTable: "OfxBancos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OfxLancamentos_ContasCorrentes_ContaCorrenteId",
                table: "OfxLancamentos",
                column: "ContaCorrenteId",
                principalTable: "ContasCorrentes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContasCorrentes_Empresas_EmpresaId",
                table: "ContasCorrentes");

            migrationBuilder.DropForeignKey(
                name: "FK_ContasCorrentes_OfxBancos_BancoOfxId",
                table: "ContasCorrentes");

            migrationBuilder.DropForeignKey(
                name: "FK_OfxLancamentos_ContasCorrentes_ContaCorrenteId",
                table: "OfxLancamentos");

            migrationBuilder.DropTable(
                name: "AutoDescricoes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContasCorrentes",
                table: "ContasCorrentes");

            migrationBuilder.RenameTable(
                name: "ContasCorrentes",
                newName: "ConstasCorrentes");

            migrationBuilder.RenameIndex(
                name: "IX_ContasCorrentes_EmpresaId",
                table: "ConstasCorrentes",
                newName: "IX_ConstasCorrentes_EmpresaId");

            migrationBuilder.RenameIndex(
                name: "IX_ContasCorrentes_BancoOfxId",
                table: "ConstasCorrentes",
                newName: "IX_ConstasCorrentes_BancoOfxId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConstasCorrentes",
                table: "ConstasCorrentes",
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
                name: "IX_Descricao_VendaId",
                table: "Descricao",
                column: "VendaId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConstasCorrentes_Empresas_EmpresaId",
                table: "ConstasCorrentes",
                column: "EmpresaId",
                principalTable: "Empresas",
                principalColumn: "Codigo",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ConstasCorrentes_OfxBancos_BancoOfxId",
                table: "ConstasCorrentes",
                column: "BancoOfxId",
                principalTable: "OfxBancos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OfxLancamentos_ConstasCorrentes_ContaCorrenteId",
                table: "OfxLancamentos",
                column: "ContaCorrenteId",
                principalTable: "ConstasCorrentes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
