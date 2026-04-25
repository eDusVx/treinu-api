using FluentResults;
using Treinu.Contracts.Queries.Alunos;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Alunos;

public class ObterDashboardEvolucaoHandler(
    IUsuarioRepository usuarioRepository)
    : IRequestHandler<ObterDashboardEvolucaoQuery, Result<object>>
{
    public async Task<Result<object>> Handle(ObterDashboardEvolucaoQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var alunoResult = await usuarioRepository.BuscarAlunoPorIdAsync(request.AlunoId, cancellationToken);
            if (alunoResult.IsFailed) return Result.Fail<object>(alunoResult.Errors);

            var avaliacoes = alunoResult.Value.AvaliacaoFisica
                .OrderBy(a => a.Data)
                .ToList();

            // Mapeando para um formato de timeline simples (pode ser customizado no futuro)
            var dashboardData = avaliacoes.Select(a => new
            {
                a.Id,
                a.Data,
                Tipo = "QUESTIONARIO",
                // Se fôssemos abrir `a` para propriedades específicas, faríamos um pattern matching.
                // Como não sabemos, passamos o objeto inteiro.
                Detalhes = a
            }).ToList();

            return Result.Ok((object)dashboardData);
        }
        catch (Exception ex)
        {
            return Result.Fail<object>($"Erro inesperado ao gerar dashboard: {ex.Message}");
        }
    }
}
