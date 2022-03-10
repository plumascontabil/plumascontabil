using Microsoft.EntityFrameworkCore.Migrations;

namespace Demonstrativo.Migrations
{
    public partial class CriandoTabelaComplementoETabelaSaldo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComplementosOfxs_LancamentoOfx_OfxId",
                table: "ComplementosOfxs");

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

            migrationBuilder.DropForeignKey(
                name: "FK_SaldoConta_ContasContabeis_ContaContabilId",
                table: "SaldoConta");

            migrationBuilder.DropForeignKey(
                name: "FK_SaldoConta_Empresas_EmpresaId",
                table: "SaldoConta");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SaldoConta",
                table: "SaldoConta");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LancamentoOfx",
                table: "LancamentoOfx");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContaCorrente",
                table: "ContaCorrente");

            migrationBuilder.RenameTable(
                name: "SaldoConta",
                newName: "SaldoContas");

            migrationBuilder.RenameTable(
                name: "LancamentoOfx",
                newName: "Ofxs");

            migrationBuilder.RenameTable(
                name: "ContaCorrente",
                newName: "ConstasCorrentes");

            migrationBuilder.RenameIndex(
                name: "IX_SaldoConta_EmpresaId",
                table: "SaldoContas",
                newName: "IX_SaldoContas_EmpresaId");

            migrationBuilder.RenameIndex(
                name: "IX_SaldoConta_ContaContabilId",
                table: "SaldoContas",
                newName: "IX_SaldoContas_ContaContabilId");

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

            migrationBuilder.AddPrimaryKey(
                name: "PK_SaldoContas",
                table: "SaldoContas",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ofxs",
                table: "Ofxs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConstasCorrentes",
                table: "ConstasCorrentes",
                column: "Id");

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
                name: "FK_SaldoContas_ContasContabeis_ContaContabilId",
                table: "SaldoContas",
                column: "ContaContabilId",
                principalTable: "ContasContabeis",
                principalColumn: "Codigo",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SaldoContas_Empresas_EmpresaId",
                table: "SaldoContas",
                column: "EmpresaId",
                principalTable: "Empresas",
                principalColumn: "Codigo",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComplementosOfxs_Ofxs_OfxId",
                table: "ComplementosOfxs");

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
                name: "FK_SaldoContas_ContasContabeis_ContaContabilId",
                table: "SaldoContas");

            migrationBuilder.DropForeignKey(
                name: "FK_SaldoContas_Empresas_EmpresaId",
                table: "SaldoContas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SaldoContas",
                table: "SaldoContas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ofxs",
                table: "Ofxs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ConstasCorrentes",
                table: "ConstasCorrentes");

            migrationBuilder.RenameTable(
                name: "SaldoContas",
                newName: "SaldoConta");

            migrationBuilder.RenameTable(
                name: "Ofxs",
                newName: "LancamentoOfx");

            migrationBuilder.RenameTable(
                name: "ConstasCorrentes",
                newName: "ContaCorrente");

            migrationBuilder.RenameIndex(
                name: "IX_SaldoContas_EmpresaId",
                table: "SaldoConta",
                newName: "IX_SaldoConta_EmpresaId");

            migrationBuilder.RenameIndex(
                name: "IX_SaldoContas_ContaContabilId",
                table: "SaldoConta",
                newName: "IX_SaldoConta_ContaContabilId");

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
                name: "PK_SaldoConta",
                table: "SaldoConta",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LancamentoOfx",
                table: "LancamentoOfx",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContaCorrente",
                table: "ContaCorrente",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ComplementosOfxs_LancamentoOfx_OfxId",
                table: "ComplementosOfxs",
                column: "OfxId",
                principalTable: "LancamentoOfx",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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

            migrationBuilder.AddForeignKey(
                name: "FK_SaldoConta_ContasContabeis_ContaContabilId",
                table: "SaldoConta",
                column: "ContaContabilId",
                principalTable: "ContasContabeis",
                principalColumn: "Codigo",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SaldoConta_Empresas_EmpresaId",
                table: "SaldoConta",
                column: "EmpresaId",
                principalTable: "Empresas",
                principalColumn: "Codigo",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
