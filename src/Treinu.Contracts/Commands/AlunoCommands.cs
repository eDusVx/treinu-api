using FluentResults;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Dtos;
using Treinu.Domain.Enums;

namespace Treinu.Contracts.Commands;

public record AdicionarContatoAlunoCommand(
    Guid AlunoId,
    TipoContatoEnum Tipo,
    string Valor,
    string? Descricao = null,
    bool? Principal = null,
    PlataformaRedeSocialEnum? Plataforma = null,
    string? NomeExibicao = null
) : IRequest<Result<object>>;

public record RemoverContatoAlunoCommand(
    Guid AlunoId,
    Guid ContatoId
) : IRequest<Result<object>>;

public record AdicionarAvaliacaoFisicaAlunoCommand(
    Guid AlunoId,
    AvaliacaoFisicaDto AvaliacaoFisica
) : IRequest<Result<object>>;

public record RemoverAvaliacaoFisicaAlunoCommand(
    Guid AlunoId,
    Guid AvaliacaoFisicaId
) : IRequest<Result<object>>;
