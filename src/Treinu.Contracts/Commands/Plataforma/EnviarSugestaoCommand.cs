using Treinu.Domain.Core.Mediator;
using FluentResults;

namespace Treinu.Contracts.Commands.Plataforma;

public record EnviarSugestaoCommand(Guid UsuarioId, string Titulo, string Descricao) : IRequest<Result>;
