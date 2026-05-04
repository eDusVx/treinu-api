using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Treinu.Api.Controllers.Base;
using Treinu.Contracts.Commands.Chat;
using Treinu.Contracts.Queries.Chat;
using Treinu.Domain.Core.Mediator;

namespace Treinu.Api.Controllers;

[ApiController]
[Route("api/chat")]
[Authorize]
public class ChatController(IMediator mediator) : ApiController
{
    private Guid GetUsuarioId()
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (Guid.TryParse(userIdString, out var userId))
            return userId;
        throw new UnauthorizedAccessException("Usuário não autenticado ou ID inválido.");
    }

    [HttpPost("salas")]
    public async Task<IActionResult> CriarSala([FromBody] CriarSalaRequest request)
    {
        var command = new CriarSalaChatCommand(GetUsuarioId(), request.Nome, request.ParticipantesIds, request.EhGrupo);
        var result = await mediator.Send(command);
        if (result.IsFailed) return HandleFailure(result);
        return Ok(result.Value);
    }

    [HttpGet("salas")]
    public async Task<IActionResult> ListarSalas()
    {
        var query = new ListarSalasChatQuery(GetUsuarioId());
        var result = await mediator.Send(query);
        if (result.IsFailed) return HandleFailure(result);
        return Ok(result.Value);
    }

    [HttpGet("salas/{salaId:guid}/mensagens")]
    public async Task<IActionResult> ListarMensagens(Guid salaId, [FromQuery] int page = 1, [FromQuery] int limit = 50)
    {
        var query = new ListarMensagensChatQuery(salaId, GetUsuarioId(), page, limit);
        var result = await mediator.Send(query);
        if (result.IsFailed) return HandleFailure(result);
        return Ok(result.Value);
    }

    [HttpPost("salas/{salaId:guid}/mensagens")]
    public async Task<IActionResult> EnviarMensagem(Guid salaId, [FromBody] EnviarMensagemRequest request)
    {
        var command = new EnviarMensagemChatCommand(salaId, GetUsuarioId(), request.Conteudo, User.Identity?.Name ?? "Usuário");
        var result = await mediator.Send(command);
        if (result.IsFailed) return HandleFailure(result);
        return Ok(result.Value);
    }

    [HttpPost("salas/{salaId:guid}/lidas")]
    public async Task<IActionResult> MarcarComoLida(Guid salaId)
    {
        var command = new MarcarMensagensComoLidasCommand(salaId, GetUsuarioId());
        var result = await mediator.Send(command);
        if (result.IsFailed) return HandleFailure(result);
        return NoContent();
    }

    [HttpPost("salas/{salaId:guid}/membros")]
    public async Task<IActionResult> AdicionarMembro(Guid salaId, [FromBody] Guid novoMembroId)
    {
        var command = new AdicionarMembroSalaChatCommand(salaId, GetUsuarioId(), novoMembroId);
        var result = await mediator.Send(command);
        if (result.IsFailed) return HandleFailure(result);
        return NoContent();
    }

    [HttpDelete("salas/{salaId:guid}/membros/{membroId:guid}")]
    public async Task<IActionResult> RemoverMembro(Guid salaId, Guid membroId)
    {
        var command = new RemoverMembroSalaChatCommand(salaId, GetUsuarioId(), membroId);
        var result = await mediator.Send(command);
        if (result.IsFailed) return HandleFailure(result);
        return NoContent();
    }

    [HttpGet("notificacoes/nao-lidas")]
    public async Task<IActionResult> ObterTotalNaoLidas()
    {
        var query = new ListarSalasChatQuery(GetUsuarioId());
        var result = await mediator.Send(query);
        if (result.IsFailed) return HandleFailure(result);

        var salas = result.Value as List<SalaChatDto>;
        var total = salas?.Sum(s => s.NaoLidas) ?? 0;
        return Ok(new { Total = total });
    }

    [HttpGet("usuarios/{outroUsuarioId:guid}/sala")]
    public async Task<IActionResult> ObterOuCriarSalaDireta(Guid outroUsuarioId)
    {
        var command = new ObterOuCriarSalaDiretaCommand(GetUsuarioId(), outroUsuarioId);
        var result = await mediator.Send(command);
        if (result.IsFailed) return HandleFailure(result);
        return Ok(result.Value);
    }
}

public record CriarSalaRequest(string Nome, List<Guid> ParticipantesIds, bool EhGrupo);
public record EnviarMensagemRequest(string Conteudo);
