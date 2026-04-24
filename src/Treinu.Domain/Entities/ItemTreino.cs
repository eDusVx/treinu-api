using FluentResults;
using Treinu.Domain.Core;

namespace Treinu.Domain.Entities;

public record CriarItemTreinoProps(
    Guid ExercicioId,
    int Series,
    string Repeticoes,
    string Carga,
    string Pausa,
    string Observacoes,
    int Ordem
);

public class ItemTreino : Entity
{
    protected ItemTreino() { }

    private ItemTreino(Guid id) : base(id) { }

    public Guid TreinoId { get; private set; }
    public Guid ExercicioId { get; private set; }
    public virtual Exercicio? Exercicio { get; private set; }
    
    public int Series { get; private set; }
    public string Repeticoes { get; private set; } = string.Empty;
    public string Carga { get; private set; } = string.Empty;
    public string Pausa { get; private set; } = string.Empty;
    public string Observacoes { get; private set; } = string.Empty;
    public int Ordem { get; private set; }

    internal static Result<ItemTreino> Criar(CriarItemTreinoProps props)
    {
        var instance = new ItemTreino(Guid.NewGuid())
        {
            ExercicioId = props.ExercicioId,
            Series = props.Series,
            Repeticoes = props.Repeticoes ?? string.Empty,
            Carga = props.Carga ?? string.Empty,
            Pausa = props.Pausa ?? string.Empty,
            Observacoes = props.Observacoes ?? string.Empty,
            Ordem = props.Ordem
        };

        if (props.ExercicioId == Guid.Empty)
            return Result.Fail("ExercicioId inválido para o item do treino.");
            
        if (props.Series <= 0)
            return Result.Fail("O número de séries deve ser maior que zero.");

        return Result.Ok(instance);
    }

    public Treinu.Domain.Dtos.ItemTreinoDto ToDto()
    {
        return new Treinu.Domain.Dtos.ItemTreinoDto(
            Id, 
            ExercicioId, 
            Exercicio?.ToDto(), 
            Series, 
            Repeticoes, 
            Carga, 
            Pausa, 
            Observacoes, 
            Ordem
        );
    }
}
