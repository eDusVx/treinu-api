using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Treinu.Api.Controllers.Base;
using Treinu.Contracts.Queries.Usuarios;
using Treinu.Domain.Core.Mediator;
using System.Security.Claims;

namespace Treinu.Api.Controllers;

[ApiController]
[Route("api/notificacoes")]
[Authorize]
public class NotificacoesController(IMediator mediator) : ApiController
{
    [HttpGet]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 400)]
    public async Task<IActionResult> BuscarNotificacoes()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var usuarioId))
            return Unauthorized();

        var query = new BuscarNotificacoesQuery(usuarioId);
        var result = await mediator.Send(query);
        if (result.IsFailed) return HandleFailure(result);
        return Ok(result.Value);
    }
}
