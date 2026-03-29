using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Treinu.Contracts.Commands.Autenticacao;
using Treinu.Contracts.Queries.Autenticacao;
using Treinu.Contracts.Responses;
using Treinu.Domain.Core.Mediator;

namespace Treinu.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(TokenDto), 200)]
    public async Task<IActionResult> Login([FromBody] AutenticarUsuarioLocalQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result.Value);
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(TokenDto), 200)]
    public async Task<IActionResult> Refresh([FromBody] RenovarTokenQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result.Value);
    }

    [AllowAnonymous]
    [HttpPost("recuperar-senha")]
    [ProducesResponseType(typeof(object), 200)]
    public async Task<IActionResult> SolicitarRecuperacaoSenha([FromBody] SolicitarRecuperacaoSenhaCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result.Value);
    }

    [AllowAnonymous]
    [HttpPost("redefinir-senha")]
    [ProducesResponseType(typeof(object), 200)]
    public async Task<IActionResult> RedefinirSenha([FromBody] RedefinirSenhaCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result.Value);
    }

    [AllowAnonymous]
    [HttpPost("solicitar-codigo-login")]
    [ProducesResponseType(typeof(object), 200)]
    public async Task<IActionResult> SolicitarCodigoLogin([FromBody] SolicitarCodigoLoginCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result.Value);
    }

    [AllowAnonymous]
    [HttpPost("login-codigo")]
    [ProducesResponseType(typeof(TokenDto), 200)]
    public async Task<IActionResult> LoginCodigo([FromBody] LoginComCodigoQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result.Value);
    }
}