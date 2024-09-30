using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransporteWeb.Migrations
{
    /// <inheritdoc />
    public partial class Trocaidaevolta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "horario",
                table: "Agendamentos");

            migrationBuilder.AddColumn<int>(
                name: "TipoViagem",
                table: "Agendamentos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoViagem",
                table: "Agendamentos");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "horario",
                table: "Agendamentos",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }
    }
}
