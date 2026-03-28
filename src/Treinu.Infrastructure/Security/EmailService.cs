using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using Treinu.Domain.Core;
using Treinu.Domain.Enums;

namespace Treinu.Infrastructure.Security;

public class EmailService : IEmailService
{
    private readonly string _apiKey;
    private readonly string _appUrl;
    private readonly string _fromEmail;
    private readonly string _fromName;

    public EmailService(IConfiguration configuration)
    {
        _apiKey = configuration.GetValue<string>("SendGrid:ApiKey") ??
                  throw new ArgumentNullException("SendGrid:ApiKey");
        _fromEmail = configuration.GetValue<string>("SendGrid:FromEmail") ?? "contato@treinu.app";
        _fromName = configuration.GetValue<string>("SendGrid:FromName") ?? "Treinu App";
        _appUrl = configuration.GetValue<string>("AppUrl") ?? "https://treinu.app";
    }

    public async Task EnviarConviteAsync(string email, Guid token, PerfilEnum perfil)
    {
        var subject = "Convite para Treinu App";
        var body = $@"
            <h1>Bem-vindo ao Treinu App!</h1>
            <p>Você foi convidado a se registrar como {perfil.ToString().ToLower()}.</p>
            <p>Clique no link abaixo para completar seu cadastro:</p>
            <a href='{_appUrl}/registro?token={token}'>Completar Cadastro</a>
            <p>Este token expira em 48 horas.</p>";

        await SendEmailAsync(email, subject, body);
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var client = new SendGridClient(_apiKey);
        var from = new EmailAddress(_fromEmail, _fromName);
        var to = new EmailAddress(toEmail);

        var msg = MailHelper.CreateSingleEmail(
            from,
            to,
            subject,
            null,
            body
        );

        var response = await client.SendEmailAsync(msg);

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await response.Body.ReadAsStringAsync();
            throw new Exception($"Falha ao enviar e-mail: {response.StatusCode}. Erro: {errorMessage}");
        }
    }
}