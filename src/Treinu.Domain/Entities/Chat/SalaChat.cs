using FluentResults;
using Treinu.Domain.Core;

namespace Treinu.Domain.Entities.Chat;

public enum TipoSalaChat
{
    Direta = 1,
    Grupo = 2
}

public class SalaChat : AggregateRoot
{
    private readonly List<ParticipanteSala> _participantes = new();
    private readonly List<MensagemChat> _mensagens = new();

    protected SalaChat() { }

    private SalaChat(Guid id) : base(id) { }

    public string Nome { get; private set; } = string.Empty;
    public TipoSalaChat Tipo { get; private set; }
    public Guid? CriadorId { get; private set; }
    
    public IReadOnlyCollection<ParticipanteSala> Participantes => _participantes.AsReadOnly();
    public IReadOnlyCollection<MensagemChat> Mensagens => _mensagens.AsReadOnly();

    public static Result<SalaChat> CriarDireta(Guid usuario1Id, Guid usuario2Id)
    {
        var sala = new SalaChat(Guid.NewGuid())
        {
            Tipo = TipoSalaChat.Direta,
            Nome = "Chat Direto"
        };

        sala.AdicionarParticipante(usuario1Id, false);
        sala.AdicionarParticipante(usuario2Id, false);

        return Result.Ok(sala);
    }

    public static Result<SalaChat> CriarGrupo(string nome, Guid criadorId, List<Guid> membrosIds)
    {
        if (string.IsNullOrWhiteSpace(nome)) return Result.Fail("Nome do grupo é obrigatório.");

        var sala = new SalaChat(Guid.NewGuid())
        {
            Tipo = TipoSalaChat.Grupo,
            Nome = nome,
            CriadorId = criadorId
        };

        sala.AdicionarParticipante(criadorId, true);

        foreach (var membroId in membrosIds.Distinct())
        {
            if (membroId != criadorId)
                sala.AdicionarParticipante(membroId, false);
        }

        return Result.Ok(sala);
    }

    public Result AdicionarParticipante(Guid usuarioId, bool isAdmin = false)
    {
        if (_participantes.Any(p => p.UsuarioId == usuarioId))
            return Result.Fail("Usuário já é participante desta sala.");

        var participante = ParticipanteSala.Criar(Id, usuarioId, isAdmin);
        _participantes.Add(participante);
        return Result.Ok();
    }

    public Result RemoverParticipante(Guid usuarioId)
    {
        var participante = _participantes.FirstOrDefault(p => p.UsuarioId == usuarioId);
        if (participante == null) return Result.Fail("Participante não encontrado.");

        _participantes.Remove(participante);
        return Result.Ok();
    }

    public Result<MensagemChat> AdicionarMensagem(Guid remetenteId, string conteudo, TipoMensagem tipo = TipoMensagem.Texto)
    {
        if (!_participantes.Any(p => p.UsuarioId == remetenteId))
            return Result.Fail("Remetente não é um participante da sala.");

        var mensagemResult = MensagemChat.Criar(Id, remetenteId, conteudo, tipo);
        if (mensagemResult.IsFailed) return Result.Fail<MensagemChat>(mensagemResult.Errors);

        _mensagens.Add(mensagemResult.Value);

        // Incrementar contador de não lidas para os outros participantes
        foreach (var participante in _participantes.Where(p => p.UsuarioId != remetenteId))
        {
            participante.IncrementarMensagensNaoLidas();
        }

        return Result.Ok(mensagemResult.Value);
    }

    public Result MarcarMensagensComoLidas(Guid usuarioId)
    {
        var participante = _participantes.FirstOrDefault(p => p.UsuarioId == usuarioId);
        if (participante == null) return Result.Fail("Participante não encontrado na sala.");

        participante.ZerarMensagensNaoLidas();
        return Result.Ok();
    }
}
