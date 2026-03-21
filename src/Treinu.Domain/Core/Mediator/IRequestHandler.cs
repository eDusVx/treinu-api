namespace Treinu.Domain.Core.Mediator;

public interface IRequestHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}

public interface IRequestHandler<in TRequest> : IRequestHandler<TRequest, object> where TRequest : IRequest<object>
{
}
