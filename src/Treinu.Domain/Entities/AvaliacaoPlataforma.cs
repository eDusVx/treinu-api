using FluentResults;
using Treinu.Domain.Core;

namespace Treinu.Domain.Entities;

public class AvaliacaoPlataforma : AggregateRoot
{
    protected AvaliacaoPlataforma() { }

    private AvaliacaoPlataforma(Guid id) : base(id) { }

    public Guid UsuarioId { get; private set; }
    public int Nota { get; private set; }
    public string? Comentario { get; private set; }
    public DateTime DataCriacao { get; private set; }

    public static Result<AvaliacaoPlataforma> Criar(Guid usuarioId, int nota, string? comentario)
    {
        if (nota < 0 || nota > 10) return Result.Fail("Nota deve ser entre 0 e 10.");

        var instance = new AvaliacaoPlataforma(Guid.NewGuid())
        {
            UsuarioId = usuarioId,
            Nota = nota,
            Comentario = comentario,
            DataCriacao = DateTime.UtcNow
        };

        return Result.Ok(instance);
    }
}
