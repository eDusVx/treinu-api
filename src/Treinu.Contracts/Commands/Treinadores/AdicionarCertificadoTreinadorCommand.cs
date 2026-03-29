using FluentResults;
using Treinu.Domain.Core.Mediator;
using Treinu.Contracts.Commands.Treinadores;

namespace Treinu.Contracts.Commands.Treinadores;

public record AdicionarCertificadoTreinadorCommand(
    Guid TreinadorId,
    string Nome,
    string ArquivoPdf,
    DateTime DataUpload,
    bool Validado = false
) : IRequest<Result<object>>;
