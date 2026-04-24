using Treinu.Domain.Core;

namespace Treinu.Domain.Events;

public class TreinoAtribuidoEvent : IDomainEvent
{
    public TreinoAtribuidoEvent(Guid treinoId, Guid alunoId, Guid treinadorId, string nomeTreino)
    {
        TreinoId = treinoId;
        AlunoId = alunoId;
        TreinadorId = treinadorId;
        NomeTreino = nomeTreino;
        DataOcorrencia = DateTime.UtcNow;
    }

    public Guid TreinoId { get; }
    public Guid AlunoId { get; }
    public Guid TreinadorId { get; }
    public string NomeTreino { get; }
    public DateTime DataOcorrencia { get; }
}
