using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Treinu.Domain.Core.Mediator;
using Treinu.Application.Mediator;

namespace Treinu.Application.Extensions;

public static class MediatorServiceCollectionExtensions
{
    public static IServiceCollection AddCustomMediator(this IServiceCollection services, Assembly assembly)
    {
        services.AddScoped<IMediator, CustomMediator>();

        var types = assembly.GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface);

        foreach (var type in types)
        {
            var interfaces = type.GetInterfaces();

            foreach (var @interface in interfaces)
            {
                if (!@interface.IsGenericType) continue;

                var genericTypeDefinition = @interface.GetGenericTypeDefinition();
                
                if (genericTypeDefinition == typeof(IRequestHandler<,>) ||
                    genericTypeDefinition == typeof(INotificationHandler<>) ||
                    genericTypeDefinition == typeof(IPipelineBehavior<,>))
                {
                    if (type.IsGenericTypeDefinition)
                    {
                        services.AddTransient(genericTypeDefinition, type.GetGenericTypeDefinition());
                    }
                    else
                    {
                        services.AddTransient(@interface, type);
                    }
                }
            }
        }

        return services;
    }
}
