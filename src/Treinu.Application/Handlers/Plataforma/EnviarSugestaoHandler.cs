using FluentResults;
using Treinu.Domain.Core.Mediator;
using Treinu.Contracts.Commands.Plataforma;
using Treinu.Domain.Entities;
using Treinu.Domain.Repositories;
using Treinu.Domain.Enums;

namespace Treinu.Application.Handlers.Plataforma;

public class EnviarSugestaoHandler(IPlataformaRepository plataformaRepository, ITelemetriaRepository telemetriaRepository)
    : IRequestHandler<EnviarSugestaoCommand, Result>
{
    public async Task<Result> Handle(EnviarSugestaoCommand request, CancellationToken cancellationToken)
    {
        var sugestaoResult = Sugestao.Criar(request.UsuarioId, request.Titulo, request.Descricao);
        
        if (sugestaoResult.IsFailed)
            return Result.Fail(sugestaoResult.Errors);

        var addResult = await plataformaRepository.AdicionarSugestaoAsync(sugestaoResult.Value);
        
        if (addResult.IsFailed)
            return Result.Fail(addResult.Errors);

        // Log telemetry event
        var sugestEvent = EventoTelemetria.Criar(request.UsuarioId, TipoInteracaoEnum.SUBMIT_SUGESTAO);
        await telemetriaRepository.RegistrarEventoAsync(sugestEvent, cancellationToken);

        return Result.Ok();
    }
}
