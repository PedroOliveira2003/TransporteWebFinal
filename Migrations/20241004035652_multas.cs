using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransporteWeb.Migrations
{
    /// <inheritdoc />
    public partial class multas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Multa",
                table: "Estudantes",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Multa",
                table: "Estudantes");
        }
    }
}
