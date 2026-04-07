using FluentResults;
using Treinu.Contracts.Commands.Autenticacao;
using Treinu.Domain.Core;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Errors;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Autenticacao;

public class SolicitarRecuperacaoSenhaHandler : IRequestHandler<SolicitarRecuperacaoSenhaCommand, Result<object>>
{
    private readonly ICredencialRepository _credencialRepository;
    private readonly IEmailService _emailService;

    public SolicitarRecuperacaoSenhaHandler(ICredencialRepository credencialRepository, IEmailService emailService)
    {
        _credencialRepository = credencialRepository;
        _emailService = emailService;
    }

    public async Task<Result<object>> Handle(SolicitarRecuperacaoSenhaCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var credencialResult = await _credencialRepository.BuscarCredencialPorEmailAsync(request.Email);
            if (credencialResult.IsFailed) return Result.Fail<object>(credencialResult.Errors);

            var credencial = credencialResult.Value;
            if (credencial == null)
            {
                // Para não vazar se o e-mail existe, retornamos sucesso silenciosamente
                return Result.Ok<object>(new { Mensagem = "Se o e-mail existir, você receberá um link de recuperação." });
            }

            var random = new Random();
            var token = random.Next(100000, 999999).ToString();
            credencial.GerarTokenRecuperacaoSenha(token, DateTime.UtcNow.AddHours(2));

            var updateResult = await _credencialRepository.AtualizarCredencialAsync(credencial);
            if (updateResult.IsFailed) return Result.Fail<object>(updateResult.Errors);

            await _emailService.EnviarRecuperacaoSenhaAsync(credencial.Email, "Usuário", token);

            return Result.Ok<object>(new { Mensagem = "Se o e-mail existir, você receberá um link de recuperação." });
        }
        catch (Exception ex)
        {
            return Result.Fail<object>($"Erro inesperado na solicitação de recuperação de senha: {ex.Message}");
        }
    }
}
