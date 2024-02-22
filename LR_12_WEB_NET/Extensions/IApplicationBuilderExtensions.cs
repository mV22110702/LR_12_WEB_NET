using LR6_WEB_NET.Middlewares;

namespace LR6_WEB_NET.Extensions;

public static class IApplicationBuilderExtensions
{
    public static void UseExceptionHandling(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}