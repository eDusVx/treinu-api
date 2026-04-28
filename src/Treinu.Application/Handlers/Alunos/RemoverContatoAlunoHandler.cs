using FluentResults;
using Treinu.Contracts.Commands.Alunos;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Alunos;

public class RemoverContatoAlunoHandler(
    IUsuarioRepository usuarioRepository,
    IContatoRepository contatoRepository) : IRequestHandler<RemoverContatoAlunoCommand, Result<object>>
{
    public async Task<Result<object>> Handle(RemoverContatoAlunoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var alunoResult = await usuarioRepository.BuscarAlunoPorIdAsync(request.AlunoId, cancellationToken);
            if (alunoResult.IsFailed) return Result.Fail<object>(alunoResult.Errors);

            var removeResult = await contatoRepository.RemoverContatoAsync(request.ContatoId, request.AlunoId, cancellationToken);
            if (removeResult.IsFailed) return Result.Fail<object>(removeResult.Errors);

            return Result.Ok<object>("Contato removido com sucesso.");
        }
        catch (Exception ex)
        {
            return Result.Fail<object>($"Erro inesperado ao remover contato: {ex.Message}");
        }
    }
}