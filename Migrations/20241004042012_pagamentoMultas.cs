using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransporteWeb.Migrations
{
    /// <inheritdoc />
    public partial class pagamentoMultas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PagamentosMulta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdEstudante = table.Column<int>(type: "int", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DataPagamento = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PagamentosMulta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PagamentosMulta_Estudantes_IdEstudante",
                        column: x => x.IdEstudante,
                        principalTable: "Estudantes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PagamentosMulta_IdEstudante",
                table: "PagamentosMulta",
                column: "IdEstudante");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PagamentosMulta");
        }
    }
}
