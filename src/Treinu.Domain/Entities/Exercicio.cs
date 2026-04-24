using FluentResults;
using Treinu.Domain.Core;

namespace Treinu.Domain.Entities;

public record CriarExercicioProps(
    string Nome,
    string Descricao,
    string Tags,
    string? ArquivoDemonstracao,
    Guid TreinadorId
);

public class Exercicio : AggregateRoot
{
    protected Exercicio() { }

    private Exercicio(Guid id) : base(id) { }

    public string Nome { get; private set; } = string.Empty;
    public string Descricao { get; private set; } = string.Empty;
    public string Tags { get; private set; } = string.Empty;
    public string? ArquivoDemonstracao { get; private set; }
    public Guid TreinadorId { get; private set; }
    public DateTime CriadoEm { get; private set; }

    public static Result<Exercicio> Criar(CriarExercicioProps props)
    {
        var instance = new Exercicio(Guid.NewGuid())
        {
            TreinadorId = props.TreinadorId,
            CriadoEm = DateTime.UtcNow
        };

        var nomeResult = instance.SetNome(props.Nome);
        var descResult = instance.SetDescricao(props.Descricao);
        var tagsResult = instance.SetTags(props.Tags);
        var arqResult = instance.SetArquivoDemonstracao(props.ArquivoDemonstracao);

        var merged = Result.Merge(nomeResult, descResult, tagsResult, arqResult);
        if (merged.IsFailed) return Result.Fail<Exercicio>(merged.Errors);

        return Result.Ok(instance);
    }

    private Result SetNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            return Result.Fail("Nome do exercício não pode ser vazio.");
        
        Nome = nome.Trim();
        return Result.Ok();
    }

    private Result SetDescricao(string descricao)
    {
        if (string.IsNullOrWhiteSpace(descricao))
            return Result.Fail("Descrição do exercício não pode ser vazia.");
        
        Descricao = descricao.Trim();
        return Result.Ok();
    }

    private Result SetTags(string tags)
    {
        Tags = tags?.Trim() ?? string.Empty;
        return Result.Ok();
    }

    private Result SetArquivoDemonstracao(string? arquivo)
    {
        ArquivoDemonstracao = arquivo?.Trim();
        return Result.Ok();
    }

    public Treinu.Domain.Dtos.ExercicioDto ToDto()
    {
        return new Treinu.Domain.Dtos.ExercicioDto(
            Id, Nome, Descricao, Tags, ArquivoDemonstracao, TreinadorId, CriadoEm
        );
    }
}
