using FluentResults;
using Treinu.Domain.Core.Mediator;
using Treinu.Contracts.Commands.Treinadores;

namespace Treinu.Contracts.Commands.Treinadores;

public record RemoverCertificadoTreinadorCommand(
    Guid TreinadorId,
    Guid CertificadoId
) : IRequest<Result<object>>;
