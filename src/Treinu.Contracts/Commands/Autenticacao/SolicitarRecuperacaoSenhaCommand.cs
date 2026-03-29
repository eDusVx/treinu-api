using FluentResults;
using Treinu.Domain.Core.Mediator;
using Treinu.Contracts.Commands.Autenticacao;

namespace Treinu.Contracts.Commands.Autenticacao;

public record SolicitarRecuperacaoSenhaCommand(string Email) : IRequest<Result<object>>;
