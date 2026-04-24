using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using Treinu.Domain.Core;
using Treinu.Domain.Enums;
using Treinu.Domain.Repositories;

namespace Treinu.Infrastructure.Security;

public class EmailService : IEmailService
{
    private readonly string _apiKey;
    private readonly string _appUrl;
    private readonly string _fromEmail;
    private readonly string _fromName;

    private readonly ITemplateEmailRepository _templateRepository;

    public EmailService(IConfiguration configuration, ITemplateEmailRepository templateRepository)
    {
        _apiKey = Environment.GetEnvironmentVariable("SendGrid__ApiKey")
                  ?? configuration.GetValue<string>("SendGrid:ApiKey")
                  ?? throw new ArgumentNullException("SendGrid:ApiKey");
        _fromEmail = Environment.GetEnvironmentVariable("SendGrid__FromEmail")
                     ?? configuration.GetValue<string>("SendGrid:FromEmail") ?? "contato@treinu.app";
        _fromName = Environment.GetEnvironmentVariable("SendGrid__FromName")
                     ?? configuration.GetValue<string>("SendGrid:FromName") ?? "Treinu App";
        _appUrl = Environment.GetEnvironmentVariable("AppUrl")
                  ?? configuration.GetValue<string>("AppUrl") ?? "https://treinu.app";
        _templateRepository = templateRepository;
    }

    public async Task EnviarConviteAsync(string email, Guid token, PerfilEnum perfil)
    {
        var template = await _templateRepository.ObterPorNomeAsync("ConviteTreinador");
        if (template is null) throw new InvalidOperationException("Template de e-mail 'ConviteTreinador' não encontrado no banco de dados.");

        var link = $"{_appUrl}/cadastro/aluno/{token}";

        var html = template.ConteudoHtml
            .Replace("{{Perfil}}", perfil.ToString().ToLower())
            .Replace("{{LinkConvite}}", link);

        await SendEmailAsync(email, template.AssuntoPadrao, html);
    }

    public async Task EnviarVerificacaoEmailAsync(string email, string nome, string codigo)
    {
        var template = await _templateRepository.ObterPorNomeAsync("VerificacaoEmail");
        if (template is null) throw new InvalidOperationException("Template de e-mail 'VerificacaoEmail' não encontrado no banco de dados.");

        var html = template.ConteudoHtml
            .Replace("{{Nome}}", nome)
            .Replace("{{CodigoVerificacao}}", codigo);

        await SendEmailAsync(email, template.AssuntoPadrao, html);
    }

    public async Task EnviarRecuperacaoSenhaAsync(string email, string nome, string codigo)
    {
        var template = await _templateRepository.ObterPorNomeAsync("RecuperacaoSenha");
        if (template is null) throw new InvalidOperationException("Template de e-mail 'RecuperacaoSenha' não encontrado no banco de dados.");

        var html = template.ConteudoHtml
            .Replace("{{Nome}}", nome)
            .Replace("{{CodigoRecuperacao}}", codigo);

        await SendEmailAsync(email, template.AssuntoPadrao, html);
    }

    public async Task EnviarBoasVindasAsync(string email, string nome, string linkLogin)
    {
        var template = await _templateRepository.ObterPorNomeAsync("BoasVindas");
        if (template is null) throw new InvalidOperationException("Template de e-mail 'BoasVindas' não encontrado no banco de dados.");

        var html = template.ConteudoHtml
            .Replace("{{Nome}}", nome)
            .Replace("{{LinkLogin}}", string.IsNullOrEmpty(linkLogin) ? _appUrl : linkLogin);

        await SendEmailAsync(email, template.AssuntoPadrao, html);
    }

    public async Task EnviarTreinadorAprovadoAsync(string email, string nome, string linkLogin)
    {
        var template = await _templateRepository.ObterPorNomeAsync("TreinadorAprovado");
        if (template is null) throw new InvalidOperationException("Template de e-mail 'TreinadorAprovado' não encontrado no banco de dados.");

        var html = template.ConteudoHtml
            .Replace("{{Nome}}", nome)
            .Replace("{{LinkLogin}}", string.IsNullOrEmpty(linkLogin) ? _appUrl : linkLogin);

        await SendEmailAsync(email, template.AssuntoPadrao, html);
    }

    public async Task EnviarTreinadorEmAnaliseAsync(string email, string nome)
    {
        var template = await _templateRepository.ObterPorNomeAsync("TreinadorEmAnalise");
        if (template is null) throw new InvalidOperationException("Template de e-mail 'TreinadorEmAnalise' não encontrado no banco de dados.");

        var html = template.ConteudoHtml
            .Replace("{{Nome}}", nome);

        await SendEmailAsync(email, template.AssuntoPadrao, html);
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