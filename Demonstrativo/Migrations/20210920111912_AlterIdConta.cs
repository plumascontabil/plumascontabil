using Microsoft.EntityFrameworkCore.Migrations;

namespace Demonstrativo.Migrations
{
    public partial class AlterIdConta : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lancamentos_Contas_ContaId",
                table: "Lancamentos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contas",
                table: "Contas");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Contas",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contas",
                table: "Contas",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lancamentos_Contas_ContaId",
                table: "Lancamentos",
                column: "ContaId",
                principalTable: "Contas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lancamentos_Contas_ContaId",
                table: "Lancamentos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contas",
                table: "Contas");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Contas");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contas",
                table: "Contas",
                column: "Codigo");

            migrationBuilder.AddForeignKey(
                name: "FK_Lancamentos_Contas_ContaId",
                table: "Lancamentos",
                column: "ContaId",
                principalTable: "Contas",
                principalColumn: "Codigo",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
