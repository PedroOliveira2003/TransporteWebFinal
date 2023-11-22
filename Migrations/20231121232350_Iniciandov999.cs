using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransporteWeb.Migrations
{
    /// <inheritdoc />
    public partial class Iniciandov999 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdCurso",
                table: "Estudantes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdVeiculo",
                table: "Estudantes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Estudantes_IdCurso",
                table: "Estudantes",
                column: "IdCurso");

            migrationBuilder.AddForeignKey(
                name: "FK_Estudantes_Cursos_IdCurso",
                table: "Estudantes",
                column: "IdCurso",
                principalTable: "Cursos",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estudantes_Cursos_IdCurso",
                table: "Estudantes");

            migrationBuilder.DropIndex(
                name: "IX_Estudantes_IdCurso",
                table: "Estudantes");

            migrationBuilder.DropColumn(
                name: "IdCurso",
                table: "Estudantes");

            migrationBuilder.DropColumn(
                name: "IdVeiculo",
                table: "Estudantes");
        }
    }
}
