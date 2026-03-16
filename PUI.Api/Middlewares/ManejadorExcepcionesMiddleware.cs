using System.Net;
using System.Text.Json;
using PUI.Domain.Exceptions;
using PUI.Infrastructure.Identity.Exceptions;
using PUI.Persistence.Exceptions;

namespace PUI.Api.Middlewares
{
    public class ManejadorExcepcionesMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ManejadorExcepcionesMiddleware> _logger;

        public ManejadorExcepcionesMiddleware(
            RequestDelegate next,
            ILogger<ManejadorExcepcionesMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await ManejarExcepcion(context, ex);
            }
        }

        private Task ManejarExcepcion(HttpContext context, Exception excepcion)
        {
            HttpStatusCode httpStatusCode;
            object resultado;

            switch (excepcion)
            {
                case ExcepcionNoEncontrado:
                    httpStatusCode = HttpStatusCode.NotFound;
                    resultado = new
                    {
                        mensaje = "El recurso solicitado no fue encontrado.",
                        traceId = context.TraceIdentifier
                    };
                    break;

                case ExcepcionDeValidacion ex:
                    httpStatusCode = HttpStatusCode.BadRequest;
                    resultado = new
                    {
                        mensaje = "Los datos enviados no son válidos.",
                        errores = ex.ErroresDeValidacion,
                        traceId = context.TraceIdentifier
                    };
                    break;

                case ExcepcionReglaNegocio ex:
                    httpStatusCode = HttpStatusCode.BadRequest;
                    resultado = new
                    {
                        mensaje = ex.Message,
                        traceId = context.TraceIdentifier
                    };
                    break;

                case ExcepcionDePersistencia ex:
                    httpStatusCode = HttpStatusCode.BadRequest;

                    _logger.LogError(ex,
                        "Error de persistencia. Código: {Codigo} Detalle: {Detalle}",
                        ex.CodigoError,
                        ex.Detalle);

                    resultado = new
                    {
                        mensaje = "No fue posible guardar la información. Intente nuevamente.",
                        traceId = context.TraceIdentifier
                    };
                    break;
                    
                case IdentityValidationException ex:
                    httpStatusCode = HttpStatusCode.BadRequest;
                    resultado = new
                    {
                        mensaje = "Error de validación de identidad.",
                        errores = ex.Errores,
                        traceId = context.TraceIdentifier
                    };
                    break;

                default:
                    httpStatusCode = HttpStatusCode.InternalServerError;

                    _logger.LogError(excepcion,
                        "Error inesperado en el servidor");

                    resultado = new
                    {
                        mensaje = "Ocurrió un error inesperado. Intente más tarde.",
                        traceId = context.TraceIdentifier
                    };
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)httpStatusCode;

            var json = JsonSerializer.Serialize(resultado);

            return context.Response.WriteAsync(json);
        }
    }

    public static class ManejadorExcepcionesMiddlewareExtensions
    {
        public static IApplicationBuilder UseManejadorExcepciones(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ManejadorExcepcionesMiddleware>();
        }
    }
}