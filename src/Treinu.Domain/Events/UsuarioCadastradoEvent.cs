using Treinu.Domain.Core;
using Treinu.Domain.Enums;

namespace Treinu.Domain.Events;

public class UsuarioCadastradoEvent : IDomainEvent
{
    public Guid Id { get; }
    public string Email { get; }
    public string Senha { get; }
    public PerfilEnum Perfil { get; }
    public bool Ativo { get; }
    public AuthProviderEnum Provider { get; }

    public UsuarioCadastradoEvent(Guid id, string email, string senha, PerfilEnum perfil, bool ativo, AuthProviderEnum provider)
    {
        Id = id;
        Email = email;
        Senha = senha;
        Perfil = perfil;
        Ativo = ativo;
        Provider = provider;
    }
}
