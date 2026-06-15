using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Treinu.Api.Controllers.Base;
using Treinu.Contracts.Commands.Treinadores;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Enums;

namespace Treinu.Api.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = RoleConstants.Admin)]
public class AdminController(IMediator mediator) : ApiController
{
    /// <summary>
    /// Aprova um treinador recém-cadastrado na plataforma, permitindo o seu login.
    /// </summary>
    [HttpPatch("treinador/{id:guid}/approve")]
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

    /// <summary>
    /// Retorna as métricas e relatórios gerais da plataforma (cadastros e status operacional).
    /// </summary>
    [HttpGet("metricas-plataforma")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 400)]
    public async Task<IActionResult> ObterMetricasPlataforma(
        [FromQuery] DateTime dataInicio,
        [FromQuery] DateTime dataFim)
    {
        var query = new Treinu.Contracts.Queries.Admin.ObterMetricasPlataformaQuery(dataInicio, dataFim);
        var result = await mediator.Send(query);
        if (result.IsFailed) return HandleFailure(result);
        return Ok(result.Value);
    }
}