using FluentResults;
using System;
using System.Threading;
using System.Threading.Tasks;
using Treinu.Contracts.Commands.Metas;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Entities;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Metas;

public class CadastrarMetaHandler(IMetaRepository metaRepository, IUsuarioRepository usuarioRepository)
    : IRequestHandler<CadastrarMetaCommand, Result<object>>
{
    public async Task<Result<object>> Handle(CadastrarMetaCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var alunoResult = await usuarioRepository.BuscarAlunoPorIdAsync(request.AlunoId, cancellationToken);
            if (alunoResult.IsFailed) return Result.Fail<object>(alunoResult.Errors);

            // Deactivate existing active goals of the same type for this student
            var existingGoals = await metaRepository.BuscarMetasPorAlunoAsync(request.AlunoId, cancellationToken);
            var activeGoalOfType = existingGoals.FirstOrDefault(g => g.Tipo == request.Tipo && g.Ativa);
            if (activeGoalOfType != null)
            {
                activeGoalOfType.Desativar();
                await metaRepository.AtualizarMetaAsync(activeGoalOfType, cancellationToken);
            }

            var metaResult = Meta.Criar(request.AlunoId, request.Tipo, request.ValorAlvo, request.DataLimite);
            if (metaResult.IsFailed) return Result.Fail<object>(metaResult.Errors);

            var saveResult = await metaRepository.SalvarMetaAsync(metaResult.Value, cancellationToken);
            if (saveResult.IsFailed) return Result.Fail<object>(saveResult.Errors);

            return Result.Ok((object)metaResult.Value);
        }
        catch (Exception ex)
        {
            return Result.Fail<object>($"Erro inesperado ao cadastrar meta: {ex.Message}");
        }
    }
}
