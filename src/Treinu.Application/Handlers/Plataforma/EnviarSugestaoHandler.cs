using FluentResults;
using Treinu.Domain.Core.Mediator;
using Treinu.Contracts.Commands.Plataforma;
using Treinu.Domain.Entities;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Plataforma;

public class EnviarSugestaoHandler(IPlataformaRepository plataformaRepository)
    : IRequestHandler<EnviarSugestaoCommand, Result>
{
    public async Task<Result> Handle(EnviarSugestaoCommand request, CancellationToken cancellationToken)
    {
        var sugestaoResult = Sugestao.Criar(request.UsuarioId, request.Titulo, request.Descricao);
        
        if (sugestaoResult.IsFailed)
            return Result.Fail(sugestaoResult.Errors);

        var addResult = await plataformaRepository.AdicionarSugestaoAsync(sugestaoResult.Value);
        
        if (addResult.IsFailed)
            return Result.Fail(addResult.Errors);

        return Result.Ok();
    }
}
