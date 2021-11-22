using Microsoft.EntityFrameworkCore.Migrations;

namespace Demonstrativo.Migrations
{
    public partial class CorrecaoTabProvisoesDepreciacao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "SaldoPrejuizo",
                table: "ProvisoesDepreciacoes",
                type: "decimal(11,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SaldoPrejuizo",
                table: "ProvisoesDepreciacoes");
        }
    }
}
