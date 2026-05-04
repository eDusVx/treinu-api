using FluentResults;
using Treinu.Contracts.Commands.Chat;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Entities.Chat;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Chat;

public class AdicionarMembroSalaChatHandler(IChatRepository chatRepository) 
    : IRequestHandler<AdicionarMembroSalaChatCommand, Result<object>>
{
    public async Task<Result<object>> Handle(AdicionarMembroSalaChatCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var salaResult = await chatRepository.BuscarSalaPorIdAsync(request.SalaId, cancellationToken);
            if (salaResult.IsFailed) return Result.Fail<object>("Sala não encontrada.");

            var sala = salaResult.Value;

            if (sala.Tipo != TipoSalaChat.Grupo)
                return Result.Fail<object>("Somente salas em grupo permitem adição de membros.");

            if (sala.CriadorId != request.UsuarioLogadoId)
                return Result.Fail<object>("Somente o criador da sala pode adicionar membros.");

            var addResult = sala.AdicionarParticipante(request.NovoMembroId);
            if (addResult.IsFailed) return Result.Fail<object>(addResult.Errors);

            var saveResult = await chatRepository.AtualizarSalaAsync(sala);
            if (saveResult.IsFailed) return Result.Fail<object>(saveResult.Errors);

            return Result.Ok((object)new { Sucesso = true });
        }
        catch (Exception ex)
        {
            return Result.Fail<object>($"Erro inesperado ao adicionar membro: {ex.Message}");
        }
    }
}
