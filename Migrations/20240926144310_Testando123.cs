using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransporteWeb.Migrations
{
    /// <inheritdoc />
    public partial class Testando123 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Adiciona a coluna IdPonto à tabela Agendamentos
            migrationBuilder.AddColumn<int>(
                name: "IdPonto",
                table: "Agendamentos",
                nullable: false,
                defaultValue: 0);

            // Cria a chave estrangeira para a tabela Pontos
            migrationBuilder.CreateIndex(
                name: "IX_Agendamentos_IdPonto",
                table: "Agendamentos",
                column: "IdPonto");

            migrationBuilder.AddForeignKey(
                name: "FK_Agendamentos_Pontos_IdPonto",
                table: "Agendamentos",
                column: "IdPonto",
                principalTable: "Pontos",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove a chave estrangeira e a coluna IdPonto se a migração for revertida
            migrationBuilder.DropForeignKey(
                name: "FK_Agendamentos_Pontos_IdPonto",
                table: "Agendamentos");

            migrationBuilder.DropIndex(
                name: "IX_Agendamentos_IdPonto",
                table: "Agendamentos");

            migrationBuilder.DropColumn(
                name: "IdPonto",
                table: "Agendamentos");
        }
    }
}
