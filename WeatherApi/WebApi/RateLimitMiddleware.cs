using System.Collections.Concurrent;

namespace WeatherApi;

public class RateLimitMiddleware
{
    private readonly RequestDelegate _next;
    private static readonly ConcurrentDictionary<string, (int Count, DateTime WindowStart)> _requests = new();
    private const int MaxRequests = 100;
    private const int WindowMinutes = 1;

    public RateLimitMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var now = DateTime.UtcNow;

        _requests.AddOrUpdate(ip,
            _ => (1, now),
            (_, existing) =>
            {
                if ((now - existing.WindowStart).TotalMinutes >= WindowMinutes)
                    return (1, now);
                return (existing.Count + 1, existing.WindowStart);
            });

        var current = _requests[ip];

        if (current.Count > MaxRequests)
        {
            context.Response.StatusCode = 429;
            await context.Response.WriteAsync("Prea multe requesturi! Incearca din nou in 1 minut.");
            return;
        }

        await _next(context);
    }
}