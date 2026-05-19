using Treinu.Domain.Core.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Treinu.Contracts.Commands.ExecucoesTreino;

namespace Treinu.Api.Controllers;

[ApiController]
[Route("api/v1/execucoes-treino")]
[Authorize]
public class ExecucaoTreinoController(IMediator mediator) : ControllerBase
{
    [HttpPost("iniciar")]
    public async Task<IActionResult> Iniciar([FromBody] IniciarExecucaoTreinoRequest request)
    {
        var usuarioIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(usuarioIdClaim, out var alunoId)) return Unauthorized();

        var command = new IniciarExecucaoTreinoCommand(request.TreinoId, alunoId);
        var result = await mediator.Send(command);

        if (result.IsFailed)
            return BadRequest(result.Errors.Select(e => e.Message));

        return Ok(new { ExecucaoTreinoId = result.Value });
    }

    [HttpPost("{id}/exercicios")]
    public async Task<IActionResult> RegistrarExercicio(Guid id, [FromBody] RegistrarExercicioExecutadoRequest request)
    {
        var command = new RegistrarExercicioExecutadoCommand(id, request.ItemTreinoId, request.SeriesRealizadas, request.RepeticoesRealizadas, request.CargaUtilizada);
        var result = await mediator.Send(command);

        if (result.IsFailed)
            return BadRequest(result.Errors.Select(e => e.Message));

        return Ok();
    }

    [HttpPost("{id}/concluir")]
    public async Task<IActionResult> Concluir(Guid id, [FromBody] ConcluirExecucaoTreinoRequest request)
    {
        var command = new ConcluirExecucaoTreinoCommand(id, request.NotaFeedback, request.ComentarioFeedback);
        var result = await mediator.Send(command);

        if (result.IsFailed)
            return BadRequest(result.Errors.Select(e => e.Message));

        return Ok(new { Mensagem = "Treino concluído com sucesso!" });
    }

    [HttpGet("historico/{alunoId}")]
    public async Task<IActionResult> ObterHistorico(Guid alunoId)
    {
        var query = new Treinu.Contracts.Queries.ExecucoesTreino.ObterHistoricoExecucoesQuery(alunoId);
        var result = await mediator.Send(query);

        if (result.IsFailed)
            return BadRequest(result.Errors.Select(e => e.Message));

        return Ok(result.Value);
    }

    [HttpGet("ativas")]
    public async Task<IActionResult> ObterExecucaoAtiva([FromQuery] Guid treinoId)
    {
        var usuarioIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(usuarioIdClaim, out var alunoId)) return Unauthorized();

        var query = new Treinu.Contracts.Queries.ExecucoesTreino.ObterExecucaoAtivaQuery(alunoId, treinoId);
        var result = await mediator.Send(query);

        if (result.IsFailed)
            return BadRequest(result.Errors.Select(e => e.Message));

        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterDetalhes(Guid id)
    {
        var query = new Treinu.Contracts.Queries.ExecucoesTreino.ObterDetalhesExecucaoQuery(id);
        var result = await mediator.Send(query);

        if (result.IsFailed)
            return BadRequest(result.Errors.Select(e => e.Message));

        return Ok(result.Value);
    }
}

public record IniciarExecucaoTreinoRequest(Guid TreinoId);
public record RegistrarExercicioExecutadoRequest(Guid ItemTreinoId, int SeriesRealizadas, int RepeticoesRealizadas, decimal CargaUtilizada);
public record ConcluirExecucaoTreinoRequest(int? NotaFeedback, string? ComentarioFeedback);
