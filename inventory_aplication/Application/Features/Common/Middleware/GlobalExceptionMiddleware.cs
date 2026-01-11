using inventory_aplication.Application.Features.Common.Results;
using System.Net;
using System.Text.Json;

namespace inventory_aplication.Application.Features.Common.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
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

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Configuramos el status code, 500 por defecto
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            // Creamos el Result<string> usando tu estructura
            var result = Result<string>.Fail(exception.Message, ErrorCodes.InternalError);

            // Convertimos a JSON
            var json = JsonSerializer.Serialize(result);

            return context.Response.WriteAsync(json);
        }
    }
}
