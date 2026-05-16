using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Treinu.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNewEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AvaliacoesPlataforma",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    Nota = table.Column<int>(type: "integer", nullable: false),
                    Comentario = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvaliacoesPlataforma", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AvaliacoesPlataforma_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExecucoesTreino",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TreinoId = table.Column<Guid>(type: "uuid", nullable: false),
                    AlunoId = table.Column<Guid>(type: "uuid", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataFim = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Concluido = table.Column<bool>(type: "boolean", nullable: false),
                    NotaFeedback = table.Column<int>(type: "integer", nullable: true),
                    ComentarioFeedback = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExecucoesTreino", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sugestoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    Titulo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Descricao = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Lido = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sugestoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sugestoes_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExecucoesExercicio",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ExecucaoTreinoId = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemTreinoId = table.Column<Guid>(type: "uuid", nullable: false),
                    SeriesRealizadas = table.Column<int>(type: "integer", nullable: false),
                    RepeticoesRealizadas = table.Column<int>(type: "integer", nullable: false),
                    CargaUtilizada = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExecucoesExercicio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExecucoesExercicio_ExecucoesTreino_ExecucaoTreinoId",
                        column: x => x.ExecucaoTreinoId,
                        principalTable: "ExecucoesTreino",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AvaliacoesPlataforma_UsuarioId",
                table: "AvaliacoesPlataforma",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_ExecucoesExercicio_ExecucaoTreinoId",
                table: "ExecucoesExercicio",
                column: "ExecucaoTreinoId");

            migrationBuilder.CreateIndex(
                name: "IX_Sugestoes_UsuarioId",
                table: "Sugestoes",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AvaliacoesPlataforma");

            migrationBuilder.DropTable(
                name: "ExecucoesExercicio");

            migrationBuilder.DropTable(
                name: "Sugestoes");

            migrationBuilder.DropTable(
                name: "ExecucoesTreino");
        }
    }
}
