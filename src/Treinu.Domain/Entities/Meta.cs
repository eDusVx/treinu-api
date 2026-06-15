using FluentResults;
using Treinu.Domain.Core;
using Treinu.Domain.Enums;

namespace Treinu.Domain.Entities;

public class Meta : AggregateRoot
{
    protected Meta() { }

    private Meta(Guid id) : base(id) { }

    public Guid AlunoId { get; private set; }
    public virtual Aluno? Aluno { get; private set; }
    public TipoMetaEnum Tipo { get; private set; }
    public decimal ValorAlvo { get; private set; }
    public DateTime DataLimite { get; private set; }
    public DateTime DataCriacao { get; private set; }
    public bool Ativa { get; private set; }

    public static Result<Meta> Criar(Guid alunoId, TipoMetaEnum tipo, decimal valorAlvo, DateTime dataLimite)
    {
        if (alunoId == Guid.Empty) return Result.Fail("O ID do aluno é obrigatório.");
        if (valorAlvo <= 0) return Result.Fail("O valor alvo deve ser maior que zero.");
        if (dataLimite.Date <= DateTime.UtcNow.Date) return Result.Fail("A data limite deve ser no futuro.");

        var instance = new Meta(Guid.NewGuid())
        {
            AlunoId = alunoId,
            Tipo = tipo,
            ValorAlvo = Math.Round(valorAlvo, 2),
            DataLimite = DateTime.SpecifyKind(dataLimite.Date, DateTimeKind.Utc),
            DataCriacao = DateTime.UtcNow,
            Ativa = true
        };

        return Result.Ok(instance);
    }

    public Result Desativar()
    {
        Ativa = false;
        return Result.Ok();
    }
}
