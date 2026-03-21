using Microsoft.Extensions.DependencyInjection;
using Treinu.Domain.Core.Mediator;

namespace Treinu.Application.Mediator;

public class CustomMediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;

    public CustomMediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request,
        CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResponse));

        var handler = _serviceProvider.GetService(handlerType);

        if (handler == null)
            throw new InvalidOperationException(
                $"Nenhum handler registrado para {requestType.Name}. O injetor de dependências não varreu o assembly ou a interface não bate.");

        var handlerMethod = handlerType.GetMethod("Handle");
        if (handlerMethod == null)
            throw new InvalidOperationException($"Método Handle não encontrado no handler para {requestType.Name}.");

        var pipelineType = typeof(IPipelineBehavior<,>).MakeGenericType(requestType, typeof(TResponse));
        var pipelines = _serviceProvider.GetServices(pipelineType).Cast<object>().ToList();

        RequestHandlerDelegate<TResponse> next = () =>
            (Task<TResponse>)handlerMethod.Invoke(handler, new object[] { request, cancellationToken })!;

        foreach (var pipeline in Enumerable.Reverse(pipelines))
        {
            var nextClosure = next;
            var pipelineMethod = pipelineType.GetMethod("Handle")!;
            next = () =>
                (Task<TResponse>)pipelineMethod.Invoke(pipeline,
                    new object[] { request, nextClosure, cancellationToken })!;
        }

        return await next();
    }

    public async Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
        where TNotification : INotification
    {
        var notificationType = notification.GetType();
        var handlerType = typeof(INotificationHandler<>).MakeGenericType(notificationType);

        var handlers = _serviceProvider.GetServices(handlerType);

        var method = handlerType.GetMethod("Handle");
        if (method == null) return;

        var tasks = handlers.Select(handler =>
            (Task)method.Invoke(handler, new object[] { notification, cancellationToken })!);
        await Task.WhenAll(tasks);
    }
}