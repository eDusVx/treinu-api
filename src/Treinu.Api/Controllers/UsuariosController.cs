using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Treinu.Contracts.Commands;
using Treinu.Contracts.Queries;
using Treinu.Contracts.Responses;
using Treinu.Domain.Enums;

namespace Treinu.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsuariosController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsuariosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [AllowAnonymous] // allowing anonymous registration
    [HttpPost]
    [ProducesResponseType(typeof(object), 201)] // Returns the created user
    public async Task<IActionResult> RegistrarUsuario([FromBody] RegistrarUsuarioCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(BuscarUsuarios), new { page = 1 }, result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PaginationResponse<object>), 200)]
    public async Task<IActionResult> BuscarUsuarios([FromQuery] PerfilEnum? tipoUsuario, [FromQuery] int page = 1, [FromQuery] int limit = 10)
    {
        var query = new BuscarUsuariosQuery(tipoUsuario, page, limit);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
