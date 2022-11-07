using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Shared.Extensions;

public class CorsMiddleware
{
    private readonly RequestDelegate _next;

    public CorsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task Invoke(HttpContext httpContext)
    {
        httpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
        httpContext.Response.Headers.Add("Access-Control-Allow-Headers", "*");
        httpContext.Response.Headers.Add("Access-Control-Allow-Methods", "POST, DELETE, OPTIONS");
        return _next(httpContext);
    }
}

public static class CorsMiddlewareExtensions
{
    public static IApplicationBuilder UseCorsMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CorsMiddleware>();
    }
}