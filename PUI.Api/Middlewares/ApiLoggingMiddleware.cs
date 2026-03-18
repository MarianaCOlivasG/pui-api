using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace PUI.Api.Middlewares
{
    public class ApiLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiLoggingMiddleware> _logger;

        public ApiLoggingMiddleware(RequestDelegate next, ILogger<ApiLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var sw = Stopwatch.StartNew();

            // Leer request
            string requestBody = string.Empty;
            if (context.Request.ContentLength > 0 && context.Request.Body.CanSeek)
            {
                context.Request.EnableBuffering();
                using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
                requestBody = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;
            }

            // Capturar response
            var originalResponseBodyStream = context.Response.Body;
            using var responseBodyStream = new MemoryStream();
            context.Response.Body = responseBodyStream;

            try
            {
                await _next(context);
            }
            finally
            {
                sw.Stop();
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                string responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
                context.Response.Body.Seek(0, SeekOrigin.Begin);

               
                _logger.LogInformation("==== API LOG ====");
                _logger.LogInformation("Endpoint: {Endpoint}", context.Request.Path);
                _logger.LogInformation("Método: {Metodo}", context.Request.Method);
                _logger.LogInformation("IP Origen: {IpOrigen}", context.Connection.RemoteIpAddress?.ToString());
                _logger.LogInformation("RequestBody: {RequestBody}", requestBody);
                _logger.LogInformation("ResponseBody: {ResponseBody}", responseBody);
                _logger.LogInformation("StatusCode: {StatusCode}", context.Response.StatusCode);
                _logger.LogInformation("Duración: {Duracion} ms", sw.ElapsedMilliseconds);
                _logger.LogInformation("================");

                await responseBodyStream.CopyToAsync(originalResponseBodyStream);
            }
        }
    }

    public static class ApiLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseApiLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiLoggingMiddleware>();
        }
    }
}