using FluentResults;
using Treinu.Contracts.Commands;
using Treinu.Domain.Core;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Entities;
using Treinu.Domain.Enums;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Convites;

public class ConvidarAlunoHandler(
    IConviteRepository conviteRepository,
    IEmailService emailService) : IRequestHandler<ConvidarAlunoCommand, Result>
{
    public async Task<Result> Handle(ConvidarAlunoCommand request, CancellationToken cancellationToken)
    {
        var existePendente = await conviteRepository.ExisteConvitePendenteParaEmailAsync(request.Email);
        if (existePendente)
            return Result.Fail("Já existe um convite pendente para este e-mail.");

        var conviteResult = Convite.Criar(request.Email, PerfilEnum.ALUNO, request.TreinadorId);
        if (conviteResult.IsFailed) return Result.Fail(conviteResult.Errors);

        await emailService.EnviarConviteAsync(conviteResult.Value.Email, conviteResult.Value.Token, conviteResult.Value.Perfil);

        var saveResult = await conviteRepository.SalvarConviteAsync(conviteResult.Value);
        if (saveResult.IsFailed) return Result.Fail(saveResult.Errors);

        return Result.Ok();
    }
}
