using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Treinu.Api.Controllers.Base;
using Treinu.Contracts.Commands.Exercicios;
using Treinu.Contracts.Queries.Exercicios;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Enums;

namespace Treinu.Api.Controllers;

[ApiController]
[Route("api/exercicios")]
[Authorize(Roles = $"{RoleConstants.Treinador},{RoleConstants.Admin}")]
public class ExerciciosController(IMediator mediator) : ApiController
{
    [HttpPost]
    [ProducesResponseType(typeof(object), 201)]
    [ProducesResponseType(typeof(ProblemDetails), 400)]
    public async Task<IActionResult> RegistrarExercicio([FromBody] RegistrarExercicioCommand command)
    {
        var result = await mediator.Send(command);
        if (result.IsFailed) return HandleFailure(result);
        return CreatedAtAction(nameof(BuscarExercicios), new { tags = string.Empty }, result.Value);
    }

    [HttpGet]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 400)]
    public async Task<IActionResult> BuscarExercicios([FromQuery] Guid treinadorId, [FromQuery] string? tags)
    {
        var query = new BuscarExerciciosQuery(treinadorId, tags);
        var result = await mediator.Send(query);
        if (result.IsFailed) return HandleFailure(result);
        return Ok(result.Value);
    }
}
