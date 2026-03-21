using MediatR;
using Treinu.Domain.Enums;

namespace Treinu.Contracts.Events;

public record UsuarioCadastradoNotification(
    Guid Id,
    string Email,
    string Senha,
    PerfilEnum Perfil,
    bool Ativo,
    AuthProviderEnum Provider
) : INotification;
