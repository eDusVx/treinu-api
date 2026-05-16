using FluentResults;
using Treinu.Domain.Core;

namespace Treinu.Domain.Entities;

public class Sugestao : AggregateRoot
{
    protected Sugestao() { }

    private Sugestao(Guid id) : base(id) { }

    public Guid UsuarioId { get; private set; }
    public string Titulo { get; private set; } = string.Empty;
    public string Descricao { get; private set; } = string.Empty;
    public DateTime DataCriacao { get; private set; }
    public bool Lido { get; private set; }

    public static Result<Sugestao> Criar(Guid usuarioId, string titulo, string descricao)
    {
        if (string.IsNullOrWhiteSpace(titulo)) return Result.Fail("Título é obrigatório.");
        if (string.IsNullOrWhiteSpace(descricao)) return Result.Fail("Descrição é obrigatória.");

        var instance = new Sugestao(Guid.NewGuid())
        {
            UsuarioId = usuarioId,
            Titulo = titulo,
            Descricao = descricao,
            DataCriacao = DateTime.UtcNow,
            Lido = false
        };

        return Result.Ok(instance);
    }

    public Result MarcarComoLido()
    {
        Lido = true;
        return Result.Ok();
    }
}
