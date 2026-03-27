using FluentResults;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Dtos;
using Treinu.Domain.Enums;

namespace Treinu.Contracts.Commands;

public record AdicionarContatoTreinadorCommand(
    Guid TreinadorId,
    TipoContatoEnum Tipo,
    string Valor,
    string? Descricao = null,
    bool? Principal = null,
    PlataformaRedeSocialEnum? Plataforma = null,
    string? NomeExibicao = null
) : IRequest<Result<object>>;

public record RemoverContatoTreinadorCommand(
    Guid TreinadorId,
    Guid ContatoId
) : IRequest<Result<object>>;

public record AdicionarEspecializacaoTreinadorCommand(
    Guid TreinadorId,
    string Especializacao
) : IRequest<Result<object>>;

public record RemoverEspecializacaoTreinadorCommand(
    Guid TreinadorId,
    string Especializacao
) : IRequest<Result<object>>;

public record AdicionarCertificadoTreinadorCommand(
    Guid TreinadorId,
    string Nome,
    string ArquivoPdf,
    DateTime DataUpload,
    bool Validado = false
) : IRequest<Result<object>>;

public record RemoverCertificadoTreinadorCommand(
    Guid TreinadorId,
    Guid CertificadoId
) : IRequest<Result<object>>;
