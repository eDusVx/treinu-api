using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Treinu.Contracts.Queries.Alunos;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Entities;
using Treinu.Domain.Enums;
using Treinu.Domain.Repositories;
using Treinu.Domain.Dtos;

namespace Treinu.Application.Handlers.Alunos;

public class ObterEvolucaoFisicaHandler(IUsuarioRepository usuarioRepository, IMetaRepository metaRepository)
    : IRequestHandler<ObterEvolucaoFisicaQuery, Result<object>>
{
    public async Task<Result<object>> Handle(ObterEvolucaoFisicaQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // 1. Fetch student with assessments
            var alunoResult = await usuarioRepository.BuscarAlunoPorIdAsync(request.AlunoId, cancellationToken);
            if (alunoResult.IsFailed) return Result.Fail<object>(alunoResult.Errors);

            var queryAvaliacoes = alunoResult.Value.AvaliacaoFisica.AsQueryable();

            if (request.DataInicio.HasValue)
            {
                var inicioUtc = DateTime.SpecifyKind(request.DataInicio.Value, DateTimeKind.Utc);
                queryAvaliacoes = queryAvaliacoes.Where(a => a.Data >= inicioUtc);
            }

            if (request.DataFim.HasValue)
            {
                var fimUtc = DateTime.SpecifyKind(request.DataFim.Value, DateTimeKind.Utc);
                queryAvaliacoes = queryAvaliacoes.Where(a => a.Data <= fimUtc);
            }

            var avaliacoes = queryAvaliacoes
                .OrderBy(a => a.Data)
                .ToList();

            // 2. Fetch goals for the student
            var metas = await metaRepository.BuscarMetasPorAlunoAsync(request.AlunoId, cancellationToken);
            var metasAtivas = metas.Where(m => m.Ativa).ToList();

            var metricsResult = new Dictionary<string, EvolucaoMedidaDto>();

            // Helper to compute evolution, trend, and meta crossing
            void ProcessMetric(string key, List<(DateTime Date, double Value)> history, Meta? meta)
            {
                if (history == null || !history.Any())
                {
                    metricsResult[key] = new EvolucaoMedidaDto(
                        new List<HistoricoValorDto>(),
                        0.0,
                        0.0,
                        0.0,
                        "ESTAVEL",
                        meta != null ? new MetaEvolucaoDto(meta.Id, meta.ValorAlvo, meta.DataLimite, "PENDENTE", 0.0) : null
                    );
                    return;
                }

                var primeiro = history.First();
                var ultimo = history.Last();

                var deltaAbs = Math.Round(ultimo.Value - primeiro.Value, 2);
                var deltaPct = primeiro.Value != 0 ? Math.Round((deltaAbs / primeiro.Value) * 100, 2) : 0.0;

                // Simple Linear Regression slope calculation
                string tendencia = "ESTAVEL";
                if (history.Count >= 2)
                {
                    double sumX = 0;
                    double sumY = 0;
                    double sumXY = 0;
                    double sumX2 = 0;
                    int n = history.Count;

                    var baseDate = primeiro.Date;
                    foreach (var pt in history)
                    {
                        double x = (pt.Date - baseDate).TotalDays;
                        double y = pt.Value;

                        sumX += x;
                        sumY += y;
                        sumXY += x * y;
                        sumX2 += x * x;
                    }

                    double denominator = n * sumX2 - sumX * sumX;
                    if (Math.Abs(denominator) > 0.0001)
                    {
                        double slope = (n * sumXY - sumX * sumY) / denominator;
                        if (slope > 0.001) tendencia = "SUBINDO";
                        else if (slope < -0.001) tendencia = "DESCENDO";
                    }
                    else
                    {
                        // Fallback to basic slope if all dates are same
                        var diff = ultimo.Value - primeiro.Value;
                        if (diff > 0.01) tendencia = "SUBINDO";
                        else if (diff < -0.01) tendencia = "DESCENDO";
                    }
                }

                MetaEvolucaoDto? metaInfo = null;
                if (meta != null)
                {
                    var valorAlvo = (double)meta.ValorAlvo;
                    string status = "PENDENTE";
                    double progresso = 0.0;

                    bool isReduction = valorAlvo < primeiro.Value;
                    if (isReduction)
                    {
                        if (ultimo.Value <= valorAlvo)
                        {
                            status = "CONCLUIDA";
                            progresso = 100.0;
                        }
                        else
                        {
                            double totalDist = primeiro.Value - valorAlvo;
                            double currentDist = primeiro.Value - ultimo.Value;
                            progresso = totalDist > 0 ? Math.Clamp(Math.Round((currentDist / totalDist) * 100, 2), 0.0, 100.0) : 0.0;
                        }
                    }
                    else // Gain/Hipertrofia
                    {
                        if (ultimo.Value >= valorAlvo)
                        {
                            status = "CONCLUIDA";
                            progresso = 100.0;
                        }
                        else
                        {
                            double totalDist = valorAlvo - primeiro.Value;
                            double currentDist = ultimo.Value - primeiro.Value;
                            progresso = totalDist > 0 ? Math.Clamp(Math.Round((currentDist / totalDist) * 100, 2), 0.0, 100.0) : 0.0;
                        }
                    }

                    metaInfo = new MetaEvolucaoDto(
                        meta.Id,
                        meta.ValorAlvo,
                        meta.DataLimite,
                        status,
                        progresso
                    );
                }

                metricsResult[key] = new EvolucaoMedidaDto(
                    history.Select(h => new HistoricoValorDto(h.Date, h.Value)).ToList(),
                    ultimo.Value,
                    deltaAbs,
                    deltaPct,
                    tendencia,
                    metaInfo
                );
            }

            // A. Process Weight (Peso)
            var pesoHistory = avaliacoes
                .Where(a => a.Peso > 0)
                .Select(a => (a.Data, a.Peso))
                .ToList();
            var pesoMeta = metasAtivas.FirstOrDefault(m => m.Tipo == TipoMetaEnum.PESO);
            ProcessMetric("peso", pesoHistory, pesoMeta);

            // B. Process Fat (Gordura)
            var gorduraHistory = avaliacoes
                .Where(a => a.PercentualGordura.HasValue && a.PercentualGordura.Value > 0)
                .Select(a => (a.Data, a.PercentualGordura!.Value))
                .ToList();
            var gorduraMeta = metasAtivas.FirstOrDefault(m => m.Tipo == TipoMetaEnum.GORDURA);
            ProcessMetric("gordura", gorduraHistory, gorduraMeta);

            // C. Process Medidas (Waist, Hip, etc.)
            foreach (ChaveMedidaEnum chave in Enum.GetValues(typeof(ChaveMedidaEnum)))
            {
                var keyStr = chave.ToString().ToLower();
                var medidaHistory = avaliacoes
                    .Select(a => new { a.Data, Medida = a.Medidas.FirstOrDefault(m => m.Chave == chave) })
                    .Where(x => x.Medida != null && x.Medida.Valor > 0)
                    .Select(x => (x.Data, (double)x.Medida!.Valor))
                    .ToList();

                // Find active meta corresponding to this measure
                var tipoMetaChave = Enum.Parse<TipoMetaEnum>(chave.ToString());
                var medidaMeta = metasAtivas.FirstOrDefault(m => m.Tipo == tipoMetaChave);

                ProcessMetric(keyStr, medidaHistory, medidaMeta);
            }

            return Result.Ok((object)metricsResult);
        }
        catch (Exception ex)
        {
            return Result.Fail<object>($"Erro ao obter evolução física: {ex.Message}");
        }
    }
}
