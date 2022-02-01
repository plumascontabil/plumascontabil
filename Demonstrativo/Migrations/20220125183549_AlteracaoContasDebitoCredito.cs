using Microsoft.EntityFrameworkCore.Migrations;

namespace Demonstrativo.Migrations
{
    public partial class AlteracaoContasDebitoCredito : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ofxs_ContasContabeis_ContaContabilId",
                table: "Ofxs");

            migrationBuilder.DropIndex(
                name: "IX_Ofxs_ContaContabilId",
                table: "Ofxs");

            migrationBuilder.RenameColumn(
                name: "ContaContabilId",
                table: "Ofxs",
                newName: "ContaDebitolId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "ContaDebitolId",
                table: "Ofxs",
                newName: "ContaContabilId");

            migrationBuilder.CreateIndex(
                name: "IX_Ofxs_ContaContabilId",
                table: "Ofxs",
                column: "ContaContabilId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ofxs_ContasContabeis_ContaContabilId",
                table: "Ofxs",
                column: "ContaContabilId",
                principalTable: "ContasContabeis",
                principalColumn: "Codigo",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
