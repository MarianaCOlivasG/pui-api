using PUI.Application.Interfaces.Persistence;
using PUI.Application.Interfaces.Repositories;
using PUI.Domain.Entities;
using PUI.Domain.Enums;
using System.Diagnostics;
using System.Text;

namespace PUI.Api.Middlewares
{
    public class ApiLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiLoggingMiddleware> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public ApiLoggingMiddleware(
            RequestDelegate next,
            ILogger<ApiLoggingMiddleware> logger,
            IServiceScopeFactory scopeFactory)
        {
            _next = next;
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower() ?? "";

            // EXCLUIR HANGFIRE COMPLETAMENTE
            if (path.StartsWith("/hangfire"))
            {
                await _next(context);
                return;
            }

            var sw = Stopwatch.StartNew();

            string? requestBody = null;

            if (context.Request.ContentLength > 0 && context.Request.Body.CanRead)
            {
                context.Request.EnableBuffering();

                using var reader = new StreamReader(
                    context.Request.Body,
                    Encoding.UTF8,
                    leaveOpen: true);

                var body = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;

                requestBody = string.IsNullOrWhiteSpace(body) ? null : body;
            }

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

                await GuardarLog(context, requestBody, responseBody, sw.ElapsedMilliseconds);

                await responseBodyStream.CopyToAsync(originalResponseBodyStream);
            }
        }

        private async Task GuardarLog(
            HttpContext context,
            string? requestBody,
            string? responseBody,
            long duracion)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();

                var repo = scope.ServiceProvider.GetRequiredService<IApiLogsRepository>();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                requestBody = EsJsonValido(requestBody) ? Limitar(requestBody) : null;
                responseBody = EsJsonValido(responseBody) ? Limitar(responseBody) : null;

                var log = new ApiLog(
                    endpoint: context.Request.Path,
                    metodo: context.Request.Method,
                    direccion: DireccionApi.INBOUND,
                    requestBody: requestBody,
                    responseBody: responseBody,
                    httpStatus: context.Response.StatusCode,
                    ipOrigen: context.Connection.RemoteIpAddress?.ToString(),
                    duracionMs: (int)duracion
                );

                await repo.Agregar(log);
                await unitOfWork.Persistir();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error guardando log en base de datos");
            }
        }

        private bool EsJsonValido(string? json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return false;

            json = json.Trim();

            return (json.StartsWith("{") && json.EndsWith("}")) ||
                   (json.StartsWith("[") && json.EndsWith("]"));
        }

        private string Limitar(string input, int max = 5000)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return input.Length <= max ? input : input.Substring(0, max);
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