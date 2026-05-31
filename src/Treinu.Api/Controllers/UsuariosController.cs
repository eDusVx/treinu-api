using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Treinu.Api.Controllers.Base;
using Treinu.Contracts.Queries.Usuarios;
using Treinu.Contracts.Responses;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Enums;

namespace Treinu.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController(IMediator mediator) : ApiController
{
    /// <summary>
    /// Busca lista de usuários com paginação e filtro por tipo de perfil (Admin/Treinador).
    /// </summary>
    [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.Treinador}")]
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

    [HttpPut("{id}/notification-settings")]
    [Authorize]
    public async Task<IActionResult> ConfigurarNotificacoes(Guid id, [FromBody] ConfigurarNotificacoesRequest request)
    {
        var usuarioIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(usuarioIdClaim, out var logadoId) || logadoId != id) return Forbid();

        var command = new Treinu.Contracts.Commands.Usuarios.ConfigurarNotificacoesCommand(
            id,
            request.ReceberEmail,
            request.ReceberPush,
            request.AlertaVencimentoAvaliacao,
            request.AlertaVencimentoTreino,
            request.AlertaNovoTreino
        );

        var result = await mediator.Send(command);

        if (result.IsFailed)
            return BadRequest(result.Errors.Select(e => e.Message));

        return Ok(new { Mensagem = "Configurações atualizadas com sucesso" });
    }
}

public record ConfigurarNotificacoesRequest(
    bool ReceberEmail,
    bool ReceberPush,
    bool AlertaVencimentoAvaliacao,
    bool AlertaVencimentoTreino,
    bool AlertaNovoTreino
);