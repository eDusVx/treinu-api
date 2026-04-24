using FluentResults;
using Treinu.Domain.Core;

namespace Treinu.Domain.Entities;

public record CriarNotificacaoProps(
    Guid UsuarioId,
    string Titulo,
    string Mensagem
);

public class Notificacao : AggregateRoot
{
    protected Notificacao() { }

    private Notificacao(Guid id) : base(id) { }

    public Guid UsuarioId { get; private set; }
    public string Titulo { get; private set; } = string.Empty;
    public string Mensagem { get; private set; } = string.Empty;
    public bool Lida { get; private set; }
    public DateTime CriadaEm { get; private set; }

    public static Result<Notificacao> Criar(CriarNotificacaoProps props)
    {
        var instance = new Notificacao(Guid.NewGuid())
        {
            UsuarioId = props.UsuarioId,
            Lida = false,
            CriadaEm = DateTime.UtcNow
        };

        var tituloResult = instance.SetTitulo(props.Titulo);
        var mensagemResult = instance.SetMensagem(props.Mensagem);

        var merged = Result.Merge(tituloResult, mensagemResult);
        if (merged.IsFailed) return Result.Fail<Notificacao>(merged.Errors);

        return Result.Ok(instance);
    }

    private Result SetTitulo(string titulo)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            return Result.Fail("O título da notificação não pode ser vazio.");
        
        Titulo = titulo.Trim();
        return Result.Ok();
    }

    private Result SetMensagem(string mensagem)
    {
        if (string.IsNullOrWhiteSpace(mensagem))
            return Result.Fail("A mensagem da notificação não pode ser vazia.");
        
        Mensagem = mensagem.Trim();
        return Result.Ok();
    }

    public Result MarcarComoLida()
    {
        Lida = true;
        return Result.Ok();
    }

    public Treinu.Domain.Dtos.NotificacaoDto ToDto()
    {
        return new Treinu.Domain.Dtos.NotificacaoDto(
            Id, UsuarioId, Titulo, Mensagem, Lida, CriadaEm
        );
    }
}
