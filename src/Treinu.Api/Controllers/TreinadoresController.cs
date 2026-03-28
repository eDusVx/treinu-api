using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Treinu.Api.Controllers.Base;
using Treinu.Contracts.Commands;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Enums;

namespace Treinu.Api.Controllers;

[ApiController]
[Route("api/trainers")]
public class TreinadoresController(IMediator mediator) : ApiController
{
    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(typeof(object), 201)]
    [ProducesResponseType(typeof(ProblemDetails), 400)]
    public async Task<IActionResult> RegistrarTreinador([FromBody] RegistrarTreinadorCommand command)
    {
        var result = await mediator.Send(command);
        if (result.IsFailed) return HandleFailure(result);
        return CreatedAtAction(nameof(RegistrarTreinador), result.Value);
    }

    [Authorize(Roles = $"{RoleConstants.Treinador},{RoleConstants.Admin}")]
    [HttpPost("{id:guid}/contatos")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 400)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    public async Task<IActionResult> AdicionarContato(Guid id, [FromBody] AdicionarContatoTreinadorCommand command)
    {
        var cmd = command with { TreinadorId = id };
        var result = await mediator.Send(cmd);
        if (result.IsFailed) return HandleFailure(result);
        return Ok(result.Value);
    }

    [Authorize(Roles = $"{RoleConstants.Treinador},{RoleConstants.Admin}")]
    [HttpDelete("{id:guid}/contatos/{contatoId:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    public async Task<IActionResult> RemoverContato(Guid id, Guid contatoId)
    {
        var command = new RemoverContatoTreinadorCommand(id, contatoId);
        var result = await mediator.Send(command);
        if (result.IsFailed) return HandleFailure(result);
        return NoContent();
    }

    [Authorize(Roles = $"{RoleConstants.Treinador},{RoleConstants.Admin}")]
    [HttpPost("{id:guid}/especializacoes")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 400)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    public async Task<IActionResult> AdicionarEspecializacao(Guid id,
        [FromBody] AdicionarEspecializacaoTreinadorCommand command)
    {
        var cmd = command with { TreinadorId = id };
        var result = await mediator.Send(cmd);
        if (result.IsFailed) return HandleFailure(result);
        return Ok(result.Value);
    }

    [Authorize(Roles = $"{RoleConstants.Treinador},{RoleConstants.Admin}")]
    [HttpDelete("{id:guid}/especializacoes")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    public async Task<IActionResult> RemoverEspecializacao(Guid id,
        [FromBody] RemoverEspecializacaoTreinadorCommand command)
    {
        var cmd = command with { TreinadorId = id };
        var result = await mediator.Send(cmd);
        if (result.IsFailed) return HandleFailure(result);
        return NoContent();
    }

    [Authorize(Roles = $"{RoleConstants.Treinador},{RoleConstants.Admin}")]
    [HttpPost("{id:guid}/certificados")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 400)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    public async Task<IActionResult> AdicionarCertificado(Guid id,
        [FromBody] AdicionarCertificadoTreinadorCommand command)
    {
        var cmd = command with { TreinadorId = id };
        var result = await mediator.Send(cmd);
        if (result.IsFailed) return HandleFailure(result);
        return Ok(result.Value);
    }

    [Authorize(Roles = $"{RoleConstants.Treinador},{RoleConstants.Admin}")]
    [HttpDelete("{id:guid}/certificados/{certificadoId:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    public async Task<IActionResult> RemoverCertificado(Guid id, Guid certificadoId)
    {
        var command = new RemoverCertificadoTreinadorCommand(id, certificadoId);
        var result = await mediator.Send(command);
        if (result.IsFailed) return HandleFailure(result);
        return NoContent();
    }
}