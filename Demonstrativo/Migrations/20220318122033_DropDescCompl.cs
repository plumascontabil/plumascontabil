using Microsoft.EntityFrameworkCore.Migrations;

namespace Demonstrativo.Migrations
{
    public partial class DropDescCompl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OfxComplemento");

            migrationBuilder.DropTable(
                name: "OfxDescricao");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OfxDescricao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContaCreditoId = table.Column<int>(type: "int", nullable: false),
                    ContaDebitoId = table.Column<int>(type: "int", nullable: false),
                    Descricao = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfxDescricao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfxDescricao_ContasContabeis_ContaCreditoId",
                        column: x => x.ContaCreditoId,
                        principalTable: "ContasContabeis",
                        principalColumn: "Codigo",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OfxDescricao_ContasContabeis_ContaDebitoId",
                        column: x => x.ContaDebitoId,
                        principalTable: "ContasContabeis",
                        principalColumn: "Codigo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OfxComplemento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DescricaoComplemento = table.Column<string>(type: "varchar(70)", nullable: true),
                    HistoricoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfxComplemento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfxComplemento_OfxDescricao_HistoricoId",
                        column: x => x.HistoricoId,
                        principalTable: "OfxDescricao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OfxComplemento_HistoricoId",
                table: "OfxComplemento",
                column: "HistoricoId");

            migrationBuilder.CreateIndex(
                name: "IX_OfxDescricao_ContaCreditoId",
                table: "OfxDescricao",
                column: "ContaCreditoId");

            migrationBuilder.CreateIndex(
                name: "IX_OfxDescricao_ContaDebitoId",
                table: "OfxDescricao",
                column: "ContaDebitoId");
        }
    }
}
