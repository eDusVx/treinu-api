using FluentResults;
using Treinu.Domain.Core;

namespace Treinu.Domain.Entities;

public class ExecucaoTreino : AggregateRoot
{
    private readonly List<ExecucaoExercicio> _exercicios = new();

    protected ExecucaoTreino() { }

    private ExecucaoTreino(Guid id) : base(id) { }

    public Guid TreinoId { get; private set; }
    public Guid AlunoId { get; private set; }
    public DateTime DataInicio { get; private set; }
    public DateTime? DataFim { get; private set; }
    public bool Concluido { get; private set; }
    
    // Feedback parameters
    public int? NotaFeedback { get; private set; }
    public string? ComentarioFeedback { get; private set; }

    public IReadOnlyCollection<ExecucaoExercicio> Exercicios => _exercicios.AsReadOnly();

    public static Result<ExecucaoTreino> Iniciar(Guid treinoId, Guid alunoId)
    {
        var instance = new ExecucaoTreino(Guid.NewGuid())
        {
            TreinoId = treinoId,
            AlunoId = alunoId,
            DataInicio = DateTime.UtcNow,
            Concluido = false
        };

        return Result.Ok(instance);
    }

    public Result RegistrarExercicio(Guid itemTreinoId, int seriesRealizadas, int repeticoesRealizadas, decimal cargaUtilizada)
    {
        if (Concluido) return Result.Fail("Treino já está concluído.");

        var exercicio = new ExecucaoExercicio(Guid.NewGuid(), Id, itemTreinoId, seriesRealizadas, repeticoesRealizadas, cargaUtilizada);
        _exercicios.Add(exercicio);

        return Result.Ok();
    }

    public Result Concluir(int? notaFeedback = null, string? comentarioFeedback = null)
    {
        if (Concluido) return Result.Fail("Treino já está concluído.");

        if (notaFeedback.HasValue && (notaFeedback < 0 || notaFeedback > 5))
            return Result.Fail("A nota de feedback deve ser entre 0 e 5.");

        Concluido = true;
        DataFim = DateTime.UtcNow;
        NotaFeedback = notaFeedback;
        ComentarioFeedback = comentarioFeedback;

        return Result.Ok();
    }
}
