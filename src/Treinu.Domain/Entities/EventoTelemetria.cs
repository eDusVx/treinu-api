using Treinu.Domain.Core;
using Treinu.Domain.Enums;

namespace Treinu.Domain.Entities;

public class EventoTelemetria : AggregateRoot
{
    protected EventoTelemetria() { }

    private EventoTelemetria(Guid id) : base(id) { }

    public Guid? UsuarioId { get; private set; }
    public virtual Usuario? Usuario { get; private set; }
    public TipoInteracaoEnum Tipo { get; private set; }
    public DateTime DataOcorrencia { get; private set; }

    public static EventoTelemetria Criar(Guid? usuarioId, TipoInteracaoEnum tipo)
    {
        return new EventoTelemetria(Guid.NewGuid())
        {
            UsuarioId = usuarioId,
            Tipo = tipo,
            DataOcorrencia = DateTime.UtcNow
        };
    }
}
