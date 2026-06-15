using FluentResults;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Treinu.Contracts.Queries.Admin;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Enums;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Admin;

public class ObterMetricasPlataformaHandler(ITelemetriaRepository telemetriaRepository)
    : IRequestHandler<ObterMetricasPlataformaQuery, Result<object>>
{
    public async Task<Result<object>> Handle(ObterMetricasPlataformaQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // 1. Total users by profile (aluno, treinador) - cumulative total
            var totalAlunosResult = await telemetriaRepository.ObterTotalUsuariosAtivosAsync(PerfilEnum.ALUNO, cancellationToken);
            var totalTreinadoresResult = await telemetriaRepository.ObterTotalUsuariosAtivosAsync(PerfilEnum.TREINADOR, cancellationToken);

            if (totalAlunosResult.IsFailed || totalTreinadoresResult.IsFailed)
            {
                return Result.Fail<object>("Erro ao buscar total de usuários ativos segmentados.");
            }

            // 2. Volume of new registrations, workouts created and workouts completed in range
            var novosCadastrosResult = await telemetriaRepository.ObterTotalNovosCadastrosAsync(request.DataInicio, request.DataFim, cancellationToken);
            var treinosCriadosResult = await telemetriaRepository.ObterTotalTreinosCriadosAsync(request.DataInicio, request.DataFim, cancellationToken);
            var treinosConcluidosResult = await telemetriaRepository.ObterTotalTreinosConcluidosAsync(request.DataInicio, request.DataFim, cancellationToken);

            if (novosCadastrosResult.IsFailed || treinosCriadosResult.IsFailed || treinosConcluidosResult.IsFailed)
            {
                return Result.Fail<object>("Erro ao buscar volumes operacionais no período.");
            }

            // 3. General engagement details (logins, chat, submits) in range
            var eventosResult = await telemetriaRepository.ObterEventosInteracaoAsync(request.DataInicio, request.DataFim, cancellationToken);
            if (eventosResult.IsFailed)
            {
                return Result.Fail<object>("Erro ao buscar eventos de telemetria.");
            }

            var eventos = eventosResult.Value;
            var totalLogins = eventos.Count(e => e.Tipo == TipoInteracaoEnum.LOGIN);
            var totalChat = eventos.Count(e => e.Tipo == TipoInteracaoEnum.MENSAGEM_CHAT);
            var totalSubmits = eventos.Count(e => e.Tipo != TipoInteracaoEnum.LOGIN && e.Tipo != TipoInteracaoEnum.MENSAGEM_CHAT);

            // General engagement score calculation
            // Logins: 1 pt, Chat messages: 2 pts, submissions: 5 pts
            var scoreGeral = (totalLogins * 1) + (totalChat * 2) + (totalSubmits * 5);

            var metricas = new
            {
                UsuariosAtivos = new
                {
                    Alunos = totalAlunosResult.Value,
                    Treinadores = totalTreinadoresResult.Value,
                    Total = totalAlunosResult.Value + totalTreinadoresResult.Value
                },
                VolumesPeriodo = new
                {
                    NovosCadastros = novosCadastrosResult.Value,
                    TreinosCriados = treinosCriadosResult.Value,
                    TreinosConcluidos = treinosConcluidosResult.Value
                },
                EngajamentoGeral = new
                {
                    TotalLogins = totalLogins,
                    TotalMensagensChat = totalChat,
                    TotalSubmissoes = totalSubmits,
                    ScoreGeral = scoreGeral
                }
            };

            return Result.Ok((object)metricas);
        }
        catch (Exception ex)
        {
            return Result.Fail<object>($"Erro inesperado ao obter métricas da plataforma: {ex.Message}");
        }
    }
}
