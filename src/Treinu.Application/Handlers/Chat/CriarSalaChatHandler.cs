using FluentResults;
using Treinu.Contracts.Commands.Chat;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Entities.Chat;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Chat;

public class CriarSalaChatHandler(IChatRepository chatRepository, IUsuarioRepository usuarioRepository) 
    : IRequestHandler<CriarSalaChatCommand, Result<object>>
{
    public async Task<Result<object>> Handle(CriarSalaChatCommand request, CancellationToken cancellationToken)
    {
        try
        {
            SalaChat sala;

            if (request.EhGrupo)
            {
                var salaResult = SalaChat.CriarGrupo(request.Nome, request.UsuarioCriadorId, request.ParticipantesIds);
                if (salaResult.IsFailed) return Result.Fail<object>(salaResult.Errors);
                sala = salaResult.Value;
            }
            else
            {
                if (request.ParticipantesIds.Count != 1)
                    return Result.Fail<object>("Chat direto precisa de exatamente 1 outro participante.");

                var usuario2Id = request.ParticipantesIds.First();

                var existenteResult = await chatRepository.BuscarSalaDiretaAsync(request.UsuarioCriadorId, usuario2Id, cancellationToken);
                if (existenteResult.IsSuccess)
                    return Result.Ok((object)new { SalaId = existenteResult.Value.Id }); // Sala já existe

                var salaResult = SalaChat.CriarDireta(request.UsuarioCriadorId, usuario2Id);
                if (salaResult.IsFailed) return Result.Fail<object>(salaResult.Errors);
                sala = salaResult.Value;
            }

            var saveResult = await chatRepository.AdicionarSalaAsync(sala);
            if (saveResult.IsFailed) return Result.Fail<object>(saveResult.Errors);

            return Result.Ok((object)new { SalaId = sala.Id });
        }
        catch (Exception ex)
        {
            return Result.Fail<object>($"Erro inesperado ao criar sala de chat: {ex.Message}");
        }
    }
}
