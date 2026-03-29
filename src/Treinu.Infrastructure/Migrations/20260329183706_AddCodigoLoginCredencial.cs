using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Treinu.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCodigoLoginCredencial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodigoLogin",
                table: "Credenciais",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CodigoLoginExpiryTime",
                table: "Credenciais",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodigoLogin",
                table: "Credenciais");

            migrationBuilder.DropColumn(
                name: "CodigoLoginExpiryTime",
                table: "Credenciais");
        }
    }
}
