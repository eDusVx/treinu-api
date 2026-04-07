using FluentResults;
using Treinu.Contracts.Commands.Autenticacao;
using Treinu.Domain.Core;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Errors;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Autenticacao;

public class SolicitarCodigoLoginHandler : IRequestHandler<SolicitarCodigoLoginCommand, Result<object>>
{
    private readonly ICredencialRepository _credencialRepository;
    private readonly IEmailService _emailService;

    public SolicitarCodigoLoginHandler(ICredencialRepository credencialRepository, IEmailService emailService)
    {
        _credencialRepository = credencialRepository;
        _emailService = emailService;
    }

    public async Task<Result<object>> Handle(SolicitarCodigoLoginCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var credencialResult = await _credencialRepository.BuscarCredencialPorEmailAsync(request.Email);
            if (credencialResult.IsFailed) return Result.Fail<object>(credencialResult.Errors);

            var credencial = credencialResult.Value;
            if (credencial == null)
            {
                // Segurança: não revelar se o e-mail existe
                return Result.Ok<object>(new { Mensagem = "Se o e-mail estiver cadastrado, um código de login foi enviado." });
            }

            var random = new Random();
            var token = random.Next(100000, 999999).ToString();
            credencial.GerarCodigoLogin(token, DateTime.UtcNow.AddMinutes(15)); // 15 minutos de validade para login

            var updateResult = await _credencialRepository.AtualizarCredencialAsync(credencial);
            if (updateResult.IsFailed) return Result.Fail<object>(updateResult.Errors);

            await _emailService.EnviarVerificacaoEmailAsync(credencial.Email, "Usuário", token);

            return Result.Ok<object>(new { Mensagem = "Se o e-mail estiver cadastrado, um código de login foi enviado." });
        }
        catch (Exception ex)
        {
            return Result.Fail<object>($"Erro inesperado ao solicitar código de login: {ex.Message}");
        }
    }
}
