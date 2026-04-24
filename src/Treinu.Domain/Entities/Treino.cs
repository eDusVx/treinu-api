using FluentResults;
using Treinu.Domain.Core;
using Treinu.Domain.Enums;
using Treinu.Domain.Events;

namespace Treinu.Domain.Entities;

public record CriarTreinoProps(
    string Nome,
    string Descricao,
    DateTime DataInicio,
    DateTime DataFim,
    Guid TreinadorId,
    Guid AlunoId,
    List<CriarItemTreinoProps> Itens
);

public class Treino : AggregateRoot
{
    private readonly List<ItemTreino> _itens = new();

    protected Treino() { }

    private Treino(Guid id) : base(id) { }

    public string Nome { get; private set; } = string.Empty;
    public string Descricao { get; private set; } = string.Empty;
    public DateTime DataInicio { get; private set; }
    public DateTime DataFim { get; private set; }
    public TreinoStatusEnum Status { get; private set; }
    public Guid TreinadorId { get; private set; }
    public Guid AlunoId { get; private set; }
    public virtual Aluno? Aluno { get; private set; }
    
    public IReadOnlyCollection<ItemTreino> Itens => _itens.AsReadOnly();

    public static Result<Treino> Criar(CriarTreinoProps props)
    {
        var instance = new Treino(Guid.NewGuid())
        {
            TreinadorId = props.TreinadorId,
            AlunoId = props.AlunoId,
            Status = TreinoStatusEnum.ATIVO
        };

        var merged = Result.Merge(
            instance.SetNome(props.Nome),
            instance.SetDescricao(props.Descricao),
            instance.SetDatas(props.DataInicio, props.DataFim)
        );

        if (merged.IsFailed) return Result.Fail<Treino>(merged.Errors);

        if (props.Itens == null || !props.Itens.Any())
            return Result.Fail<Treino>("O treino deve conter pelo menos um exercício.");

        foreach (var itemProp in props.Itens)
        {
            var itemResult = ItemTreino.Criar(itemProp);
            if (itemResult.IsFailed) return Result.Fail<Treino>(itemResult.Errors);
            instance._itens.Add(itemResult.Value);
        }

        instance.Apply(new TreinoAtribuidoEvent(instance.Id, instance.AlunoId, instance.TreinadorId, instance.Nome));

        return Result.Ok(instance);
    }

    private Result SetNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            return Result.Fail("Nome do treino não pode ser vazio.");
        
        Nome = nome.Trim();
        return Result.Ok();
    }

    private Result SetDescricao(string descricao)
    {
        Descricao = descricao?.Trim() ?? string.Empty;
        return Result.Ok();
    }

    private Result SetDatas(DateTime inicio, DateTime fim)
    {
        if (inicio == default) return Result.Fail("Data de início inválida.");
        if (fim == default) return Result.Fail("Data de fim inválida.");
        if (fim <= inicio) return Result.Fail("Data de fim deve ser posterior à data de início.");

        DataInicio = inicio;
        DataFim = fim;
        return Result.Ok();
    }

    public Result AtualizarStatus()
    {
        if (Status == TreinoStatusEnum.CONCLUIDO) return Result.Ok();

        if (DataFim.Date < DateTime.Now.Date && Status != TreinoStatusEnum.VENCIDO)
        {
            Status = TreinoStatusEnum.VENCIDO;
            Apply(new TreinoVencidoEvent(Id, AlunoId, Nome));
        }

        return Result.Ok();
    }

    public Treinu.Domain.Dtos.TreinoDto ToDto()
    {
        return new Treinu.Domain.Dtos.TreinoDto(
            Id,
            Nome,
            Descricao,
            DataInicio,
            DataFim,
            Status.ToString(),
            TreinadorId,
            AlunoId,
            Itens.Select(i => i.ToDto()).ToList()
        );
    }
}
