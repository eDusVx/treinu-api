using Treinu.Domain.Core.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Treinu.Contracts.Commands.Plataforma;
using Treinu.Contracts.Queries.Plataforma;

namespace Treinu.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class PlataformaController(IMediator mediator) : ControllerBase
{
    [HttpPost("sugestoes")]
    public async Task<IActionResult> EnviarSugestao([FromBody] EnviarSugestaoRequest request)
    {
        var usuarioIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(usuarioIdClaim, out var usuarioId)) return Unauthorized();

        var command = new EnviarSugestaoCommand(usuarioId, request.Titulo, request.Descricao);
        var result = await mediator.Send(command);

        if (result.IsFailed)
            return BadRequest(result.Errors.Select(e => e.Message));

        return Ok(new { Mensagem = "Sugestão enviada com sucesso!" });
    }

    [HttpPost("avaliacoes")]
    public async Task<IActionResult> RegistrarAvaliacao([FromBody] RegistrarAvaliacaoRequest request)
    {
        var usuarioIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(usuarioIdClaim, out var usuarioId)) return Unauthorized();

        var command = new RegistrarAvaliacaoPlataformaCommand(usuarioId, request.Nota, request.Comentario);
        var result = await mediator.Send(command);

        if (result.IsFailed)
            return BadRequest(result.Errors.Select(e => e.Message));

        return Ok(new { Mensagem = "Avaliação registrada com sucesso!" });
    }

    [HttpGet("ranking")]
    [AllowAnonymous] // ou [Authorize]
    public async Task<IActionResult> ObterRanking([FromQuery] Guid? treinadorId)
    {
        var query = new ObterRankingAlunosQuery(treinadorId);
        var result = await mediator.Send(query);

        if (result.IsFailed)
            return BadRequest(result.Errors.Select(e => e.Message));

        return Ok(result.Value);
    }
}

public record EnviarSugestaoRequest(string Titulo, string Descricao);
public record RegistrarAvaliacaoRequest(int Nota, string? Comentario);
