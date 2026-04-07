using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Treinu.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTemplatesEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TemplatesEmail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    AssuntoPadrao = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ConteudoHtml = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplatesEmail", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "TemplatesEmail",
                columns: new[] { "Id", "AssuntoPadrao", "ConteudoHtml", "Nome" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Seu código de acesso - Treinu", "<!DOCTYPE html><html lang=\"pt-BR\"><head><meta charset=\"UTF-8\"><meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"><title>Código de Verificação - Treinu</title><style>body { font-family: 'Inter', 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #1E1E1E; color: #FFFFFF; margin: 0; padding: 0; } .container { width: 100%; max-width: 600px; margin: 0 auto; background-color: #2D2D2D; border-radius: 12px; overflow: hidden; margin-top: 40px; margin-bottom: 40px; box-shadow: 0 4px 15px rgba(0, 0, 0, 0.3); } .header { background-color: #A3F51D; padding: 40px 20px; text-align: center; color: #000000; } .header h1 { margin: 0; font-size: 28px; font-weight: 800; letter-spacing: -0.5px; } .content { padding: 40px 30px; text-align: center; } .content p { font-size: 16px; line-height: 1.6; color: #E0E0E0; margin-bottom: 25px; } .code-box { background-color: #1E1E1E; border: 2px dashed #A3F51D; border-radius: 8px; padding: 20px; font-size: 32px; font-weight: bold; color: #A3F51D; letter-spacing: 4px; margin: 30px 0; display: inline-block; } .footer { background-color: #1E1E1E; padding: 20px; text-align: center; font-size: 14px; color: #888888; border-top: 1px solid #333333; } .footer p { margin: 5px 0; }</style></head><body><div class=\"container\"><div class=\"header\"><div style=\"font-size: 40px; font-weight: bold; margin-bottom: 10px;\">⬢</div><h1>Seu código de acesso</h1></div><div class=\"content\"><p>Olá, {{Nome}}!</p><p>Você solicitou um código de verificação para acessar sua conta ou confirmar seu e-mail no <strong>Treinu</strong>.</p><p>Por favor, insira o código abaixo no aplicativo:</p><div class=\"code-box\">{{CodigoVerificacao}}</div><p style=\"font-size: 14px; color: #AAAAAA;\">Se você não solicitou este código, pode ignorar este e-mail em segurança. Outra pessoa pode ter digitado seu e-mail por engano.</p></div><div class=\"footer\"><p>© 2026 Treinu App. Todos os direitos reservados.</p><p>O seu app definitivo para gerenciar treinos.</p></div></div></body></html>", "VerificacaoEmail" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "Recuperação de Senha - Treinu", "<!DOCTYPE html><html lang=\"pt-BR\"><head><meta charset=\"UTF-8\"><meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"><title>Recuperação de Senha - Treinu</title><style>body { font-family: 'Inter', 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #1E1E1E; color: #FFFFFF; margin: 0; padding: 0; } .container { width: 100%; max-width: 600px; margin: 0 auto; background-color: #2D2D2D; border-radius: 12px; overflow: hidden; margin-top: 40px; margin-bottom: 40px; box-shadow: 0 4px 15px rgba(0, 0, 0, 0.3); } .header { background-color: #A3F51D; padding: 40px 20px; text-align: center; color: #000000; } .header h1 { margin: 0; font-size: 28px; font-weight: 800; letter-spacing: -0.5px; } .content { padding: 40px 30px; text-align: center; } .content p { font-size: 16px; line-height: 1.6; color: #E0E0E0; margin-bottom: 25px; } .btn { display: inline-block; background-color: #A3F51D; color: #000000; text-decoration: none; padding: 15px 35px; font-size: 16px; font-weight: bold; border-radius: 50px; margin: 20px 0; transition: background-color 0.3s; } .btn:hover { background-color: #8cde16; } .footer { background-color: #1E1E1E; padding: 20px; text-align: center; font-size: 14px; color: #888888; border-top: 1px solid #333333; } .footer p { margin: 5px 0; }</style></head><body><div class=\"container\"><div class=\"header\"><div style=\"font-size: 40px; font-weight: bold; margin-bottom: 10px;\">⬢</div><h1>Recuperação de Senha</h1></div><div class=\"content\"><p>Olá, {{Nome}}!</p><p>Recebemos uma solicitação para redefinir a senha da sua conta no <strong>Treinu</strong>.</p><p>Clique no botão abaixo para criar uma nova senha:</p><a href=\"{{LinkRecuperacao}}\" class=\"btn\">Redefinir Minha Senha</a><p style=\"font-size: 14px; color: #AAAAAA; margin-top: 30px;\">Se você não solicitou a redefinição de senha, ignore este e-mail. Sua senha atual permanecerá a mesma.</p><p style=\"font-size: 12px; color: #666666; margin-top: 20px; word-break: break-all;\">Ou copie e cole o link no seu navegador:<br><a href=\"{{LinkRecuperacao}}\" style=\"color: #A3F51D;\">{{LinkRecuperacao}}</a></p></div><div class=\"footer\"><p>© 2026 Treinu App. Todos os direitos reservados.</p></div></div></body></html>", "RecuperacaoSenha" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), "Bem-vindo ao Treinu!", "<!DOCTYPE html><html lang=\"pt-BR\"><head><meta charset=\"UTF-8\"><meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"><title>Bem-vindo ao Treinu</title><style>body { font-family: 'Inter', 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #1E1E1E; color: #FFFFFF; margin: 0; padding: 0; } .container { width: 100%; max-width: 600px; margin: 0 auto; background-color: #2D2D2D; border-radius: 12px; overflow: hidden; margin-top: 40px; margin-bottom: 40px; box-shadow: 0 4px 15px rgba(0, 0, 0, 0.3); } .header { background-color: #A3F51D; padding: 40px 20px; text-align: center; color: #000000; } .header h1 { margin: 0; font-size: 28px; font-weight: 800; letter-spacing: -0.5px; } .content { padding: 40px 30px; text-align: center; } .content h2 { color: #FFFFFF; font-size: 24px; margin-top: 0; } .content p { font-size: 16px; line-height: 1.6; color: #E0E0E0; margin-bottom: 25px; } .btn { display: inline-block; background-color: #A3F51D; color: #000000; text-decoration: none; padding: 15px 35px; font-size: 16px; font-weight: bold; border-radius: 50px; margin: 20px 0; transition: background-color 0.3s; } .btn:hover { background-color: #8cde16; } .features { background-color: #242424; border-radius: 8px; padding: 20px; margin-top: 30px; text-align: left; } .features ul { list-style-type: none; padding: 0; margin: 0; } .features li { margin-bottom: 10px; padding-left: 25px; position: relative; color: #CCCCCC; } .features li::before { content: '✓'; color: #A3F51D; position: absolute; left: 0; font-weight: bold; } .footer { background-color: #1E1E1E; padding: 20px; text-align: center; font-size: 14px; color: #888888; border-top: 1px solid #333333; } .footer p { margin: 5px 0; }</style></head><body><div class=\"container\"><div class=\"header\"><div style=\"font-size: 40px; font-weight: bold; margin-bottom: 10px;\">⬢</div><h1>Seja bem-vindo!</h1></div><div class=\"content\"><h2>Olá, {{Nome}}!</h2><p>Obrigado por se registrar no <strong>Treinu</strong>! Sua conta foi criada com sucesso e você já pode começar a usar todas as nossas ferramentas.</p><p>Se você se registrou como Treinador, complete suas informações profissionais no aplicativo para liberar o acesso aos seus alunos e gerenciar suas fichas de treino.</p><a href=\"{{LinkLogin}}\" class=\"btn\">Acessar minha conta</a><div class=\"features\"><p style=\"color: #FFFFFF; font-weight: bold; margin-bottom: 15px; text-align: center;\">O que você pode fazer no Treinu:</p><ul><li>Gerenciar alunos e prescrição de treinos;</li><li>Acompanhar evolução e métricas de desempenho;</li><li>Organizar sua agenda e informações em um só lugar.</li></ul></div></div><div class=\"footer\"><p>© 2026 Treinu App. Todos os direitos reservados.</p><p>Estamos felizes em ter você com a gente!</p></div></div></body></html>", "BoasVindas" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TemplatesEmail");
        }
    }
}
