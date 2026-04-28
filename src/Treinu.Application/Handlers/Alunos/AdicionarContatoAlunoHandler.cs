using FluentResults;
using Treinu.Contracts.Commands.Alunos;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Entities;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Alunos;

public class AdicionarContatoAlunoHandler(
    IUsuarioRepository usuarioRepository,
    IContatoRepository contatoRepository) : IRequestHandler<AdicionarContatoAlunoCommand, Result<object>>
{
    public async Task<Result<object>> Handle(AdicionarContatoAlunoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var alunoResult = await usuarioRepository.BuscarAlunoPorIdAsync(request.AlunoId, cancellationToken);
            if (alunoResult.IsFailed) return Result.Fail<object>(alunoResult.Errors);

            var contatoResult = Contato.Criar(new CriarContatoProps(
                request.Tipo,
                request.Valor,
                request.Descricao,
                request.Principal,
                request.Plataforma,
                request.NomeExibicao
            ));
            if (contatoResult.IsFailed) return Result.Fail<object>(contatoResult.Errors);

            var saveResult = await contatoRepository.AdicionarContatoAsync(contatoResult.Value, request.AlunoId, cancellationToken);
            if (saveResult.IsFailed) return Result.Fail<object>(saveResult.Errors);

            // Recarrega o aluno com os contatos atualizados para retornar o DTO completo
            var alunoAtualizado = await usuarioRepository.BuscarAlunoPorIdAsync(request.AlunoId, cancellationToken);
            if (alunoAtualizado.IsFailed) return Result.Fail<object>(alunoAtualizado.Errors);

            return Result.Ok(alunoAtualizado.Value.ToDto());
        }
        catch (Exception ex)
        {
            return Result.Fail<object>($"Erro inesperado ao adicionar contato: {ex.Message}");
        }
    }
}