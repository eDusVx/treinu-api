using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Treinu.Api.Controllers.Base;
using Treinu.Contracts.Commands.Alunos;
using Treinu.Contracts.Commands.Usuarios;
using Treinu.Contracts.Queries.Alunos;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Enums;
using System.Security.Claims;

namespace Treinu.Api.Controllers;

[ApiController]
[Route("api/aluno")]
public class AlunosController(IMediator mediator) : ApiController
{
    [AllowAnonymous]
    [HttpPost("register")]
    [ProducesResponseType(typeof(object), 201)]
    [ProducesResponseType(typeof(ProblemDetails), 400)]
    public async Task<IActionResult> RegistrarAluno([FromBody] RegistrarAlunoCommand command)
    {
        var result = await mediator.Send(command);
        if (result.IsFailed) return HandleFailure(result);
        return CreatedAtAction(nameof(RegistrarAluno), result.Value);
    }

    [Authorize(Roles = $"{RoleConstants.Aluno},{RoleConstants.Admin}")]
    [HttpPost("{id:guid}/contatos")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 400)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    public async Task<IActionResult> AdicionarContato(Guid id, [FromBody] AdicionarContatoAlunoCommand command)
    {
        var cmd = command with { AlunoId = id };
        var result = await mediator.Send(cmd);
        if (result.IsFailed) return HandleFailure(result);
        return Ok(result.Value);
    }

    [Authorize(Roles = $"{RoleConstants.Aluno},{RoleConstants.Admin}")]
    [HttpDelete("{id:guid}/contatos/{contatoId:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    public async Task<IActionResult> RemoverContato(Guid id, Guid contatoId)
    {
        var command = new RemoverContatoAlunoCommand(id, contatoId);
        var result = await mediator.Send(command);
        if (result.IsFailed) return HandleFailure(result);
        return NoContent();
    }

    [Authorize(Roles = $"{RoleConstants.Treinador},{RoleConstants.Admin}")]
    [HttpPost("{id:guid}/avaliacoes")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 400)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    public async Task<IActionResult> AdicionarAvaliacaoFisica(Guid id,
        [FromBody] AdicionarAvaliacaoFisicaAlunoCommand command)
    {
        var cmd = command with { AlunoId = id };
        var result = await mediator.Send(cmd);
        if (result.IsFailed) return HandleFailure(result);
        return Ok(result.Value);
    }

    [Authorize(Roles = $"{RoleConstants.Treinador},{RoleConstants.Admin}")]
    [HttpDelete("{id:guid}/avaliacoes/{avaliacaoId:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    public async Task<IActionResult> RemoverAvaliacaoFisica(Guid id, Guid avaliacaoId)
    {
        var command = new RemoverAvaliacaoFisicaAlunoCommand(id, avaliacaoId);
        var result = await mediator.Send(command);
        if (result.IsFailed) return HandleFailure(result);
        return NoContent();
    }

    [Authorize(Roles = RoleConstants.Aluno)]
    [HttpPost("me/avaliacoes")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 400)]
    public async Task<IActionResult> AdicionarMinhaAvaliacaoFisica([FromBody] AdicionarAvaliacaoFisicaProprioAlunoCommand command)
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var alunoId))
            return Unauthorized();

        var cmd = command with { AlunoId = alunoId };
        var result = await mediator.Send(cmd);
        if (result.IsFailed) return HandleFailure(result);
        return Ok(result.Value);
    }

    [Authorize(Roles = RoleConstants.Aluno)]
    [HttpGet("me/dashboard")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 400)]
    public async Task<IActionResult> ObterMeuDashboardEvolucao()
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var alunoId))
            return Unauthorized();

        var query = new ObterDashboardEvolucaoQuery(alunoId);
        var result = await mediator.Send(query);
        if (result.IsFailed) return HandleFailure(result);
        return Ok(result.Value);
    }

    [Authorize(Roles = $"{RoleConstants.Treinador},{RoleConstants.Admin}")]
    [HttpGet("{id:guid}/dashboard")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 400)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    public async Task<IActionResult> ObterDashboardEvolucao(Guid id)
    {
        var query = new ObterDashboardEvolucaoQuery(id);
        var result = await mediator.Send(query);
        if (result.IsFailed) return HandleFailure(result);
        return Ok(result.Value);
    }
}