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
    /// <summary>
    /// Cria um novo treino para um aluno.
    /// </summary>
    /// <remarks>
    /// Exemplo de payload:
    /// 
    ///     POST /api/treinos
    ///     {
    ///       "nome": "Treino A - Hipertrofia",
    ///       "descricao": "Foco em membros superiores",
    ///       "dataInicio": "2023-11-01T00:00:00Z",
    ///       "dataFim": "2023-12-01T00:00:00Z",
    ///       "treinadorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///       "alunoId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///       "itens": [
    ///         {
    ///           "exercicioId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///           "series": 4,
    ///           "repeticoes": "10-12",
    ///           "carga": "20kg",
    ///           "pausa": "60s",
    ///           "observacoes": "Movimento bem controlado",
    ///           "ordem": 1
    ///         }
    ///       ]
    ///     }
    /// </remarks>
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

    /// <summary>
    /// Lista os treinos de acordo com os filtros (Aluno, Treinador, Status).
    /// </summary>
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
