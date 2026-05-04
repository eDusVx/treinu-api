using FluentResults;
using Treinu.Contracts.Commands.Chat;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Entities.Chat;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Chat;

public class RemoverMembroSalaChatHandler(IChatRepository chatRepository) 
    : IRequestHandler<RemoverMembroSalaChatCommand, Result<object>>
{
    public async Task<Result<object>> Handle(RemoverMembroSalaChatCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var salaResult = await chatRepository.BuscarSalaPorIdAsync(request.SalaId, cancellationToken);
            if (salaResult.IsFailed) return Result.Fail<object>("Sala não encontrada.");

            var sala = salaResult.Value;

            if (sala.Tipo != TipoSalaChat.Grupo)
                return Result.Fail<object>("Somente salas em grupo permitem remoção de membros.");

            // Pode remover se for o criador ou o próprio membro saindo
            if (sala.CriadorId != request.UsuarioLogadoId && request.MembroId != request.UsuarioLogadoId)
                return Result.Fail<object>("Sem permissão para remover este membro.");

            var removeResult = sala.RemoverParticipante(request.MembroId);
            if (removeResult.IsFailed) return Result.Fail<object>(removeResult.Errors);

            var saveResult = await chatRepository.AtualizarSalaAsync(sala);
            if (saveResult.IsFailed) return Result.Fail<object>(saveResult.Errors);

            return Result.Ok((object)new { Sucesso = true });
        }
        catch (Exception ex)
        {
            return Result.Fail<object>($"Erro inesperado ao remover membro: {ex.Message}");
        }
    }
}
