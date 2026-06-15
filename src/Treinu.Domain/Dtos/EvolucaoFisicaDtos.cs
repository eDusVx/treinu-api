using System;
using System.Collections.Generic;

namespace Treinu.Domain.Dtos;

public record HistoricoValorDto(
    DateTime Date,
    double Value
);

public record MetaEvolucaoDto(
    Guid Id,
    decimal ValorAlvo,
    DateTime DataLimite,
    string Status,
    double Progresso
);

public record EvolucaoMedidaDto(
    List<HistoricoValorDto> Historico,
    double UltimoValor,
    double DeltaAbsoluto,
    double DeltaPercentual,
    string Tendencia,
    MetaEvolucaoDto? Meta
);
