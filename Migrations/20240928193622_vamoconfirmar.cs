using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransporteWeb.Migrations
{
    /// <inheritdoc />
    public partial class vamoconfirmar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConfirmacaoPresencas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdAgendamento = table.Column<int>(type: "int", nullable: false),
                    PresencaConfirmada = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfirmacaoPresencas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConfirmacaoPresencas_Agendamentos_IdAgendamento",
                        column: x => x.IdAgendamento,
                        principalTable: "Agendamentos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConfirmacaoPresencas_IdAgendamento",
                table: "ConfirmacaoPresencas",
                column: "IdAgendamento");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfirmacaoPresencas");
        }
    }
}
