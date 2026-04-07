using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Treinu.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAprovacaoTemplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TemplatesEmail",
                columns: new[] { "Id", "AssuntoPadrao", "ConteudoHtml", "Nome" },
                values: new object[] { new Guid("44444444-4444-4444-4444-444444444444"), "Cadastro Aprovado - Treinu!", "<!DOCTYPE html><html lang=\"pt-BR\"><head><meta charset=\"UTF-8\"><meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"><title>Cadastro Aprovado - Treinu</title><style>body { font-family: 'Inter', 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #1E1E1E; color: #FFFFFF; margin: 0; padding: 0; } .container { width: 100%; max-width: 600px; margin: 0 auto; background-color: #2D2D2D; border-radius: 12px; overflow: hidden; margin-top: 40px; margin-bottom: 40px; box-shadow: 0 4px 15px rgba(0, 0, 0, 0.3); } .header { background-color: #A3F51D; padding: 40px 20px; text-align: center; color: #000000; } .header h1 { margin: 0; font-size: 28px; font-weight: 800; letter-spacing: -0.5px; } .content { padding: 40px 30px; text-align: center; } .content h2 { color: #FFFFFF; font-size: 24px; margin-top: 0; } .content p { font-size: 16px; line-height: 1.6; color: #E0E0E0; margin-bottom: 25px; } .btn { display: inline-block; background-color: #A3F51D; color: #000000; text-decoration: none; padding: 15px 35px; font-size: 16px; font-weight: bold; border-radius: 50px; margin: 20px 0; transition: background-color 0.3s; } .btn:hover { background-color: #8cde16; } .features { background-color: #242424; border-radius: 8px; padding: 20px; margin-top: 30px; text-align: left; } .features ul { list-style-type: none; padding: 0; margin: 0; } .features li { margin-bottom: 10px; padding-left: 25px; position: relative; color: #CCCCCC; } .features li::before { content: '✓'; color: #A3F51D; position: absolute; left: 0; font-weight: bold; } .footer { background-color: #1E1E1E; padding: 20px; text-align: center; font-size: 14px; color: #888888; border-top: 1px solid #333333; } .footer p { margin: 5px 0; }</style></head><body><div class=\"container\"><div class=\"header\"><div style=\"font-size: 40px; font-weight: bold; margin-bottom: 10px;\">⬢</div><h1>Cadastro Aprovado!</h1></div><div class=\"content\"><h2>Olá, {{Nome}}!</h2><p>Temos uma ótima notícia: o seu cadastro no <strong>Treinu</strong> foi <strong>aprovado</strong> com sucesso!</p><p>Você já tem acesso completo ao nosso sistema para começar a gerenciar seus alunos, prescrever treinos e acompanhar os resultados.</p><a href=\"{{LinkLogin}}\" class=\"btn\">Acessar a Plataforma</a><div class=\"features\"><p style=\"color: #FFFFFF; font-weight: bold; margin-bottom: 15px; text-align: center;\">Próximos passos:</p><ul><li>Cadastre seus primeiros alunos;</li><li>Crie avaliações físicas e metas;</li><li>Monte as rotinas e tabelas de treino;</li></ul></div></div><div class=\"footer\"><p>© 2026 Treinu App. Todos os direitos reservados.</p><p>Comece agora a transformar a vida dos seus alunos!</p></div></div></body></html>", "TreinadorAprovado" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TemplatesEmail",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"));
        }
    }
}
