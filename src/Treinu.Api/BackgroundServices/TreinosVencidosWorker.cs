using Treinu.Domain.Core.Mediator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Treinu.Domain.Repositories;

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
                            
                            // Disparar eventos (Mediator.Publish pois AtualizarTreinoAsync não chama _mediator.DispatchDomainEventsAsync do DbContext diretamente pelo repository method dependendo da implementação, mas o DbContext fará no SaveChangesAsync!
                            // Se o SaveChangesAsync dispara, não precisamos dar Publish manual, mas como o worker não chama um handler CQRS, o SaveChanges do Repositorio já dispara se tiver `await _mediator.DispatchDomainEventsAsync(this);` no AppDbContext.
                        }
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
