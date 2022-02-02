using Microsoft.EntityFrameworkCore.Migrations;

namespace Demonstrativo.Migrations
{
    public partial class FixFKEmpresasImportacoesOfxs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmpresaId",
                table: "Ofxs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Ofxs_EmpresaId",
                table: "Ofxs",
                column: "EmpresaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ofxs_Empresas_EmpresaId",
                table: "Ofxs",
                column: "EmpresaId",
                principalTable: "Empresas",
                principalColumn: "Codigo",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ofxs_Empresas_EmpresaId",
                table: "Ofxs");

            migrationBuilder.DropIndex(
                name: "IX_Ofxs_EmpresaId",
                table: "Ofxs");

            migrationBuilder.DropColumn(
                name: "EmpresaId",
                table: "Ofxs");
        }
    }
}
