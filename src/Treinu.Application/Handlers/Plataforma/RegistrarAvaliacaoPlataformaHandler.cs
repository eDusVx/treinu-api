using FluentResults;
using Treinu.Domain.Core.Mediator;
using Treinu.Contracts.Commands.Plataforma;
using Treinu.Domain.Entities;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Plataforma;

public class RegistrarAvaliacaoPlataformaHandler(IPlataformaRepository plataformaRepository)
    : IRequestHandler<RegistrarAvaliacaoPlataformaCommand, Result>
{
    public async Task<Result> Handle(RegistrarAvaliacaoPlataformaCommand request, CancellationToken cancellationToken)
    {
        var avaliacaoResult = AvaliacaoPlataforma.Criar(request.UsuarioId, request.Nota, request.Comentario);
        
        if (avaliacaoResult.IsFailed)
            return Result.Fail(avaliacaoResult.Errors);

        var addResult = await plataformaRepository.AdicionarAvaliacaoPlataformaAsync(avaliacaoResult.Value);
        
        if (addResult.IsFailed)
            return Result.Fail(addResult.Errors);

        return Result.Ok();
    }
}
