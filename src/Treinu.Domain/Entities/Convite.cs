using FluentResults;
using Treinu.Domain.Core;
using Treinu.Domain.Enums;
using Treinu.Domain.Errors;

namespace Treinu.Domain.Entities;

public class Convite : Entity
{
    protected Convite() { }

    private Convite(Guid id, string email, PerfilEnum perfil, Guid? treinadorId = null) : base(id)
    {
        Email = email;
        Token = Guid.NewGuid();
        Perfil = perfil;
        TreinadorId = treinadorId;
        Status = ConviteStatusEnum.PENDENTE;
        CriadoEm = DateTime.UtcNow;
        ExpiraEm = DateTime.UtcNow.AddDays(7);
    }

    public string Email { get; private set; } = string.Empty;
    public Guid Token { get; private set; }
    public PerfilEnum Perfil { get; private set; }
    public Guid? TreinadorId { get; private set; }
    public virtual Treinador? Treinador { get; private set; }
    public ConviteStatusEnum Status { get; private set; }
    public DateTime ExpiraEm { get; private set; }
    public DateTime CriadoEm { get; private set; }

    public static Result<Convite> Criar(string email, PerfilEnum perfil, Guid? treinadorId = null)
    {
        if (string.IsNullOrWhiteSpace(email))
            return Result.Fail(DomainErrors.Usuario.EmailInvalido);

        if (perfil == PerfilEnum.ALUNO && treinadorId == null)
            return Result.Fail("Treinador é obrigatório para convites de alunos.");

        return Result.Ok(new Convite(Guid.NewGuid(), email, perfil, treinadorId));
    }

    public Result Aceitar()
    {
        if (Status != ConviteStatusEnum.PENDENTE)
            return Result.Fail(DomainErrors.Convite.ConviteJaUtilizado);

        if (DateTime.UtcNow > ExpiraEm)
        {
            Status = ConviteStatusEnum.EXPIRADO;
            return Result.Fail(DomainErrors.Convite.ConviteExpirado);
        }

        Status = ConviteStatusEnum.ACEITO;
        return Result.Ok();
    }
}
