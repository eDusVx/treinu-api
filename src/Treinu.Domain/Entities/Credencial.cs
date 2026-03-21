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
    string? Senha = null
);

public class Credencial : Entity
{
    public Guid UsuarioId { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string? Senha { get; private set; }
    public string? RefreshToken { get; private set; }
    public DateTime? RefreshTokenExpiryTime { get; private set; }
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
        instance.SetSenhaLocal(props.Senha);

        return instance;
    }

    private void SetEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new InvalidOperationException("Email cannot be empty");
        Email = email;
    }

    private void SetSenhaLocal(string? senha)
    {
        if (string.IsNullOrWhiteSpace(senha))
            throw new InvalidOperationException("Credencial requires a password");
        
        Senha = senha;
    }

    public void AtualizarRefreshToken(string refreshToken, DateTime expiryTime)
    {
        RefreshToken = refreshToken;
        RefreshTokenExpiryTime = expiryTime;
    }

    public void RevogarRefreshToken()
    {
        RefreshToken = null;
        RefreshTokenExpiryTime = null;
    }

    public void VerificarSenha(string senha)
    {
        if (string.IsNullOrWhiteSpace(Senha) || !BCrypt.Net.BCrypt.Verify(senha, Senha))
            throw new UnauthorizedAccessException("E-mail ou senha incorretos.");
    }
}
