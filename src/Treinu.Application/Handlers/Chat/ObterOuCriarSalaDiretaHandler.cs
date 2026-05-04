using FluentResults;
using Treinu.Contracts.Commands.Chat;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Entities.Chat;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Chat;

public class ObterOuCriarSalaDiretaHandler(IChatRepository chatRepository) 
    : IRequestHandler<ObterOuCriarSalaDiretaCommand, Result<object>>
{
    public async Task<Result<object>> Handle(ObterOuCriarSalaDiretaCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.UsuarioLogadoId == request.OutroUsuarioId)
                return Result.Fail<object>("Você não pode criar um chat consigo mesmo.");

            // Tenta buscar a sala direta já existente
            var salaExistenteResult = await chatRepository.BuscarSalaDiretaAsync(request.UsuarioLogadoId, request.OutroUsuarioId, cancellationToken);
            
            if (salaExistenteResult.IsSuccess)
            {
                var sala = salaExistenteResult.Value;
                return Result.Ok((object)new 
                { 
                    SalaId = sala.Id,
                    Nome = sala.Nome,
                    Tipo = sala.Tipo.ToString(),
                    CriadaAgora = false
                });
            }

            // Se não encontrou, cria a sala na hora
            var novaSalaResult = SalaChat.CriarDireta(request.UsuarioLogadoId, request.OutroUsuarioId);
            if (novaSalaResult.IsFailed) return Result.Fail<object>(novaSalaResult.Errors);

            var novaSala = novaSalaResult.Value;
            var saveResult = await chatRepository.AdicionarSalaAsync(novaSala);
            if (saveResult.IsFailed) return Result.Fail<object>(saveResult.Errors);

            return Result.Ok((object)new 
            { 
                SalaId = novaSala.Id,
                Nome = novaSala.Nome,
                Tipo = novaSala.Tipo.ToString(),
                CriadaAgora = true
            });
        }
        catch (Exception ex)
        {
            return Result.Fail<object>($"Erro inesperado ao obter ou criar sala: {ex.Message}");
        }
    }
}
