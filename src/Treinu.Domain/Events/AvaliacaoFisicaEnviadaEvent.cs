using Treinu.Domain.Core;

namespace Treinu.Domain.Events;

public class AvaliacaoFisicaEnviadaEvent : IDomainEvent
{
    public AvaliacaoFisicaEnviadaEvent(Guid avaliacaoId, Guid alunoId, Guid? treinadorId)
    {
        AvaliacaoId = avaliacaoId;
        AlunoId = alunoId;
        TreinadorId = treinadorId;
        DataOcorrencia = DateTime.UtcNow;
    }

    public Guid AvaliacaoId { get; }
    public Guid AlunoId { get; }
    public Guid? TreinadorId { get; }
    public DateTime DataOcorrencia { get; }
}
