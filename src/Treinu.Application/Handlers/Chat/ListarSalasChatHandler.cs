using FluentResults;
using Treinu.Contracts.Queries.Chat;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Chat;

public class ListarSalasChatHandler(IChatRepository chatRepository) 
    : IRequestHandler<ListarSalasChatQuery, Result<object>>
{
    public async Task<Result<object>> Handle(ListarSalasChatQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var salasResult = await chatRepository.BuscarSalasDoUsuarioAsync(request.UsuarioId, cancellationToken);
            if (salasResult.IsFailed) return Result.Fail<object>(salasResult.Errors);

            var salasDto = salasResult.Value.Select(s => new SalaChatDto(
                s.Id,
                s.Nome,
                s.Tipo.ToString(),
                s.CriadorId,
                s.Participantes.FirstOrDefault(p => p.UsuarioId == request.UsuarioId)?.MensagensNaoLidas ?? 0,
                s.Participantes.Count
            )).ToList();

            return Result.Ok((object)salasDto);
        }
        catch (Exception ex)
        {
            return Result.Fail<object>($"Erro inesperado ao listar salas: {ex.Message}");
        }
    }
}
