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
    protected Credencial()
    {
    }

    private Credencial(Guid id) : base(id)
    {
    }

    public Guid UsuarioId { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string? Senha { get; private set; }
    public string? RefreshToken { get; private set; }
    public DateTime? RefreshTokenExpiryTime { get; private set; }
    public string? ResetPasswordToken { get; private set; }
    public DateTime? ResetPasswordTokenExpiryTime { get; private set; }
    public string? CodigoLogin { get; private set; }
    public DateTime? CodigoLoginExpiryTime { get; private set; }
    public PerfilEnum TipoUsuario { get; private set; }
    public bool Ativo { get; private set; }

    public virtual Usuario Usuario { get; private set; } = null!;

    public static Result<Credencial> Criar(CriarCredencialProps props)
    {
        var id = Guid.NewGuid();
        var instance = new Credencial(id);

        instance.UsuarioId = props.UsuarioId;

        var emailResult = instance.SetEmail(props.Email);
        if (emailResult.IsFailed) return Result.Fail<Credencial>(emailResult.Errors);

        instance.TipoUsuario = props.TipoUsuario;
        instance.Ativo = props.Ativo;

        var senhaResult = instance.SetSenhaLocal(props.Senha);
        if (senhaResult.IsFailed) return Result.Fail<Credencial>(senhaResult.Errors);

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
        RefreshTokenExpiryTime = DateTime.SpecifyKind(expiryTime, DateTimeKind.Utc);
    }

    public void GerarTokenRecuperacaoSenha(string token, DateTime expiryTime)
    {
        ResetPasswordToken = token;
        ResetPasswordTokenExpiryTime = DateTime.SpecifyKind(expiryTime, DateTimeKind.Utc);
    }

    public Result RedefinirSenha(string token, string novaSenhaHashed)
    {
        if (string.IsNullOrWhiteSpace(ResetPasswordToken) || 
            ResetPasswordToken != token || 
            ResetPasswordTokenExpiryTime < DateTime.UtcNow)
        {
            return Result.Fail(DomainErrors.Credencial.TokenRecuperacaoInvalido);
        }

        var senhaResult = SetSenhaLocal(novaSenhaHashed);
        if (senhaResult.IsFailed) return senhaResult;

        ResetPasswordToken = null;
        ResetPasswordTokenExpiryTime = null;

        return Result.Ok();
    }

    public void GerarCodigoLogin(string codigo, DateTime expiryTime)
    {
        CodigoLogin = codigo;
        CodigoLoginExpiryTime = DateTime.SpecifyKind(expiryTime, DateTimeKind.Utc);
    }

    public Result VerificarCodigoLogin(string codigo)
    {
        if (string.IsNullOrWhiteSpace(CodigoLogin) || 
            CodigoLogin != codigo || 
            CodigoLoginExpiryTime < DateTime.UtcNow)
        {
            return Result.Fail(DomainErrors.Credencial.CodigoLoginInvalido);
        }

        CodigoLogin = null;
        CodigoLoginExpiryTime = null;

        return Result.Ok();
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