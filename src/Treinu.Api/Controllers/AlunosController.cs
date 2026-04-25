using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Treinu.Api.Controllers.Base;
using Treinu.Contracts.Commands.Alunos;
using Treinu.Contracts.Commands.Usuarios;
using Treinu.Contracts.Queries.Alunos;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Enums;
using System.Security.Claims;

namespace Treinu.Api.Controllers;

[ApiController]
[Route("api/aluno")]
public class AlunosController(IMediator mediator) : ApiController
{
    /// <summary>
    /// Registra um novo aluno a partir de um convite.
    /// </summary>
    /// <remarks>
    /// Exemplo de payload:
    /// 
    ///     POST /api/aluno/register
    ///     {
    ///       "nomeCompleto": "João Aluno",
    ///       "email": "joao@email.com",
    ///       "senha": "SenhaForte123!",
    ///       "dataNascimento": "2000-01-01T00:00:00Z",
    ///       "genero": "Masculino",
    ///       "cpf": "12345678909",
    ///       "aceiteTermoAdesao": true,
    ///       "objetivo": "Emagrecimento",
    ///       "tokenConvite": "123e4567-e89b-12d3-a456-426614174000"
    ///     }
    /// </remarks>
    [AllowAnonymous]
    [HttpPost("register")]
    [ProducesResponseType(typeof(object), 201)]
    [ProducesResponseType(typeof(ProblemDetails), 400)]
    public async Task<IActionResult> RegistrarAluno([FromBody] RegistrarAlunoCommand command)
    {
        var result = await mediator.Send(command);
        if (result.IsFailed) return HandleFailure(result);
        return CreatedAtAction(nameof(RegistrarAluno), result.Value);
    }

    /// <summary>
    /// Adiciona um contato ao perfil do aluno.
    /// </summary>
    [Authorize(Roles = $"{RoleConstants.Aluno},{RoleConstants.Admin}")]
    [HttpPost("{id:guid}/contatos")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 400)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    public async Task<IActionResult> AdicionarContato(Guid id, [FromBody] AdicionarContatoAlunoCommand command)
    {
        var cmd = command with { AlunoId = id };
        var result = await mediator.Send(cmd);
        if (result.IsFailed) return HandleFailure(result);
        return Ok(result.Value);
    }

    /// <summary>
    /// Remove um contato existente do perfil do aluno.
    /// </summary>
    [Authorize(Roles = $"{RoleConstants.Aluno},{RoleConstants.Admin}")]
    [HttpDelete("{id:guid}/contatos/{contatoId:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    public async Task<IActionResult> RemoverContato(Guid id, Guid contatoId)
    {
        var command = new RemoverContatoAlunoCommand(id, contatoId);
        var result = await mediator.Send(command);
        if (result.IsFailed) return HandleFailure(result);
        return NoContent();
    }

    /// <summary>
    /// Registra uma nova avaliação física para o aluno (Treinador/Admin).
    /// </summary>
    /// <remarks>
    /// Observação: Use "medidas" com as chaves corretas.
    /// Valores possíveis: PESCOCO, OMBROS, PEITO, CINTURA, QUADRIL, BRACO_ESQUERDO, BRACO_DIREITO, PERNA_ESQUERDA, PERNA_DIREITA, PANTURRILHA_ESQUERDA, PANTURRILHA_DIREITA.
    /// 
    /// Exemplo de payload:
    /// 
    ///     POST /api/aluno/{id}/avaliacoes
    ///     {
    ///       "altura": 1.78,
    ///       "peso": 83.5,
    ///       "medidas": [
    ///         { "chave": "PESCOCO", "valor": 38.5 },
    ///         { "chave": "OMBROS", "valor": 110.0 },
    ///         { "chave": "PEITO", "valor": 102.0 },
    ///         { "chave": "CINTURA", "valor": 84.0 },
    ///         { "chave": "QUADRIL", "valor": 95.5 },
    ///         { "chave": "BRACO_ESQUERDO", "valor": 36.0 },
    ///         { "chave": "BRACO_DIREITO", "valor": 36.5 },
    ///         { "chave": "PERNA_ESQUERDA", "valor": 59.0 },
    ///         { "chave": "PERNA_DIREITA", "valor": 59.5 },
    ///         { "chave": "PANTURRILHA_ESQUERDA", "valor": 38.0 },
    ///         { "chave": "PANTURRILHA_DIREITA", "valor": 38.5 }
    ///       ]
    ///     }
    /// </remarks>
    [Authorize(Roles = $"{RoleConstants.Treinador},{RoleConstants.Admin}")]
    [HttpPost("{id:guid}/avaliacoes")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 400)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    public async Task<IActionResult> AdicionarAvaliacaoFisica(Guid id,
        [FromBody] Treinu.Domain.Dtos.AvaliacaoFisicaDto dto)
    {
        var cmd = new AdicionarAvaliacaoFisicaAlunoCommand(id, dto);
        var result = await mediator.Send(cmd);
        if (result.IsFailed) return HandleFailure(result);
        return Ok(result.Value);
    }

    /// <summary>
    /// Remove uma avaliação física de um aluno.
    /// </summary>
    [Authorize(Roles = $"{RoleConstants.Treinador},{RoleConstants.Admin}")]
    [HttpDelete("{id:guid}/avaliacoes/{avaliacaoId:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    public async Task<IActionResult> RemoverAvaliacaoFisica(Guid id, Guid avaliacaoId)
    {
        var command = new RemoverAvaliacaoFisicaAlunoCommand(id, avaliacaoId);
        var result = await mediator.Send(command);
        if (result.IsFailed) return HandleFailure(result);
        return NoContent();
    }

    /// <summary>
    /// Registra uma nova avaliação física para o próprio aluno logado.
    /// </summary>
    /// <remarks>
    /// Observação: Use "medidas" com as chaves corretas.
    /// Valores possíveis: PESCOCO, OMBROS, PEITO, CINTURA, QUADRIL, BRACO_ESQUERDO, BRACO_DIREITO, PERNA_ESQUERDA, PERNA_DIREITA, PANTURRILHA_ESQUERDA, PANTURRILHA_DIREITA.
    /// 
    /// Exemplo de payload:
    /// 
    ///     POST /api/aluno/me/avaliacoes
    ///     {
    ///       "altura": 1.78,
    ///       "peso": 83.5,
    ///       "medidas": [
    ///         { "chave": "CINTURA", "valor": 84.0 },
    ///         { "chave": "PEITO", "valor": 102.0 }
    ///       ]
    ///     }
    /// </remarks>
    [Authorize(Roles = RoleConstants.Aluno)]
    [HttpPost("me/avaliacoes")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 400)]
    public async Task<IActionResult> AdicionarMinhaAvaliacaoFisica([FromBody] Treinu.Domain.Dtos.AvaliacaoFisicaDto dto)
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var alunoId))
            return Unauthorized();

        var cmd = new AdicionarAvaliacaoFisicaProprioAlunoCommand(alunoId, dto);
        var result = await mediator.Send(cmd);
        if (result.IsFailed) return HandleFailure(result);
        return Ok(result.Value);
    }

    /// <summary>
    /// Retorna os dados do dashboard de evolução do próprio aluno.
    /// </summary>
    [Authorize(Roles = RoleConstants.Aluno)]
    [HttpGet("me/dashboard")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 400)]
    public async Task<IActionResult> ObterMeuDashboardEvolucao()
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var alunoId))
            return Unauthorized();

        var query = new ObterDashboardEvolucaoQuery(alunoId);
        var result = await mediator.Send(query);
        if (result.IsFailed) return HandleFailure(result);
        return Ok(result.Value);
    }

    /// <summary>
    /// Retorna os dados do dashboard de evolução de um aluno específico (Treinador/Admin).
    /// </summary>
    [Authorize(Roles = $"{RoleConstants.Treinador},{RoleConstants.Admin}")]
    [HttpGet("{id:guid}/dashboard")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 400)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    public async Task<IActionResult> ObterDashboardEvolucao(Guid id)
    {
        var query = new ObterDashboardEvolucaoQuery(id);
        var result = await mediator.Send(query);
        if (result.IsFailed) return HandleFailure(result);
        return Ok(result.Value);
    }
}