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
    /// <summary>
    /// Cadastra um novo exercício no banco de dados do treinador.
    /// </summary>
    /// <remarks>
    /// Exemplo de payload:
    /// 
    ///     POST /api/exercicios
    ///     {
    ///       "nome": "Supino Reto",
    ///       "descricao": "Exercício de peito em banco plano",
    ///       "tags": "Peito, Força, Barra",
    ///       "arquivoDemonstracao": "https://link-para-video.com/video.mp4",
    ///       "treinadorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    ///     }
    /// </remarks>
    [HttpPost]
    [ProducesResponseType(typeof(object), 201)]
    [ProducesResponseType(typeof(ProblemDetails), 400)]
    public async Task<IActionResult> RegistrarExercicio([FromBody] RegistrarExercicioCommand command)
    {
        var result = await mediator.Send(command);
        if (result.IsFailed) return HandleFailure(result);
        return CreatedAtAction(nameof(BuscarExercicios), new { tags = string.Empty }, result.Value);
    }

    /// <summary>
    /// Busca ou lista os exercícios de um treinador com opções de filtros por tags.
    /// </summary>
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
