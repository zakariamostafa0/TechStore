using ECommerce.Api.Helper;
using System.Net;
using System.Text.Json;

namespace ECommerce.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _enviroment;

        public ExceptionMiddleware(RequestDelegate next, IHostEnvironment enviroment)
        {
            _next = next;
            _enviroment = enviroment;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var response = _enviroment.IsDevelopment() ?
                    new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                    : new ApiException(context.Response.StatusCode, "Internal Server Error");
                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsJsonAsync(json);
            }
            finally
            {
                // Optional: Log the exception or perform any other actions
            }
        }
    }
}
