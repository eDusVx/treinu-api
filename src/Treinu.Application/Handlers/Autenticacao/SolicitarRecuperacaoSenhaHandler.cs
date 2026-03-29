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

            var subject = "Recuperação de Senha - Treinu";
            var body = $@"
            <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; color: #333;'>
                <div style='background-color: #0f172a; padding: 20px; text-align: center; border-radius: 8px 8px 0 0;'>
                    <h1 style='color: #fff; margin: 0;'>Treinu App</h1>
                </div>
                <div style='padding: 30px; background-color: #f8fafc; border: 1px solid #e2e8f0; border-top: none; border-radius: 0 0 8px 8px;'>
                    <h2 style='color: #0f172a; margin-top: 0;'>Recuperação de Senha</h2>
                    <p style='font-size: 16px; line-height: 1.5;'>Olá,</p>
                    <p style='font-size: 16px; line-height: 1.5;'>Recebemos uma solicitação de recuperação de senha para sua conta no <strong>Treinu App</strong>. Se foi você, utilize o código de 6 dígitos abaixo no aplicativo para redefinir sua senha:</p>
                    
                    <div style='background-color: #e2e8f0; border-radius: 6px; padding: 20px; text-align: center; margin: 30px 0;'>
                        <span style='font-size: 32px; font-weight: bold; letter-spacing: 5px; color: #0f172a;'>{token}</span>
                    </div>
                    
                    <p style='font-size: 14px; color: #64748b; line-height: 1.5;'>Este código é válido por <strong>2 horas</strong>. Caso você não tenha solicitado, por favor, desconsidere este e-mail por segurança.</p>
                </div>
                <div style='text-align: center; margin-top: 20px; font-size: 12px; color: #94a3b8;'>
                    <p>&copy; {DateTime.UtcNow.Year} Treinu App. Todos os direitos reservados.</p>
                </div>
            </div>";

            await _emailService.SendEmailAsync(credencial.Email, subject, body);

            return Result.Ok<object>(new { Mensagem = "Se o e-mail existir, você receberá um link de recuperação." });
        }
        catch (Exception ex)
        {
            return Result.Fail<object>($"Erro inesperado na solicitação de recuperação de senha: {ex.Message}");
        }
    }
}
