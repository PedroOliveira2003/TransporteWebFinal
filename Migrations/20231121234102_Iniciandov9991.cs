using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransporteWeb.Migrations
{
    /// <inheritdoc />
    public partial class Iniciandov9991 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estudantes_Cursos_IdCurso",
                table: "Estudantes");

            migrationBuilder.DropColumn(
                name: "IdVeiculo",
                table: "Estudantes");

            migrationBuilder.AlterColumn<int>(
                name: "IdCurso",
                table: "Estudantes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Estudantes_Cursos_IdCurso",
                table: "Estudantes",
                column: "IdCurso",
                principalTable: "Cursos",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estudantes_Cursos_IdCurso",
                table: "Estudantes");

            migrationBuilder.AlterColumn<int>(
                name: "IdCurso",
                table: "Estudantes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "IdVeiculo",
                table: "Estudantes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Estudantes_Cursos_IdCurso",
                table: "Estudantes",
                column: "IdCurso",
                principalTable: "Cursos",
                principalColumn: "id");
        }
    }
}
