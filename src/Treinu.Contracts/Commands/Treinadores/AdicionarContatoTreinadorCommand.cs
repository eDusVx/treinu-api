using FluentResults;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Enums;
using Treinu.Contracts.Commands.Treinadores;

namespace Treinu.Contracts.Commands.Treinadores;

public record AdicionarContatoTreinadorCommand(
    Guid TreinadorId,
    TipoContatoEnum Tipo,
    string Valor,
    string? Descricao = null,
    bool? Principal = null,
    PlataformaRedeSocialEnum? Plataforma = null,
    string? NomeExibicao = null
) : IRequest<Result<object>>;
