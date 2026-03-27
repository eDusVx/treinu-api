using Treinu.Domain.Core;
using Treinu.Domain.Enums;

namespace Treinu.Infrastructure.Security;

public class EmailService : IEmailService
{
    public Task EnviarConviteAsync(string email, Guid token, PerfilEnum perfil)
    {
        // Stub: In a real scenario, this would send an actual email.
        Console.WriteLine($"[EMAIL STUB] Enviando convite para {email}. Perfil: {perfil}. Token: {token}");
        return Task.CompletedTask;
    }
}
