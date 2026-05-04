using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Treinu.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddChatFeature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SalasChat",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    CriadorId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalasChat", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MensagensChat",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SalaChatId = table.Column<Guid>(type: "uuid", nullable: false),
                    RemetenteId = table.Column<Guid>(type: "uuid", nullable: false),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    Conteudo = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    DataEnvio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MensagensChat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MensagensChat_SalasChat_SalaChatId",
                        column: x => x.SalaChatId,
                        principalTable: "SalasChat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MensagensChat_Usuarios_RemetenteId",
                        column: x => x.RemetenteId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ParticipantesSala",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SalaChatId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    MensagensNaoLidas = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    IsAdmin = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DataEntrada = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParticipantesSala", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParticipantesSala_SalasChat_SalaChatId",
                        column: x => x.SalaChatId,
                        principalTable: "SalasChat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParticipantesSala_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MensagensChat_RemetenteId",
                table: "MensagensChat",
                column: "RemetenteId");

            migrationBuilder.CreateIndex(
                name: "IX_MensagensChat_SalaChatId",
                table: "MensagensChat",
                column: "SalaChatId");

            migrationBuilder.CreateIndex(
                name: "IX_ParticipantesSala_SalaChatId",
                table: "ParticipantesSala",
                column: "SalaChatId");

            migrationBuilder.CreateIndex(
                name: "IX_ParticipantesSala_UsuarioId",
                table: "ParticipantesSala",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MensagensChat");

            migrationBuilder.DropTable(
                name: "ParticipantesSala");

            migrationBuilder.DropTable(
                name: "SalasChat");
        }
    }
}
