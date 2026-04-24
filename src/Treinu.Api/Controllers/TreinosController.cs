using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Treinu.Api.Controllers.Base;
using Treinu.Contracts.Commands.Treinos;
using Treinu.Contracts.Queries.Treinos;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Enums;

namespace Treinu.Api.Controllers;

[ApiController]
[Route("api/treinos")]
[Authorize]
public class TreinosController(IMediator mediator) : ApiController
{
    [HttpPost]
    [Authorize(Roles = $"{RoleConstants.Treinador},{RoleConstants.Admin}")]
    [ProducesResponseType(typeof(object), 201)]
    [ProducesResponseType(typeof(ProblemDetails), 400)]
    public async Task<IActionResult> CriarTreino([FromBody] CriarTreinoCommand command)
    {
        var result = await mediator.Send(command);
        if (result.IsFailed) return HandleFailure(result);
        return CreatedAtAction(nameof(ListarTreinos), new { }, result.Value);
    }

    [HttpGet]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 400)]
    public async Task<IActionResult> ListarTreinos(
        [FromQuery] Guid? alunoId,
        [FromQuery] Guid? treinadorId,
        [FromQuery] TreinoStatusEnum? status)
    {
        var query = new FiltrarTreinosQuery(alunoId, treinadorId, status);
        var result = await mediator.Send(query);
        if (result.IsFailed) return HandleFailure(result);
        return Ok(result.Value);
    }
}
