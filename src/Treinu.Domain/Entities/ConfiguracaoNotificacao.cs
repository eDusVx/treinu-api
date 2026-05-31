using FluentResults;
using Treinu.Domain.Core;

namespace Treinu.Domain.Entities;

public class ConfiguracaoNotificacao : Entity
{
    public Guid UsuarioId { get; private set; }
    public bool ReceberEmail { get; private set; }
    public bool ReceberPush { get; private set; }
    public bool AlertaVencimentoAvaliacao { get; private set; }
    public bool AlertaVencimentoTreino { get; private set; }
    public bool AlertaNovoTreino { get; private set; }

    protected ConfiguracaoNotificacao() { }

    private ConfiguracaoNotificacao(Guid id, Guid usuarioId) : base(id)
    {
        UsuarioId = usuarioId;
        ReceberEmail = true;
        ReceberPush = true;
        AlertaVencimentoAvaliacao = true;
        AlertaVencimentoTreino = true;
        AlertaNovoTreino = true;
    }

    public static ConfiguracaoNotificacao CriarPadrao(Guid usuarioId)
    {
        return new ConfiguracaoNotificacao(Guid.NewGuid(), usuarioId);
    }

    public Result Atualizar(bool email, bool push, bool vencAvaliacao, bool vencTreino, bool novoTreino)
    {
        ReceberEmail = email;
        ReceberPush = push;
        AlertaVencimentoAvaliacao = vencAvaliacao;
        AlertaVencimentoTreino = vencTreino;
        AlertaNovoTreino = novoTreino;
        
        return Result.Ok();
    }
}
