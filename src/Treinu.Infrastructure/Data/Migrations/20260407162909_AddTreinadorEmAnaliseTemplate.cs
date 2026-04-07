using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Treinu.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTreinadorEmAnaliseTemplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "TemplatesEmail",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "ConteudoHtml",
                value: "<!DOCTYPE html><html lang=\"pt-BR\"><head><meta charset=\"UTF-8\"><meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"><title>Recuperação de Senha - Treinu</title><style>body { font-family: 'Inter', 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #1E1E1E; color: #FFFFFF; margin: 0; padding: 0; } .container { width: 100%; max-width: 600px; margin: 0 auto; background-color: #2D2D2D; border-radius: 12px; overflow: hidden; margin-top: 40px; margin-bottom: 40px; box-shadow: 0 4px 15px rgba(0, 0, 0, 0.3); } .header { background-color: #A3F51D; padding: 40px 20px; text-align: center; color: #000000; } .header h1 { margin: 0; font-size: 28px; font-weight: 800; letter-spacing: -0.5px; } .content { padding: 40px 30px; text-align: center; } .content p { font-size: 16px; line-height: 1.6; color: #E0E0E0; margin-bottom: 25px; } .code-box { background-color: #1E1E1E; border: 2px dashed #A3F51D; border-radius: 8px; padding: 20px; font-size: 32px; font-weight: bold; color: #A3F51D; letter-spacing: 4px; margin: 30px 0; display: inline-block; } .footer { background-color: #1E1E1E; padding: 20px; text-align: center; font-size: 14px; color: #888888; border-top: 1px solid #333333; } .footer p { margin: 5px 0; }</style></head><body><div class=\"container\"><div class=\"header\"><div style=\"font-size: 40px; font-weight: bold; margin-bottom: 10px;\">⬢</div><h1>Recuperação de Senha</h1></div><div class=\"content\"><p>Olá, {{Nome}}!</p><p>Recebemos uma solicitação para redefinir a senha da sua conta no <strong>Treinu</strong>.</p><p>Por favor, insira o código de 6 dígitos abaixo no aplicativo para criar sua nova senha:</p><div class=\"code-box\">{{CodigoRecuperacao}}</div><p style=\"font-size: 14px; color: #AAAAAA;\">Se você não solicitou a redefinição de senha, ignore este e-mail. Este código expirará em 2 horas.</p></div><div class=\"footer\"><p>© 2026 Treinu App. Todos os direitos reservados.</p></div></div></body></html>");

            migrationBuilder.InsertData(
                table: "TemplatesEmail",
                columns: new[] { "Id", "AssuntoPadrao", "ConteudoHtml", "Nome" },
                values: new object[] { new Guid("66666666-6666-6666-6666-666666666666"), "Cadastro em Análise - Treinu", "<!DOCTYPE html><html lang=\"pt-BR\"><head><meta charset=\"UTF-8\"><meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"><title>Cadastro em Análise - Treinu</title><style>body { font-family: 'Inter', 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #1E1E1E; color: #FFFFFF; margin: 0; padding: 0; } .container { width: 100%; max-width: 600px; margin: 0 auto; background-color: #2D2D2D; border-radius: 12px; overflow: hidden; margin-top: 40px; margin-bottom: 40px; box-shadow: 0 4px 15px rgba(0, 0, 0, 0.3); } .header { background-color: #A3F51D; padding: 40px 20px; text-align: center; color: #000000; } .header h1 { margin: 0; font-size: 28px; font-weight: 800; letter-spacing: -0.5px; } .content { padding: 40px 30px; text-align: center; } .content h2 { color: #FFFFFF; font-size: 24px; margin-top: 0; } .content p { font-size: 16px; line-height: 1.6; color: #E0E0E0; margin-bottom: 25px; } .info-box { background-color: #242424; border-radius: 8px; padding: 20px; margin-top: 30px; text-align: center; border-left: 4px solid #A3F51D; } .footer { background-color: #1E1E1E; padding: 20px; text-align: center; font-size: 14px; color: #888888; border-top: 1px solid #333333; } .footer p { margin: 5px 0; }</style></head><body><div class=\"container\"><div class=\"header\"><div style=\"font-size: 40px; font-weight: bold; margin-bottom: 10px;\">⬢</div><h1>Cadastro Recebido!</h1></div><div class=\"content\"><h2>Olá, {{Nome}}!</h2><p>Recebemos o seu cadastro como Treinador no <strong>Treinu</strong> com sucesso!</p><p>Para garantir a segurança de todos os usuários da plataforma, seu cadastro (e seu número de CREF) entrou em nossa fila de avaliação. Nossa equipe verificará as informações em breve.</p><div class=\"info-box\"><p style=\"color: #A3F51D; font-weight: bold; margin-top: 0;\">Status: Em Análise</p><p style=\"font-size: 14px; color: #CCCCCC; margin-bottom: 0;\">Avisaremos você por e-mail assim que seu acesso for liberado.</p></div></div><div class=\"footer\"><p>© 2026 Treinu App. Todos os direitos reservados.</p></div></div></body></html>", "TreinadorEmAnalise" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TemplatesEmail",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"));

            migrationBuilder.UpdateData(
                table: "TemplatesEmail",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "ConteudoHtml",
                value: "<!DOCTYPE html><html lang=\"pt-BR\"><head><meta charset=\"UTF-8\"><meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"><title>Recuperação de Senha - Treinu</title><style>body { font-family: 'Inter', 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #1E1E1E; color: #FFFFFF; margin: 0; padding: 0; } .container { width: 100%; max-width: 600px; margin: 0 auto; background-color: #2D2D2D; border-radius: 12px; overflow: hidden; margin-top: 40px; margin-bottom: 40px; box-shadow: 0 4px 15px rgba(0, 0, 0, 0.3); } .header { background-color: #A3F51D; padding: 40px 20px; text-align: center; color: #000000; } .header h1 { margin: 0; font-size: 28px; font-weight: 800; letter-spacing: -0.5px; } .content { padding: 40px 30px; text-align: center; } .content p { font-size: 16px; line-height: 1.6; color: #E0E0E0; margin-bottom: 25px; } .btn { display: inline-block; background-color: #A3F51D; color: #000000; text-decoration: none; padding: 15px 35px; font-size: 16px; font-weight: bold; border-radius: 50px; margin: 20px 0; transition: background-color 0.3s; } .btn:hover { background-color: #8cde16; } .footer { background-color: #1E1E1E; padding: 20px; text-align: center; font-size: 14px; color: #888888; border-top: 1px solid #333333; } .footer p { margin: 5px 0; }</style></head><body><div class=\"container\"><div class=\"header\"><div style=\"font-size: 40px; font-weight: bold; margin-bottom: 10px;\">⬢</div><h1>Recuperação de Senha</h1></div><div class=\"content\"><p>Olá, {{Nome}}!</p><p>Recebemos uma solicitação para redefinir a senha da sua conta no <strong>Treinu</strong>.</p><p>Clique no botão abaixo para criar uma nova senha:</p><a href=\"{{LinkRecuperacao}}\" class=\"btn\">Redefinir Minha Senha</a><p style=\"font-size: 14px; color: #AAAAAA; margin-top: 30px;\">Se você não solicitou a redefinição de senha, ignore este e-mail. Sua senha atual permanecerá a mesma.</p><p style=\"font-size: 12px; color: #666666; margin-top: 20px; word-break: break-all;\">Ou copie e cole o link no seu navegador:<br><a href=\"{{LinkRecuperacao}}\" style=\"color: #A3F51D;\">{{LinkRecuperacao}}</a></p></div><div class=\"footer\"><p>© 2026 Treinu App. Todos os direitos reservados.</p></div></div></body></html>");
        }
    }
}
