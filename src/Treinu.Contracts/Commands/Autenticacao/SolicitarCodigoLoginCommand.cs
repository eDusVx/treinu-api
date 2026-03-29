using FluentResults;
using Treinu.Domain.Core.Mediator;
using Treinu.Contracts.Commands.Autenticacao;

namespace Treinu.Contracts.Commands.Autenticacao;

public record SolicitarCodigoLoginCommand(string Email) : IRequest<Result<object>>;
