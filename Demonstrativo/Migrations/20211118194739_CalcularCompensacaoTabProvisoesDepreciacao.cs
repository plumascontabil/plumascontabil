using Microsoft.EntityFrameworkCore.Migrations;

namespace Demonstrativo.Migrations
{
    public partial class CalcularCompensacaoTabProvisoesDepreciacao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CalcularCompensacao",
                table: "ProvisoesDepreciacoes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CalcularCompensacao",
                table: "ProvisoesDepreciacoes");
        }
    }
}
