using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Treinu.Infrastructure.Data;
using Treinu.Domain.Repositories;
using Treinu.Domain.Entities;

namespace Treinu.Api.BackgroundServices;

public class AvaliacoesProximasWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<AvaliacoesProximasWorker> _logger;

    public AvaliacoesProximasWorker(IServiceProvider serviceProvider, ILogger<AvaliacoesProximasWorker> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("AvaliacoesProximasWorker rodando em: {time}", DateTimeOffset.Now);

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var notificacaoRepository = scope.ServiceProvider.GetRequiredService<INotificacaoRepository>();

                var hoje = DateTime.UtcNow.Date;
                var dataAlvo = hoje.AddDays(7); // Alerta faltando 7 dias

                var avaliacoesVencendo = await context.Set<Treinu.Domain.Entities.AvaliacaoFisica.AvaliacaoFisica>()
                    .Where(a => a.DataProximaAvaliacao.HasValue && a.DataProximaAvaliacao.Value.Date == dataAlvo)
                    .Select(a => new { Avaliacao = a, AlunoId = EF.Property<Guid>(a, "AlunoId") })
                    .ToListAsync(stoppingToken);

                foreach (var item in avaliacoesVencendo)
                {
                    var avaliacao = item.Avaliacao;
                    var alunoId = item.AlunoId;

                    var notificacao = Notificacao.Criar(new CriarNotificacaoProps(
                        alunoId,
                        "Avaliação Física Próxima",
                        $"Sua próxima avaliação física está agendada para {avaliacao.DataProximaAvaliacao.Value:dd/MM/yyyy}."
                    ));

                    if (notificacao.IsSuccess)
                    {
                        await notificacaoRepository.AdicionarNotificacaoAsync(notificacao.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar avaliações próximas.");
            }

            // Aguarda 24 horas
            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }
}
