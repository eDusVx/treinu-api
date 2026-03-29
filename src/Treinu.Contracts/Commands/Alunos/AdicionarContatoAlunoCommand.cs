using FluentResults;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Enums;
using Treinu.Contracts.Commands.Alunos;

namespace Treinu.Contracts.Commands.Alunos;

public record AdicionarContatoAlunoCommand(
    Guid AlunoId,
    TipoContatoEnum Tipo,
    string Valor,
    string? Descricao = null,
    bool? Principal = null,
    PlataformaRedeSocialEnum? Plataforma = null,
    string? NomeExibicao = null
) : IRequest<Result<object>>;
