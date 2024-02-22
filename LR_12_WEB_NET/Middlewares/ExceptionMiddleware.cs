using System.Net;
using System.Web.Http;
using LR6_WEB_NET.Models.Dto;
using Serilog;

namespace LR6_WEB_NET.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception rawException)
    {
        if (rawException is HttpResponseException exception)
        {
            var reader = new StreamReader(exception.Response.Content.ReadAsStream());
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)exception.Response.StatusCode;
            var responseText = reader.ReadToEnd();
            Log.Error("Exception: {ResponseText}", responseText);
            var responseDto = new ResponseDto<int>
            {
                Description = responseText,
                StatusCode = (int)exception.Response.StatusCode,
            };
            await context.Response.WriteAsJsonAsync(responseDto);
        }
        else
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var responseText = rawException.Message;
            Log.Error("Exception: {ResponseText}", responseText);
            var responseDto = new ResponseDto<int>
            {
                Description = responseText,
                StatusCode = (int)HttpStatusCode.InternalServerError,
            };
            await context.Response.WriteAsJsonAsync(responseDto);
        }
    }
}