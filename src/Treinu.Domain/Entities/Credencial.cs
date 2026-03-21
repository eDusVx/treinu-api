using BCrypt.Net;
using Treinu.Domain.Core;
using Treinu.Domain.Enums;
using Treinu.Domain.Exceptions;

namespace Treinu.Domain.Entities;

public record CriarCredencialProps(
    Guid UsuarioId,
    string Email,
    PerfilEnum TipoUsuario,
    bool Ativo,
    AuthProviderEnum Provider,
    string? Senha = null
);

public class Credencial : Entity
{
    public Guid UsuarioId { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string? Senha { get; private set; }
    public AuthProviderEnum Provider { get; private set; }
    public PerfilEnum TipoUsuario { get; private set; }
    public bool Ativo { get; private set; }

    protected Credencial() { }

    private Credencial(Guid id) : base(id) { }

    public static Credencial Criar(CriarCredencialProps props)
    {
        var id = Guid.NewGuid();
        var instance = new Credencial(id);
        
        instance.UsuarioId = props.UsuarioId;
        instance.SetEmail(props.Email);
        instance.TipoUsuario = props.TipoUsuario;
        instance.Ativo = props.Ativo;
        instance.SetProvider(props.Provider, props.Senha);

        return instance;
    }

    private void SetEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new InvalidOperationException("Email cannot be empty");
        Email = email;
    }

    private void SetProvider(AuthProviderEnum provider, string? senha)
    {
        if (provider == AuthProviderEnum.LOCAL)
        {
            if (string.IsNullOrWhiteSpace(senha))
                throw new InvalidOperationException("Provider LOCAL requires a password");
            
            Senha = senha;
        }
        else
        {
            Senha = null; // No password for GOOGLE etc
        }

        Provider = provider;
    }

    public void VerificarProvider(AuthProviderEnum provider)
    {
        if (Provider != provider)
            throw new UnauthorizedAccessException("Usuário cadastrado com outro provedor de autenticação.");
    }

    public void VerificarSenha(string senha)
    {
        if (Provider != AuthProviderEnum.LOCAL)
            throw new UnauthorizedAccessException("Credencial não utiliza autenticação local.");
            
        if (string.IsNullOrWhiteSpace(Senha) || !BCrypt.Net.BCrypt.Verify(senha, Senha))
            throw new UnauthorizedAccessException("E-mail ou senha incorretos.");
    }
}
