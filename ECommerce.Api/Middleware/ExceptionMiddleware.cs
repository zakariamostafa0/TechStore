using ECommerce.Api.Helper;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Text.Json;

namespace ECommerce.Api.Middleware
{
    public class ExceptionMiddleware
    {
        #region Fields
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _enviroment;
        private readonly IMemoryCache _memoryCache;

        private readonly TimeSpan _rateLimitWindow = TimeSpan.FromSeconds(30);
        #endregion
        #region Constractors
        public ExceptionMiddleware(RequestDelegate next, IHostEnvironment enviroment, IMemoryCache memoryCache)
        {
            _next = next;
            _enviroment = enviroment;
            _memoryCache = memoryCache;
        }
        #endregion

        #region Methods Handling
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                ApplySecurity(context);

                if (!IsRequestAllowed(context))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                    context.Response.ContentType = "application/json";
                    var response = new
                        ApiException(context.Response.StatusCode, "Too many requests. Try again later.");

                    await context.Response.WriteAsJsonAsync(response);
                    return;
                }
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

        #region Private Methods
        private bool IsRequestAllowed(HttpContext context)
        {
            var ipAddress = context.Connection.RemoteIpAddress.ToString();
            var cashKey = $"Rate:{ipAddress}";
            var dateNow = DateTime.UtcNow;

            var (timesTamp, count) = _memoryCache.GetOrCreate(cashKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = _rateLimitWindow;
                return (timesTamp: dateNow, count: 0);
            });
            if (dateNow - timesTamp < _rateLimitWindow)
            {
                if (count >= 8)
                {
                    return false;
                }
                _memoryCache.Set(cashKey, (timesTamp, count += 1), _rateLimitWindow);
            }
            else
            {
                _memoryCache.Set(cashKey, (timesTamp, 1), _rateLimitWindow);
            }
            return true;
        }

        private void ApplySecurity(HttpContext context)
        {
            context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            context.Response.Headers.Add("X-Frame-Options", "DENY");
            context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
        }
        #endregion
        #endregion

    }
}
