using Treinu.Domain.Core;

namespace Treinu.Domain.Entities;

public class ExecucaoExercicio : Entity
{
    protected ExecucaoExercicio() { }

    internal ExecucaoExercicio(Guid id, Guid execucaoTreinoId, Guid itemTreinoId, int series, int repeticoes, decimal carga) : base(id)
    {
        ExecucaoTreinoId = execucaoTreinoId;
        ItemTreinoId = itemTreinoId;
        SeriesRealizadas = series;
        RepeticoesRealizadas = repeticoes;
        CargaUtilizada = carga;
    }

    public Guid ExecucaoTreinoId { get; private set; }
    public Guid ItemTreinoId { get; private set; }
    public int SeriesRealizadas { get; private set; }
    public int RepeticoesRealizadas { get; private set; }
    public decimal CargaUtilizada { get; private set; }
}
