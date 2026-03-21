using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Treinu.Infrastructure.Health;

public static class HealthCheckExtensions
{
    public static IHealthChecksBuilder AddPostgresHealthCheck(this IServiceCollection services, string connectionString)
    {
        return services.AddHealthChecks()
            .AddCheck("postgres", new PostgresHealthCheck(connectionString), tags: new[] { "db", "postgresql" });
    }

    public static IEndpointConventionBuilder MapCustomHealthChecks(this IEndpointRouteBuilder endpoints,
        string pattern = "/health")
    {
        return endpoints.MapHealthChecks(pattern, new HealthCheckOptions
        {
            ResponseWriter = HealthCheckResponseWriter.WriteResponse
        });
    }
}