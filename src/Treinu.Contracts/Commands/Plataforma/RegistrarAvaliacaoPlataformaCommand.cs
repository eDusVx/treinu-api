using Treinu.Domain.Core.Mediator;
using FluentResults;

namespace Treinu.Contracts.Commands.Plataforma;

public record RegistrarAvaliacaoPlataformaCommand(Guid UsuarioId, int Nota, string? Comentario) : IRequest<Result>;
