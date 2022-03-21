using Microsoft.EntityFrameworkCore.Migrations;

namespace Demonstrativo.Migrations
{
    public partial class DropContas2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LancamentoPadrao_ContasContabeis_ContaContabilCodigo",
                table: "LancamentoPadrao");

            migrationBuilder.DropForeignKey(
                name: "FK_LancamentoPadrao_ContasContabeis_ContaContabilCodigo1",
                table: "LancamentoPadrao");

            migrationBuilder.DropIndex(
                name: "IX_LancamentoPadrao_ContaContabilCodigo",
                table: "LancamentoPadrao");

            migrationBuilder.DropIndex(
                name: "IX_LancamentoPadrao_ContaContabilCodigo1",
                table: "LancamentoPadrao");

            migrationBuilder.DropColumn(
                name: "ContaContabilCodigo",
                table: "LancamentoPadrao");

            migrationBuilder.DropColumn(
                name: "ContaContabilCodigo1",
                table: "LancamentoPadrao");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ContaContabilCodigo",
                table: "LancamentoPadrao",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ContaContabilCodigo1",
                table: "LancamentoPadrao",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LancamentoPadrao_ContaContabilCodigo",
                table: "LancamentoPadrao",
                column: "ContaContabilCodigo");

            migrationBuilder.CreateIndex(
                name: "IX_LancamentoPadrao_ContaContabilCodigo1",
                table: "LancamentoPadrao",
                column: "ContaContabilCodigo1");

            migrationBuilder.AddForeignKey(
                name: "FK_LancamentoPadrao_ContasContabeis_ContaContabilCodigo",
                table: "LancamentoPadrao",
                column: "ContaContabilCodigo",
                principalTable: "ContasContabeis",
                principalColumn: "Codigo",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LancamentoPadrao_ContasContabeis_ContaContabilCodigo1",
                table: "LancamentoPadrao",
                column: "ContaContabilCodigo1",
                principalTable: "ContasContabeis",
                principalColumn: "Codigo",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
