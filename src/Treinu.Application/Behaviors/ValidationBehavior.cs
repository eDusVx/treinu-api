using FluentValidation;
using FluentResults;
using Treinu.Domain.Core.Mediator;
using System.Reflection;

namespace Treinu.Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .Where(r => r.Errors.Any())
                .SelectMany(r => r.Errors)
                .ToList();

            if (failures.Count != 0)
            {
                if (typeof(TResponse) == typeof(Result))
                {
                    var failedResult = Result.Fail(failures.First().ErrorMessage);
                    foreach (var failure in failures.Skip(1))
                    {
                        failedResult.WithError(failure.ErrorMessage);
                    }
                    return (TResponse)(object)failedResult;
                }

                if (typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
                {
                    var failMethod = typeof(Result).GetMethods(BindingFlags.Public | BindingFlags.Static)
                        .First(m => m.IsGenericMethod && m.Name == "Fail" && m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType == typeof(string));

                    var genericFail = failMethod.MakeGenericMethod(typeof(TResponse).GetGenericArguments()[0]);
                    
                    var resultInstance = genericFail.Invoke(null, new object[] { failures.First().ErrorMessage });
                    var withErrorMethod = resultInstance!.GetType().GetMethod("WithError", new[] { typeof(string) });
                    
                    foreach (var failure in failures.Skip(1))
                    {
                        resultInstance = withErrorMethod!.Invoke(resultInstance, new object[] { failure.ErrorMessage });
                    }
                    
                    return (TResponse)resultInstance!;
                }

                throw new ValidationException(failures);
            }
        }

        return await next();
    }
}