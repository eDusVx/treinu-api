using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Treinu.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCredenciaisTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Credenciais",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Senha = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Provider = table.Column<string>(type: "varchar(30)", nullable: false),
                    TipoUsuario = table.Column<string>(type: "varchar(30)", nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Credenciais", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Credenciais_Email",
                table: "Credenciais",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Credenciais");
        }
    }
}
