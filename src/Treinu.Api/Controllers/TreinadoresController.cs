using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Treinu.Api.Controllers.Base;
using Treinu.Contracts.Commands.Treinadores;
using Treinu.Contracts.Commands.Usuarios;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Enums;

namespace Treinu.Api.Controllers;

[ApiController]
[Route("api/treinador")]
public class TreinadoresController(IMediator mediator) : ApiController
{
    /// <summary>
    /// Registra um novo treinador.
    /// </summary>
    /// <remarks>
    /// Exemplo de payload:
    /// 
    ///     POST /api/treinador
    ///     {
    ///       "nomeCompleto": "Carlos Treinador",
    ///       "email": "carlos@esporte.com",
    ///       "senha": "SenhaForte123!",
    ///       "dataNascimento": "1990-05-20T00:00:00Z",
    ///       "genero": "Masculino",
    ///       "cpf": "09876543211",
    ///       "aceiteTermoAdesao": true,
    ///       "cref": "123456-G/SP"
    ///     }
    /// </remarks>
    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(typeof(object), 201)]
    [ProducesResponseType(typeof(ProblemDetails), 400)]
    public async Task<IActionResult> RegistrarTreinador([FromBody] RegistrarTreinadorCommand command)
    {
        var result = await mediator.Send(command);
        if (result.IsFailed) return HandleFailure(result);
        return CreatedAtAction(nameof(RegistrarTreinador), result.Value);
    }

    /// <summary>
    /// Adiciona um contato ao perfil do treinador.
    /// </summary>
    [Authorize(Roles = $"{RoleConstants.Treinador},{RoleConstants.Admin}")]
    [HttpPost("{id:guid}/contatos")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 400)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    public async Task<IActionResult> AdicionarContato(Guid id, [FromBody] AdicionarContatoTreinadorCommand command)
    {
        var cmd = command with { TreinadorId = id };
        var result = await mediator.Send(cmd);
        if (result.IsFailed) return HandleFailure(result);
        return Ok(result.Value);
    }

    /// <summary>
    /// Remove um contato existente do perfil do treinador.
    /// </summary>
    [Authorize(Roles = $"{RoleConstants.Treinador},{RoleConstants.Admin}")]
    [HttpDelete("{id:guid}/contatos/{contatoId:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    public async Task<IActionResult> RemoverContato(Guid id, Guid contatoId)
    {
        var command = new RemoverContatoTreinadorCommand(id, contatoId);
        var result = await mediator.Send(command);
        if (result.IsFailed) return HandleFailure(result);
        return NoContent();
    }

    /// <summary>
    /// Adiciona uma especialização ao perfil do treinador.
    /// </summary>
    /// <remarks>
    /// Especializações possíveis (Exemplos): Musculacao, Emagrecimento, Funcional, Crossfit, etc.
    /// </remarks>
    [Authorize(Roles = $"{RoleConstants.Treinador},{RoleConstants.Admin}")]
    [HttpPost("{id:guid}/especializacoes")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 400)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    public async Task<IActionResult> AdicionarEspecializacao(Guid id,
        [FromBody] AdicionarEspecializacaoTreinadorCommand command)
    {
        var cmd = command with { TreinadorId = id };
        var result = await mediator.Send(cmd);
        if (result.IsFailed) return HandleFailure(result);
        return Ok(result.Value);
    }

    /// <summary>
    /// Remove uma especialização do perfil do treinador.
    /// </summary>
    [Authorize(Roles = $"{RoleConstants.Treinador},{RoleConstants.Admin}")]
    [HttpDelete("{id:guid}/especializacoes")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    public async Task<IActionResult> RemoverEspecializacao(Guid id,
        [FromBody] RemoverEspecializacaoTreinadorCommand command)
    {
        var cmd = command with { TreinadorId = id };
        var result = await mediator.Send(cmd);
        if (result.IsFailed) return HandleFailure(result);
        return NoContent();
    }

}