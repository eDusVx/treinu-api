using FluentResults;
using Treinu.Contracts.Commands;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Entities;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Alunos;

public class AdicionarContatoAlunoHandler(
    IUsuarioRepository usuarioRepository) : IRequestHandler<AdicionarContatoAlunoCommand, Result<object>>
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

            var addResult = alunoResult.Value.AdicionarContato(contatoResult.Value);
            if (addResult.IsFailed) return Result.Fail<object>(addResult.Errors);

            var saveResult = await usuarioRepository.AtualizarUsuarioAsync(alunoResult.Value);
            if (saveResult.IsFailed) return Result.Fail<object>(saveResult.Errors);

            return Result.Ok(alunoResult.Value.ToDto());
        }
        catch (Exception ex)
        {
            return Result.Fail<object>($"Erro inesperado ao adicionar contato: {ex.Message}");
        }
    }
}