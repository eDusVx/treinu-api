using FluentResults;
using Treinu.Domain.Core;

namespace Treinu.Domain.Entities.Chat;

public enum TipoMensagem
{
    Texto = 1,
    Anexo = 2
}

public class MensagemChat : Entity
{
    protected MensagemChat() { }

    private MensagemChat(Guid id) : base(id) { }

    public Guid SalaChatId { get; private set; }
    public virtual SalaChat Sala { get; private set; } = null!;
    
    public Guid RemetenteId { get; private set; }
    public virtual Usuario Remetente { get; private set; } = null!;
    
    public TipoMensagem Tipo { get; private set; }
    public string Conteudo { get; private set; } = string.Empty;
    public DateTime DataEnvio { get; private set; }

    internal static Result<MensagemChat> Criar(Guid salaChatId, Guid remetenteId, string conteudo, TipoMensagem tipo)
    {
        if (string.IsNullOrWhiteSpace(conteudo))
            return Result.Fail("Conteúdo da mensagem não pode ser vazio.");

        var mensagem = new MensagemChat(Guid.NewGuid())
        {
            SalaChatId = salaChatId,
            RemetenteId = remetenteId,
            Conteudo = conteudo.Trim(),
            Tipo = tipo,
            DataEnvio = DateTime.UtcNow
        };

        return Result.Ok(mensagem);
    }
}
