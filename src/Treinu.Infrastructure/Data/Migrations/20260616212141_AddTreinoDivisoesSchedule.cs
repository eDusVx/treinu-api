using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Treinu.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTreinoDivisoesSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DivisaoDomingo",
                table: "Treinos",
                type: "character varying(1)",
                maxLength: 1,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DivisaoQuarta",
                table: "Treinos",
                type: "character varying(1)",
                maxLength: 1,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DivisaoQuinta",
                table: "Treinos",
                type: "character varying(1)",
                maxLength: 1,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DivisaoSabado",
                table: "Treinos",
                type: "character varying(1)",
                maxLength: 1,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DivisaoSegunda",
                table: "Treinos",
                type: "character varying(1)",
                maxLength: 1,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DivisaoSexta",
                table: "Treinos",
                type: "character varying(1)",
                maxLength: 1,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DivisaoTerca",
                table: "Treinos",
                type: "character varying(1)",
                maxLength: 1,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NomeDivisaoA",
                table: "Treinos",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NomeDivisaoB",
                table: "Treinos",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NomeDivisaoC",
                table: "Treinos",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NomeDivisaoD",
                table: "Treinos",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Divisao",
                table: "ItensTreino",
                type: "character varying(1)",
                maxLength: 1,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DivisaoDomingo",
                table: "Treinos");

            migrationBuilder.DropColumn(
                name: "DivisaoQuarta",
                table: "Treinos");

            migrationBuilder.DropColumn(
                name: "DivisaoQuinta",
                table: "Treinos");

            migrationBuilder.DropColumn(
                name: "DivisaoSabado",
                table: "Treinos");

            migrationBuilder.DropColumn(
                name: "DivisaoSegunda",
                table: "Treinos");

            migrationBuilder.DropColumn(
                name: "DivisaoSexta",
                table: "Treinos");

            migrationBuilder.DropColumn(
                name: "DivisaoTerca",
                table: "Treinos");

            migrationBuilder.DropColumn(
                name: "NomeDivisaoA",
                table: "Treinos");

            migrationBuilder.DropColumn(
                name: "NomeDivisaoB",
                table: "Treinos");

            migrationBuilder.DropColumn(
                name: "NomeDivisaoC",
                table: "Treinos");

            migrationBuilder.DropColumn(
                name: "NomeDivisaoD",
                table: "Treinos");

            migrationBuilder.DropColumn(
                name: "Divisao",
                table: "ItensTreino");
        }
    }
}
