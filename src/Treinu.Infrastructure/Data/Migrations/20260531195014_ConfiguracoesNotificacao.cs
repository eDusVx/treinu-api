using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Treinu.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ConfiguracoesNotificacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataProximaAvaliacao",
                table: "AvaliacoesFisicas",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ConfiguracoesNotificacao",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReceberEmail = table.Column<bool>(type: "boolean", nullable: false),
                    ReceberPush = table.Column<bool>(type: "boolean", nullable: false),
                    AlertaVencimentoAvaliacao = table.Column<bool>(type: "boolean", nullable: false),
                    AlertaVencimentoTreino = table.Column<bool>(type: "boolean", nullable: false),
                    AlertaNovoTreino = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguracoesNotificacao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConfiguracoesNotificacao_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracoesNotificacao_UsuarioId",
                table: "ConfiguracoesNotificacao",
                column: "UsuarioId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfiguracoesNotificacao");

            migrationBuilder.DropColumn(
                name: "DataProximaAvaliacao",
                table: "AvaliacoesFisicas");
        }
    }
}
