using FluentResults;
using Treinu.Contracts.Queries.Usuarios;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Usuarios;

public class BuscarNotificacoesHandler(
    INotificacaoRepository notificacaoRepository)
    : IRequestHandler<BuscarNotificacoesQuery, Result<object>>
{
    public async Task<Result<object>> Handle(BuscarNotificacoesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await notificacaoRepository.BuscarNotificacoesPorUsuarioAsync(request.UsuarioId, cancellationToken);
            if (result.IsFailed) return Result.Fail<object>(result.Errors);

            var dtos = result.Value.Select(n => n.ToDto()).ToList();
            return Result.Ok((object)dtos);
        }
        catch (Exception ex)
        {
            return Result.Fail<object>($"Erro inesperado ao buscar notificações: {ex.Message}");
        }
    }
}
