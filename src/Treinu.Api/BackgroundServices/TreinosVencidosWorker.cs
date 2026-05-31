using Treinu.Domain.Core.Mediator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Treinu.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Treinu.Api.BackgroundServices;

public class TreinosVencidosWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<TreinosVencidosWorker> _logger;

    public TreinosVencidosWorker(IServiceProvider serviceProvider, ILogger<TreinosVencidosWorker> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("TreinosVencidosWorker rodando em: {time}", DateTimeOffset.Now);

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var treinoRepository = scope.ServiceProvider.GetRequiredService<ITreinoRepository>();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                var vencidosResult = await treinoRepository.BuscarTreinosVencidosAtivosAsync(DateTime.UtcNow, stoppingToken);

                if (vencidosResult.IsSuccess)
                {
                    foreach (var treino in vencidosResult.Value)
                    {
                        var updatedResult = treino.AtualizarStatus();
                        if (updatedResult.IsSuccess)
                        {
                            await treinoRepository.AtualizarTreinoAsync(treino);
                        }
                    }
                }

                // Check for students with NO active workouts
                var context = scope.ServiceProvider.GetRequiredService<Treinu.Infrastructure.Data.AppDbContext>();
                var notificacaoRepository = scope.ServiceProvider.GetRequiredService<INotificacaoRepository>();

                var alunosInativosIds = await context.Set<Treinu.Domain.Entities.Aluno>()
                    .Where(a => !context.Set<Treinu.Domain.Entities.Treino>().Any(t => t.AlunoId == a.Id && t.Status == Treinu.Domain.Enums.TreinoStatusEnum.ATIVO))
                    .Select(a => a.Id)
                    .ToListAsync(stoppingToken);

                foreach (var alunoId in alunosInativosIds)
                {
                    var notificacao = Treinu.Domain.Entities.Notificacao.Criar(new Treinu.Domain.Entities.CriarNotificacaoProps(
                        alunoId,
                        "Alerta de Abandono",
                        "Você não possui treinos ativos no momento. Fale com seu treinador!"
                    ));

                    if (notificacao.IsSuccess)
                    {
                        await notificacaoRepository.AdicionarNotificacaoAsync(notificacao.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar treinos vencidos.");
            }

            // Aguarda 24 horas
            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }
}
