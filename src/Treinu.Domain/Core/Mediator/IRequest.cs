namespace Treinu.Domain.Core.Mediator;

public interface IRequest<out TResponse> { }

public interface IRequest : IRequest<object> { }
