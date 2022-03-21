using Microsoft.EntityFrameworkCore.Migrations;

namespace Demonstrativo.Migrations
{
    public partial class pkAutoDescricoes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AutoDescricoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AutoDescricoes");
        }
    }
}
