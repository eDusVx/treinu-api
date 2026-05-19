using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Treinu.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTreinadorIdToAluno : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TreinadorId",
                table: "Usuarios",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_TreinadorId",
                table: "Usuarios",
                column: "TreinadorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Usuarios_TreinadorId",
                table: "Usuarios",
                column: "TreinadorId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Usuarios_TreinadorId",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_TreinadorId",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "TreinadorId",
                table: "Usuarios");
        }
    }
}
