using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Treinu.Api.Controllers.Base;
using Treinu.Contracts.Commands.Convites;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Enums;

namespace Treinu.Api.Controllers;

[ApiController]
[Route("api/convites")]
public class ConvitesController(IMediator mediator) : ApiController
{
    [Authorize(Roles = $"{RoleConstants.Treinador},{RoleConstants.Admin}")]
    [HttpPost("aluno")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(ProblemDetails), 400)]
    [ProducesResponseType(typeof(ProblemDetails), 403)]
    public async Task<IActionResult> ConvidarAluno([FromBody] ConvidarAlunoCommand command)
    {
        var result = await mediator.Send(command);
        if (result.IsFailed) return HandleFailure(result);
        return Ok();
    }
}