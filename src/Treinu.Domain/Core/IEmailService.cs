using Treinu.Domain.Enums;

namespace Treinu.Domain.Core;

public interface IEmailService
{
    Task EnviarConviteAsync(string email, Guid token, PerfilEnum perfil);
}
