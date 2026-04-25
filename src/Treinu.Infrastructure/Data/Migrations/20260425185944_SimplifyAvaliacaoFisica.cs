using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Treinu.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class SimplifyAvaliacaoFisica : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medidas_AvaliacoesFisicas_QuestionarioId",
                table: "Medidas");

            migrationBuilder.DropColumn(
                name: "Arquivo",
                table: "AvaliacoesFisicas");

            migrationBuilder.DropColumn(
                name: "Nome",
                table: "AvaliacoesFisicas");

            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "AvaliacoesFisicas");

            migrationBuilder.RenameColumn(
                name: "QuestionarioId",
                table: "Medidas",
                newName: "AvaliacaoFisicaId");

            migrationBuilder.RenameIndex(
                name: "IX_Medidas_QuestionarioId",
                table: "Medidas",
                newName: "IX_Medidas_AvaliacaoFisicaId");

            migrationBuilder.AlterColumn<double>(
                name: "Peso",
                table: "AvaliacoesFisicas",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Imc",
                table: "AvaliacoesFisicas",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Classificacao",
                table: "AvaliacoesFisicas",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Altura",
                table: "AvaliacoesFisicas",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Medidas_AvaliacoesFisicas_AvaliacaoFisicaId",
                table: "Medidas",
                column: "AvaliacaoFisicaId",
                principalTable: "AvaliacoesFisicas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medidas_AvaliacoesFisicas_AvaliacaoFisicaId",
                table: "Medidas");

            migrationBuilder.RenameColumn(
                name: "AvaliacaoFisicaId",
                table: "Medidas",
                newName: "QuestionarioId");

            migrationBuilder.RenameIndex(
                name: "IX_Medidas_AvaliacaoFisicaId",
                table: "Medidas",
                newName: "IX_Medidas_QuestionarioId");

            migrationBuilder.AlterColumn<double>(
                name: "Peso",
                table: "AvaliacoesFisicas",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<double>(
                name: "Imc",
                table: "AvaliacoesFisicas",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<string>(
                name: "Classificacao",
                table: "AvaliacoesFisicas",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<double>(
                name: "Altura",
                table: "AvaliacoesFisicas",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AddColumn<string>(
                name: "Arquivo",
                table: "AvaliacoesFisicas",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nome",
                table: "AvaliacoesFisicas",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tipo",
                table: "AvaliacoesFisicas",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Medidas_AvaliacoesFisicas_QuestionarioId",
                table: "Medidas",
                column: "QuestionarioId",
                principalTable: "AvaliacoesFisicas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
