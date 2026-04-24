using Treinu.Domain.Core;

namespace Treinu.Domain.Events;

public class TreinoVencidoEvent : IDomainEvent
{
    public TreinoVencidoEvent(Guid treinoId, Guid alunoId, string nomeTreino)
    {
        TreinoId = treinoId;
        AlunoId = alunoId;
        NomeTreino = nomeTreino;
        DataOcorrencia = DateTime.UtcNow;
    }

    public Guid TreinoId { get; }
    public Guid AlunoId { get; }
    public string NomeTreino { get; }
    public DateTime DataOcorrencia { get; }
}
