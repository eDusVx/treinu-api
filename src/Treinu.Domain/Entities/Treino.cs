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
    List<CriarItemTreinoProps> Itens,
    string? NomeDivisaoA,
    string? NomeDivisaoB,
    string? NomeDivisaoC,
    string? NomeDivisaoD,
    string? DivisaoSegunda,
    string? DivisaoTerca,
    string? DivisaoQuarta,
    string? DivisaoQuinta,
    string? DivisaoSexta,
    string? DivisaoSabado,
    string? DivisaoDomingo
);

public class Treino : AggregateRoot
{
    private static readonly HashSet<string> DivisoesValidas = new() { "A", "B", "C", "D" };
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
    
    public string? NomeDivisaoA { get; private set; }
    public string? NomeDivisaoB { get; private set; }
    public string? NomeDivisaoC { get; private set; }
    public string? NomeDivisaoD { get; private set; }

    public string? DivisaoSegunda { get; private set; }
    public string? DivisaoTerca { get; private set; }
    public string? DivisaoQuarta { get; private set; }
    public string? DivisaoQuinta { get; private set; }
    public string? DivisaoSexta { get; private set; }
    public string? DivisaoSabado { get; private set; }
    public string? DivisaoDomingo { get; private set; }

    public IReadOnlyCollection<ItemTreino> Itens => _itens.AsReadOnly();

    private static Result ValidarDivisoesECronograma(
        string? nomeA, string? nomeB, string? nomeC, string? nomeD,
        string? seg, string? ter, string? qua, string? qui, string? sex, string? sab, string? dom)
    {
        if (string.IsNullOrWhiteSpace(nomeA))
            return Result.Fail("O treino deve ter pelo menos a Divisão A configurada.");
        if (string.IsNullOrWhiteSpace(nomeB))
            return Result.Fail("O treino deve ter pelo menos a Divisão B configurada.");

        var divisoesAtivas = new HashSet<string> { "A", "B" };
        if (!string.IsNullOrWhiteSpace(nomeC)) divisoesAtivas.Add("C");
        if (!string.IsNullOrWhiteSpace(nomeD)) divisoesAtivas.Add("D");

        var dias = new (string? Divisao, string NomeDia)[]
        {
            (seg, "Segunda-feira"),
            (ter, "Terça-feira"),
            (qua, "Quarta-feira"),
            (qui, "Quinta-feira"),
            (sex, "Sexta-feira"),
            (sab, "Sábado"),
            (dom, "Domingo")
        };

        foreach (var dia in dias)
        {
            if (!string.IsNullOrWhiteSpace(dia.Divisao))
            {
                var upper = dia.Divisao.Trim().ToUpper();
                if (!DivisoesValidas.Contains(upper))
                    return Result.Fail($"Divisão inválida para {dia.NomeDia}. Deve ser A, B, C ou D.");
                if (!divisoesAtivas.Contains(upper))
                    return Result.Fail($"A divisão {upper} foi atribuída a {dia.NomeDia}, mas não está configurada no treino (preencha o nome da divisão).");
            }
        }

        return Result.Ok();
    }

    public static Result<Treino> Criar(CriarTreinoProps props)
    {
        var valResult = ValidarDivisoesECronograma(
            props.NomeDivisaoA, props.NomeDivisaoB, props.NomeDivisaoC, props.NomeDivisaoD,
            props.DivisaoSegunda, props.DivisaoTerca, props.DivisaoQuarta, props.DivisaoQuinta, props.DivisaoSexta, props.DivisaoSabado, props.DivisaoDomingo);

        if (valResult.IsFailed) return Result.Fail<Treino>(valResult.Errors);

        var instance = new Treino(Guid.NewGuid())
        {
            TreinadorId = props.TreinadorId,
            AlunoId = props.AlunoId,
            Status = TreinoStatusEnum.ATIVO,
            NomeDivisaoA = props.NomeDivisaoA?.Trim(),
            NomeDivisaoB = props.NomeDivisaoB?.Trim(),
            NomeDivisaoC = props.NomeDivisaoC?.Trim(),
            NomeDivisaoD = props.NomeDivisaoD?.Trim(),
            DivisaoSegunda = props.DivisaoSegunda?.Trim().ToUpper(),
            DivisaoTerca = props.DivisaoTerca?.Trim().ToUpper(),
            DivisaoQuarta = props.DivisaoQuarta?.Trim().ToUpper(),
            DivisaoQuinta = props.DivisaoQuinta?.Trim().ToUpper(),
            DivisaoSexta = props.DivisaoSexta?.Trim().ToUpper(),
            DivisaoSabado = props.DivisaoSabado?.Trim().ToUpper(),
            DivisaoDomingo = props.DivisaoDomingo?.Trim().ToUpper()
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

    public Result Atualizar(
        string nome,
        string descricao,
        DateTime dataInicio,
        DateTime dataFim,
        string? nomeDivisaoA,
        string? nomeDivisaoB,
        string? nomeDivisaoC,
        string? nomeDivisaoD,
        string? divisaoSegunda,
        string? divisaoTerca,
        string? divisaoQuarta,
        string? divisaoQuinta,
        string? divisaoSexta,
        string? divisaoSabado,
        string? divisaoDomingo)
    {
        var valResult = ValidarDivisoesECronograma(
            nomeDivisaoA, nomeDivisaoB, nomeDivisaoC, nomeDivisaoD,
            divisaoSegunda, divisaoTerca, divisaoQuarta, divisaoQuinta, divisaoSexta, divisaoSabado, divisaoDomingo);

        if (valResult.IsFailed) return valResult;

        var merged = Result.Merge(
            SetNome(nome),
            SetDescricao(descricao),
            SetDatas(dataInicio, dataFim)
        );

        if (merged.IsSuccess)
        {
            NomeDivisaoA = nomeDivisaoA?.Trim();
            NomeDivisaoB = nomeDivisaoB?.Trim();
            NomeDivisaoC = nomeDivisaoC?.Trim();
            NomeDivisaoD = nomeDivisaoD?.Trim();
            DivisaoSegunda = divisaoSegunda?.Trim().ToUpper();
            DivisaoTerca = divisaoTerca?.Trim().ToUpper();
            DivisaoQuarta = divisaoQuarta?.Trim().ToUpper();
            DivisaoQuinta = divisaoQuinta?.Trim().ToUpper();
            DivisaoSexta = divisaoSexta?.Trim().ToUpper();
            DivisaoSabado = divisaoSabado?.Trim().ToUpper();
            DivisaoDomingo = divisaoDomingo?.Trim().ToUpper();

            Apply(new TreinoAtualizadoEvent(Id, AlunoId, Nome));
        }

        return merged;
    }

    public Result AdicionarItem(CriarItemTreinoProps props)
    {
        var itemResult = ItemTreino.Criar(props);
        if (itemResult.IsFailed) return Result.Fail(itemResult.Errors);
        
        _itens.Add(itemResult.Value);
        return Result.Ok();
    }

    public Result RemoverItem(Guid itemId)
    {
        var item = _itens.FirstOrDefault(i => i.Id == itemId);
        if (item == null) return Result.Fail("Item de treino não encontrado.");
        
        _itens.Remove(item);
        return Result.Ok();
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
        if (DataFim.Date < DateTime.UtcNow.Date && Status != TreinoStatusEnum.VENCIDO)
        {
            Status = TreinoStatusEnum.VENCIDO;
            Apply(new TreinoVencidoEvent(Id, AlunoId, Nome));
        }

        return Result.Ok();
    }

    public string? ObterDivisaoParaDia(DayOfWeek dayOfWeek)
    {
        return dayOfWeek switch
        {
            DayOfWeek.Monday => DivisaoSegunda,
            DayOfWeek.Tuesday => DivisaoTerca,
            DayOfWeek.Wednesday => DivisaoQuarta,
            DayOfWeek.Thursday => DivisaoQuinta,
            DayOfWeek.Friday => DivisaoSexta,
            DayOfWeek.Saturday => DivisaoSabado,
            DayOfWeek.Sunday => DivisaoDomingo,
            _ => null
        };
    }

    public Treinu.Domain.Dtos.TreinoDto ToDto(DayOfWeek? filtrarDiaSemana = null)
    {
        var itensFiltered = Itens.ToList();
        if (filtrarDiaSemana.HasValue)
        {
            var divisaoHoje = ObterDivisaoParaDia(filtrarDiaSemana.Value);
            // Se divisaoHoje for nula (dia de descanso), retornamos lista vazia de exercícios
            itensFiltered = string.IsNullOrEmpty(divisaoHoje) 
                ? new List<ItemTreino>() 
                : _itens.Where(i => i.Divisao == divisaoHoje).ToList();
        }

        return new Treinu.Domain.Dtos.TreinoDto(
            Id,
            Nome,
            Descricao,
            DataInicio,
            DataFim,
            Status.ToString(),
            TreinadorId,
            AlunoId,
            itensFiltered.Select(i => i.ToDto()).ToList(),
            NomeDivisaoA,
            NomeDivisaoB,
            NomeDivisaoC,
            NomeDivisaoD,
            DivisaoSegunda,
            DivisaoTerca,
            DivisaoQuarta,
            DivisaoQuinta,
            DivisaoSexta,
            DivisaoSabado,
            DivisaoDomingo
        );
    }
}
