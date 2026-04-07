using Treinu.Domain.Enums;

namespace Treinu.Domain.Core;

public interface IEmailService
{
    Task EnviarConviteAsync(string email, Guid token, PerfilEnum perfil);
    Task EnviarVerificacaoEmailAsync(string email, string nome, string codigo);
    Task EnviarRecuperacaoSenhaAsync(string email, string nome, string codigo);
    Task EnviarBoasVindasAsync(string email, string nome, string linkLogin);
    Task EnviarTreinadorAprovadoAsync(string email, string nome, string linkLogin);
    Task EnviarTreinadorEmAnaliseAsync(string email, string nome);
    Task SendEmailAsync(string to, string subject, string body);
}