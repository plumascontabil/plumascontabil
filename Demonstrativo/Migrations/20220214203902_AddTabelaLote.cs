using Microsoft.EntityFrameworkCore.Migrations;

namespace Demonstrativo.Migrations
{
    public partial class AddTabelaLote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Descricao",
                table: "Ofxs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(150)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Complemento",
                table: "Ofxs",
                type: "varchar(150)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Lotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Descricao = table.Column<string>(type: "varchar(70)", nullable: false),
                    LancamentoOfxId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lotes_Ofxs_LancamentoOfxId",
                        column: x => x.LancamentoOfxId,
                        principalTable: "Ofxs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lotes_LancamentoOfxId",
                table: "Lotes",
                column: "LancamentoOfxId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Lotes");

            migrationBuilder.DropColumn(
                name: "Complemento",
                table: "Ofxs");

            migrationBuilder.AlterColumn<string>(
                name: "Descricao",
                table: "Ofxs",
                type: "varchar(150)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
