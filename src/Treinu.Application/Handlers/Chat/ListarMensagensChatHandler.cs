using FluentResults;
using Treinu.Contracts.Queries.Chat;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Chat;

public class ListarMensagensChatHandler(IChatRepository chatRepository) 
    : IRequestHandler<ListarMensagensChatQuery, Result<object>>
{
    public async Task<Result<object>> Handle(ListarMensagensChatQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var salaResult = await chatRepository.BuscarSalaPorIdAsync(request.SalaId, cancellationToken);
            if (salaResult.IsFailed) return Result.Fail<object>("Sala não encontrada.");

            var sala = salaResult.Value;
            if (!sala.Participantes.Any(p => p.UsuarioId == request.UsuarioId))
                return Result.Fail<object>("Você não é participante desta sala.");

            var mensagensResult = await chatRepository.BuscarMensagensDaSalaAsync(request.SalaId, request.Page, request.Limit, cancellationToken);
            if (mensagensResult.IsFailed) return Result.Fail<object>(mensagensResult.Errors);

            var dto = mensagensResult.Value.Select(m => new 
            {
                Id = m.Id,
                SalaId = m.SalaChatId,
                RemetenteId = m.RemetenteId,
                NomeRemetente = m.Remetente?.NomeCompleto ?? "Usuário",
                Conteudo = m.Conteudo,
                DataEnvio = m.DataEnvio,
                Tipo = m.Tipo.ToString()
            }).ToList();

            return Result.Ok((object)dto);
        }
        catch (Exception ex)
        {
            return Result.Fail<object>($"Erro inesperado ao listar mensagens: {ex.Message}");
        }
    }
}
