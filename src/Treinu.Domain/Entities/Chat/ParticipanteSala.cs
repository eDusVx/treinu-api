using FluentResults;
using Treinu.Domain.Core;

namespace Treinu.Domain.Entities.Chat;

public class ParticipanteSala : Entity
{
    protected ParticipanteSala() { }

    private ParticipanteSala(Guid id) : base(id) { }

    public Guid SalaChatId { get; private set; }
    public virtual SalaChat Sala { get; private set; } = null!;
    
    public Guid UsuarioId { get; private set; }
    public virtual Usuario Usuario { get; private set; } = null!;
    
    public int MensagensNaoLidas { get; private set; }
    public bool IsAdmin { get; private set; }
    public DateTime DataEntrada { get; private set; }

    internal static ParticipanteSala Criar(Guid salaChatId, Guid usuarioId, bool isAdmin)
    {
        return new ParticipanteSala(Guid.NewGuid())
        {
            SalaChatId = salaChatId,
            UsuarioId = usuarioId,
            IsAdmin = isAdmin,
            DataEntrada = DateTime.UtcNow,
            MensagensNaoLidas = 0
        };
    }

    internal void IncrementarMensagensNaoLidas()
    {
        MensagensNaoLidas++;
    }

    internal void ZerarMensagensNaoLidas()
    {
        MensagensNaoLidas = 0;
    }
}
