using FluentResults;
using Treinu.Contracts.Commands.Treinadores;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Entities;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Treinadores;

public class AdicionarContatoTreinadorHandler(
    IUsuarioRepository usuarioRepository,
    IContatoRepository contatoRepository) : IRequestHandler<AdicionarContatoTreinadorCommand, Result<object>>
{
    public async Task<Result<object>> Handle(AdicionarContatoTreinadorCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var treinadorResult =
                await usuarioRepository.BuscarTreinadorPorIdAsync(request.TreinadorId, cancellationToken);
            if (treinadorResult.IsFailed) return Result.Fail<object>(treinadorResult.Errors);

            var contatoResult = Contato.Criar(new CriarContatoProps(
                request.Tipo,
                request.Valor,
                request.Descricao,
                request.Principal,
                request.Plataforma,
                request.NomeExibicao
            ));
            if (contatoResult.IsFailed) return Result.Fail<object>(contatoResult.Errors);

            var saveResult = await contatoRepository.AdicionarContatoAsync(contatoResult.Value, request.TreinadorId, cancellationToken);
            if (saveResult.IsFailed) return Result.Fail<object>(saveResult.Errors);

            // Recarrega o treinador com os contatos atualizados para retornar o DTO completo
            var treinadorAtualizado = await usuarioRepository.BuscarTreinadorPorIdAsync(request.TreinadorId, cancellationToken);
            if (treinadorAtualizado.IsFailed) return Result.Fail<object>(treinadorAtualizado.Errors);

            return Result.Ok(treinadorAtualizado.Value.ToDto());
        }
        catch (Exception ex)
        {
            return Result.Fail<object>($"Erro inesperado ao adicionar contato: {ex.Message}");
        }
    }
}