using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Treinu.Api.Controllers.Base;
using Treinu.Contracts.Queries;
using Treinu.Contracts.Responses;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Enums;

namespace Treinu.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController(IMediator mediator) : ApiController
{
    [Authorize(Roles = "ADMIN,TREINADOR")]
    [HttpGet]
    [ProducesResponseType(typeof(PaginationResponse<object>), 200)]
    public async Task<IActionResult> BuscarUsuarios([FromQuery] PerfilEnum? tipoUsuario, [FromQuery] int page = 1,
        [FromQuery] int limit = 10)
    {
        var query = new BuscarUsuariosQuery(tipoUsuario, page, limit);
        var result = await mediator.Send(query);
        if (result.IsFailed) return HandleFailure(result);
        return Ok(result.Value);
    }
}