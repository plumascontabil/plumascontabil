using Microsoft.EntityFrameworkCore.Migrations;

namespace Demonstrativo.Migrations
{
    public partial class TipoConta : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TipoContaId",
                table: "Contas",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TiposContas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposContas", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contas_TipoContaId",
                table: "Contas",
                column: "TipoContaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contas_TiposContas_TipoContaId",
                table: "Contas",
                column: "TipoContaId",
                principalTable: "TiposContas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contas_TiposContas_TipoContaId",
                table: "Contas");

            migrationBuilder.DropTable(
                name: "TiposContas");

            migrationBuilder.DropIndex(
                name: "IX_Contas_TipoContaId",
                table: "Contas");

            migrationBuilder.DropColumn(
                name: "TipoContaId",
                table: "Contas");
        }
    }
}
