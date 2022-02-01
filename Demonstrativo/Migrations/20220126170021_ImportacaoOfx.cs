using Microsoft.EntityFrameworkCore.Migrations;

namespace Demonstrativo.Migrations
{
    public partial class ImportacaoOfx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ofxs_ContasContabeis_ContaContabilCodigo",
                table: "Ofxs");

            migrationBuilder.DropIndex(
                name: "IX_Ofxs_ContaContabilCodigo",
                table: "Ofxs");

            migrationBuilder.DropColumn(
                name: "ContaContabilCodigo",
                table: "Ofxs");

            migrationBuilder.DropColumn(
                name: "ContaCreditolId",
                table: "Ofxs");

            migrationBuilder.DropColumn(
                name: "ContaDebitolId",
                table: "Ofxs");

            migrationBuilder.DropColumn(
                name: "HistoricoOfx",
                table: "Ofxs");

            migrationBuilder.AddColumn<int>(
                name: "HistoricoOfxId",
                table: "Ofxs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "HistoricosOfx",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContaDebitoId = table.Column<int>(type: "int", nullable: false),
                    ContaCreditoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoricosOfx", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoricosOfx_ContasContabeis_ContaCreditoId",
                        column: x => x.ContaCreditoId,
                        principalTable: "ContasContabeis",
                        principalColumn: "Codigo",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HistoricosOfx_ContasContabeis_ContaDebitoId",
                        column: x => x.ContaDebitoId,
                        principalTable: "ContasContabeis",
                        principalColumn: "Codigo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ofxs_HistoricoOfxId",
                table: "Ofxs",
                column: "HistoricoOfxId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricosOfx_ContaCreditoId",
                table: "HistoricosOfx",
                column: "ContaCreditoId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricosOfx_ContaDebitoId",
                table: "HistoricosOfx",
                column: "ContaDebitoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ofxs_HistoricosOfx_HistoricoOfxId",
                table: "Ofxs",
                column: "HistoricoOfxId",
                principalTable: "HistoricosOfx",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ofxs_HistoricosOfx_HistoricoOfxId",
                table: "Ofxs");

            migrationBuilder.DropTable(
                name: "HistoricosOfx");

            migrationBuilder.DropIndex(
                name: "IX_Ofxs_HistoricoOfxId",
                table: "Ofxs");

            migrationBuilder.DropColumn(
                name: "HistoricoOfxId",
                table: "Ofxs");

            migrationBuilder.AddColumn<int>(
                name: "ContaContabilCodigo",
                table: "Ofxs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ContaCreditolId",
                table: "Ofxs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ContaDebitolId",
                table: "Ofxs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HistoricoOfx",
                table: "Ofxs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ofxs_ContaContabilCodigo",
                table: "Ofxs",
                column: "ContaContabilCodigo");

            migrationBuilder.AddForeignKey(
                name: "FK_Ofxs_ContasContabeis_ContaContabilCodigo",
                table: "Ofxs",
                column: "ContaContabilCodigo",
                principalTable: "ContasContabeis",
                principalColumn: "Codigo",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
