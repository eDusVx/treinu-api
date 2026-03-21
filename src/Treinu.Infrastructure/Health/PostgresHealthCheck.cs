using Microsoft.Extensions.Diagnostics.HealthChecks;
using Npgsql;

namespace Treinu.Infrastructure.Health;

public class PostgresHealthCheck : IHealthCheck
{
    private readonly string _connectionString;

    public PostgresHealthCheck(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = connection.CreateCommand();
            command.CommandText = "SELECT 1;";
            await command.ExecuteScalarAsync(cancellationToken);

            return HealthCheckResult.Healthy("PostgreSQL está respondendo.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy($"Falha ao conectar no PostgreSQL: {ex.Message}");
        }
    }
}