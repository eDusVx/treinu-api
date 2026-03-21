using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Treinu.Infrastructure.Health;

public static class HealthCheckResponseWriter
{
    public static async Task WriteResponse(HttpContext context, HealthReport report)
    {
        context.Response.ContentType = "application/json";
        var response = new
        {
            Origem = "Treinu.Api API",
            StatusGlobal = report.Status.ToString(),
            Servicos = report.Entries.Select(e => new
            {
                Nome = e.Key,
                Status = e.Value.Status.ToString(),
                Mensagem = e.Value.Description,
                e.Value.Tags,
                Latencia = e.Value.Duration.ToString()
            })
        };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response,
            new JsonSerializerOptions { WriteIndented = true }));
    }
}