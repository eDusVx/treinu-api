using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Treinu.Api.Controllers.Base;
using Treinu.Contracts.Commands;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Enums;

namespace Treinu.Api.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = RoleConstants.Admin)]
public class AdminController(IMediator mediator) : ApiController
{
    [HttpPatch("trainers/{id:guid}/approve")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(ProblemDetails), 400)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    public async Task<IActionResult> AprovarTreinador(Guid id)
    {
        var command = new AprovarTreinadorCommand(id);
        var result = await mediator.Send(command);

        if (result.IsFailed) return HandleFailure(result);

        return NoContent();
    }
}