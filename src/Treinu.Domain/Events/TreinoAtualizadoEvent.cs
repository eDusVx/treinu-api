using Treinu.Domain.Core;

namespace Treinu.Domain.Events;

public class TreinoAtualizadoEvent : IDomainEvent
{
    public Guid TreinoId { get; }
    public Guid AlunoId { get; }
    public string NomeTreino { get; }

    public TreinoAtualizadoEvent(Guid treinoId, Guid alunoId, string nomeTreino)
    {
        TreinoId = treinoId;
        AlunoId = alunoId;
        NomeTreino = nomeTreino;
    }
}
