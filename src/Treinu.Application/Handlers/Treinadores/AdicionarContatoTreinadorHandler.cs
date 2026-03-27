using FluentResults;
using Treinu.Contracts.Commands;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Entities;
using Treinu.Domain.Factories;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Treinadores;

public class AdicionarContatoTreinadorHandler(
    IUsuarioRepository usuarioRepository) : IRequestHandler<AdicionarContatoTreinadorCommand, Result<object>>
{
    public async Task<Result<object>> Handle(AdicionarContatoTreinadorCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var treinadorResult = await usuarioRepository.BuscarTreinadorPorIdAsync(request.TreinadorId, cancellationToken);
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

            var addResult = treinadorResult.Value.AdicionarContato(contatoResult.Value);
            if (addResult.IsFailed) return Result.Fail<object>(addResult.Errors);

            var saveResult = await usuarioRepository.AtualizarUsuarioAsync(treinadorResult.Value);
            if (saveResult.IsFailed) return Result.Fail<object>(saveResult.Errors);

            return Result.Ok(treinadorResult.Value.ToDto());
        }
        catch (Exception ex)
        {
            return Result.Fail<object>($"Erro inesperado ao adicionar contato: {ex.Message}");
        }
    }
}
