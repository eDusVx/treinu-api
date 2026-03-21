using BCrypt.Net;
using FluentResults;
using Treinu.Domain.Core;
using Treinu.Domain.Enums;
using Treinu.Domain.Errors;

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

    public static Result<Credencial> Criar(CriarCredencialProps props)
    {
        var id = Guid.NewGuid();
        var instance = new Credencial(id);
        
        instance.UsuarioId = props.UsuarioId;
        
        var emailResult = instance.SetEmail(props.Email);
        if(emailResult.IsFailed) return Result.Fail<Credencial>(emailResult.Errors);
        
        instance.TipoUsuario = props.TipoUsuario;
        instance.Ativo = props.Ativo;
        
        var senhaResult = instance.SetSenhaLocal(props.Senha);
        if(senhaResult.IsFailed) return Result.Fail<Credencial>(senhaResult.Errors);

        return Result.Ok(instance);
    }

    private Result SetEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return Result.Fail(DomainErrors.Usuario.DadosVazios);
        Email = email;
        return Result.Ok();
    }

    private Result SetSenhaLocal(string? senha)
    {
        if (string.IsNullOrWhiteSpace(senha))
            return Result.Fail(DomainErrors.Usuario.DadosVazios);
        
        Senha = senha;
        return Result.Ok();
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

    public Result VerificarSenha(string senha)
    {
        if (string.IsNullOrWhiteSpace(Senha) || !BCrypt.Net.BCrypt.Verify(senha, Senha))
            return Result.Fail(DomainErrors.Credencial.SenhaIncorreta);
            
        return Result.Ok();
    }
}
