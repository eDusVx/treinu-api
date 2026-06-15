using FluentResults;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Treinu.Contracts.Queries.Alunos;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Entities;
using Treinu.Domain.Enums;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Alunos;

public class ObterComparativoPerformanceAlunosHandler(
    IUsuarioRepository usuarioRepository,
    IExecucaoTreinoRepository execucaoTreinoRepository)
    : IRequestHandler<ObterComparativoPerformanceAlunosQuery, Result<object>>
{
    public async Task<Result<object>> Handle(ObterComparativoPerformanceAlunosQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // 1. Get students (optionally filtered by TreinadorId)
            var usuariosResult = await usuarioRepository.BuscarUsuariosPaginadoAsync(
                PerfilEnum.ALUNO, 1, 99999, cancellationToken);
            if (usuariosResult.IsFailed)
                return Result.Fail<object>(usuariosResult.Errors);

            var alunos = usuariosResult.Value.Usuarios
                .OfType<Aluno>()
                .Where(a => !request.TreinadorId.HasValue || a.TreinadorId == request.TreinadorId.Value)
                .Select(a => new { a.Id, a.NomeCompleto })
                .ToList();

            var alunoIds = alunos.Select(a => a.Id).ToList();

            // 2. Get workout executions for these students in the period
            var execucoesResult = await execucaoTreinoRepository.BuscarExecucoesFiltradoAsync(
                request.TreinadorId, null, request.DataInicio, request.DataFim, cancellationToken);
            if (execucoesResult.IsFailed)
                return Result.Fail<object>(execucoesResult.Errors);

            // Filter executions strictly to our selected students just in case
            var execucoes = execucoesResult.Value
                .Where(e => alunoIds.Contains(e.AlunoId))
                .ToList();

            // 3. Generate statistics per student
            var comparativo = new List<object>();

            foreach (var aluno in alunos)
            {
                var execsAluno = execucoes.Where(e => e.AlunoId == aluno.Id).ToList();
                var total = execsAluno.Count;
                var concluidos = execsAluno.Count(e => e.Concluido);
                var taxa = total > 0 ? Math.Round((double)concluidos / total * 100, 2) : 0.0;

                object? historico = null;

                if (!string.IsNullOrEmpty(request.Agrupamento))
                {
                    if (request.Agrupamento.ToLower() == "mes")
                    {
                        historico = execsAluno
                            .GroupBy(e => new { e.DataInicio.Year, e.DataInicio.Month })
                            .Select(g => new
                            {
                                Periodo = $"{g.Key.Year}-{g.Key.Month:D2}",
                                Total = g.Count(),
                                Concluidos = g.Count(e => e.Concluido),
                                TaxaConclusao = Math.Round((double)g.Count(e => e.Concluido) / g.Count() * 100, 2)
                            })
                            .OrderBy(p => p.Periodo)
                            .ToList();
                    }
                    else if (request.Agrupamento.ToLower() == "semana")
                    {
                        historico = execsAluno
                            .GroupBy(e => {
                                int week = ISOWeek.GetWeekOfYear(e.DataInicio);
                                return new { e.DataInicio.Year, Week = week };
                            })
                            .Select(g => new
                            {
                                Periodo = $"{g.Key.Year}-W{g.Key.Week:D2}",
                                Total = g.Count(),
                                Concluidos = g.Count(e => e.Concluido),
                                TaxaConclusao = Math.Round((double)g.Count(e => e.Concluido) / g.Count() * 100, 2)
                            })
                            .OrderBy(p => p.Periodo)
                            .ToList();
                    }
                }

                comparativo.Add(new
                {
                    aluno.Id,
                    NomeAluno = aluno.NomeCompleto,
                    TotalTreinos = total,
                    TreinosConcluidos = concluidos,
                    TaxaConclusao = taxa,
                    Historico = historico
                });
            }

            return Result.Ok((object)comparativo);
        }
        catch (Exception ex)
        {
            return Result.Fail<object>($"Erro ao obter comparativo de performance: {ex.Message}");
        }
    }
}
