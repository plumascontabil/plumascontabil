using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Demonstrativo.Migrations
{
    public partial class CreateOfxs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContaCorrente_BancoOfx_BancoOfxId",
                table: "ContaCorrente");

            migrationBuilder.DropForeignKey(
                name: "FK_ContaCorrente_Empresas_EmpresaId",
                table: "ContaCorrente");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContaCorrente",
                table: "ContaCorrente");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BancoOfx",
                table: "BancoOfx");

            migrationBuilder.RenameTable(
                name: "ContaCorrente",
                newName: "ConstasCorrentes");

            migrationBuilder.RenameTable(
                name: "BancoOfx",
                newName: "BancoOfxs");

            migrationBuilder.RenameIndex(
                name: "IX_ContaCorrente_EmpresaId",
                table: "ConstasCorrentes",
                newName: "IX_ConstasCorrentes_EmpresaId");

            migrationBuilder.RenameIndex(
                name: "IX_ContaCorrente_BancoOfxId",
                table: "ConstasCorrentes",
                newName: "IX_ConstasCorrentes_BancoOfxId");

            migrationBuilder.AlterColumn<string>(
                name: "NumeroConta",
                table: "ConstasCorrentes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConstasCorrentes",
                table: "ConstasCorrentes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BancoOfxs",
                table: "BancoOfxs",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Ofxs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Documento = table.Column<string>(type: "varchar(20)", nullable: true),
                    TipoLancamento = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descricao = table.Column<string>(type: "varchar(150)", nullable: true),
                    ValorOfx = table.Column<decimal>(type: "decimal(11,2)", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoricoOfxId = table.Column<int>(type: "int", nullable: false),
                    ContaCorrenteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ofxs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ofxs_ConstasCorrentes_ContaCorrenteId",
                        column: x => x.ContaCorrenteId,
                        principalTable: "ConstasCorrentes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ofxs_HistoricosOfx_HistoricoOfxId",
                        column: x => x.HistoricoOfxId,
                        principalTable: "HistoricosOfx",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ofxs_ContaCorrenteId",
                table: "Ofxs",
                column: "ContaCorrenteId");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConstasCorrentes_BancoOfxs_BancoOfxId",
                table: "ConstasCorrentes");

            migrationBuilder.DropForeignKey(
                name: "FK_ConstasCorrentes_Empresas_EmpresaId",
                table: "ConstasCorrentes");

            migrationBuilder.DropTable(
                name: "Ofxs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ConstasCorrentes",
                table: "ConstasCorrentes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BancoOfxs",
                table: "BancoOfxs");

            migrationBuilder.RenameTable(
                name: "ConstasCorrentes",
                newName: "ContaCorrente");

            migrationBuilder.RenameTable(
                name: "BancoOfxs",
                newName: "BancoOfx");

            migrationBuilder.RenameIndex(
                name: "IX_ConstasCorrentes_EmpresaId",
                table: "ContaCorrente",
                newName: "IX_ContaCorrente_EmpresaId");

            migrationBuilder.RenameIndex(
                name: "IX_ConstasCorrentes_BancoOfxId",
                table: "ContaCorrente",
                newName: "IX_ContaCorrente_BancoOfxId");

            migrationBuilder.AlterColumn<int>(
                name: "NumeroConta",
                table: "ContaCorrente",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContaCorrente",
                table: "ContaCorrente",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BancoOfx",
                table: "BancoOfx",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ContaCorrente_BancoOfx_BancoOfxId",
                table: "ContaCorrente",
                column: "BancoOfxId",
                principalTable: "BancoOfx",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContaCorrente_Empresas_EmpresaId",
                table: "ContaCorrente",
                column: "EmpresaId",
                principalTable: "Empresas",
                principalColumn: "Codigo",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
