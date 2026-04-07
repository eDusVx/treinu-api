using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Treinu.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddConviteTemplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TemplatesEmail",
                columns: new[] { "Id", "AssuntoPadrao", "ConteudoHtml", "Nome" },
                values: new object[] { new Guid("55555555-5555-5555-5555-555555555555"), "Convite para o Treinu App", "<!DOCTYPE html><html lang=\"pt-BR\"><head><meta charset=\"UTF-8\"><meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"><title>Convite para o Treinu App</title><style>body { font-family: 'Inter', 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #1E1E1E; color: #FFFFFF; margin: 0; padding: 0; } .container { width: 100%; max-width: 600px; margin: 0 auto; background-color: #2D2D2D; border-radius: 12px; overflow: hidden; margin-top: 40px; margin-bottom: 40px; box-shadow: 0 4px 15px rgba(0, 0, 0, 0.3); } .header { background-color: #A3F51D; padding: 40px 20px; text-align: center; color: #000000; } .header h1 { margin: 0; font-size: 28px; font-weight: 800; letter-spacing: -0.5px; } .content { padding: 40px 30px; text-align: center; } .content h2 { color: #FFFFFF; font-size: 24px; margin-top: 0; } .content p { font-size: 16px; line-height: 1.6; color: #E0E0E0; margin-bottom: 25px; } .btn { display: inline-block; background-color: #A3F51D; color: #000000; text-decoration: none; padding: 15px 35px; font-size: 16px; font-weight: bold; border-radius: 50px; margin: 20px 0; transition: background-color 0.3s; } .btn:hover { background-color: #8cde16; } .footer { background-color: #1E1E1E; padding: 20px; text-align: center; font-size: 14px; color: #888888; border-top: 1px solid #333333; } .footer p { margin: 5px 0; }</style></head><body><div class=\"container\"><div class=\"header\"><div style=\"font-size: 40px; font-weight: bold; margin-bottom: 10px;\">⬢</div><h1>Você foi convidado!</h1></div><div class=\"content\"><h2>Olá!</h2><p>Você foi convidado para se registrar como <strong>{{Perfil}}</strong> no <strong>Treinu</strong>.</p><p>Clique no botão abaixo para completar o seu cadastro usando o token seguro contido no link:</p><a href=\"{{LinkConvite}}\" class=\"btn\">Completar Meu Cadastro</a><p style=\"font-size: 14px; color: #AAAAAA; margin-top: 30px;\">Atenção: Este convite expirará em 48 horas.</p></div><div class=\"footer\"><p>© 2026 Treinu App. Todos os direitos reservados.</p><p>O seu app definitivo para gerenciar treinos.</p></div></div></body></html>", "ConviteTreinador" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TemplatesEmail",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"));
        }
    }
}
