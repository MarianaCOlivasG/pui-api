using PUI.Domain.Exceptions;
using PUI.Infrastructure.Identity.Exceptions;
using PUI.Infrastructure.Infrastructure.Exceptions;
using PUI.Persistence.Exceptions;
using System.Net;
using System.Text.Json;

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
            var traceId = context.TraceIdentifier;

            HttpStatusCode status;
            string mensaje;
            string codigo = "ERROR_GENERAL";
            object? errores = null;

            switch (excepcion)
            {
                case ExcepcionNoEncontrado:
                    status = HttpStatusCode.NotFound;
                    mensaje = "Recurso no encontrado.";
                    codigo = "NOT_FOUND";
                    break;

                case ExcepcionDeValidacion ex:
                    status = HttpStatusCode.BadRequest;
                    mensaje = "Error en la solicitud.";
                    codigo = "VALIDATION_ERROR";
                    errores = ex.ErroresDeValidacion;
                    break;

                case ExcepcionReglaNegocio ex:
                    status = HttpStatusCode.BadRequest;
                    mensaje = ex.Message;
                    codigo = "BUSINESS_RULE";
                    break;

                case ExcepcionDePersistencia ex:
                    status = HttpStatusCode.InternalServerError;
                    mensaje = "Error al procesar la información.";
                    codigo = "PERSISTENCE_ERROR";

                    _logger.LogError(ex,
                        "Error de persistencia. Código: {Codigo} Detalle: {Detalle}",
                        ex.CodigoError,
                        ex.Detalle);
                    break;

                case ExcepcionValidacionIdentity ex:
                    status = HttpStatusCode.BadRequest;
                    mensaje = "Error de validación de identidad.";
                    codigo = "IDENTITY_VALIDATION";
                    errores = ex.Errores;
                    break;

                case UnauthorizedAccessException:
                    status = HttpStatusCode.Unauthorized;
                    mensaje = "Falta o falla de autenticación.";
                    codigo = "UNAUTHORIZED";
                    break;

                case TimeoutException:
                    status = HttpStatusCode.GatewayTimeout;
                    mensaje = "El servidor tardó demasiado en responder.";
                    codigo = "TIMEOUT";
                    break;

                case ApiPuiException ex:
                    status = (HttpStatusCode)ex.StatusCode;
                    codigo = string.IsNullOrEmpty(ex.Codigo) ? "PUI_ERROR" : ex.Codigo;
                    mensaje = $"Error en servicio externo (PUI): {ex.Message}";
                    errores = ex.Errores;
                    _logger.LogWarning(ex,
                        "Error en API PUI. StatusCode: {StatusCode}, Codigo: {Codigo}",
                        ex.StatusCode,
                        ex.Codigo);

                    break;

                default:
                    status = HttpStatusCode.InternalServerError;
                    mensaje = "Error interno del servidor.";
                    codigo = "INTERNAL_ERROR";

                    _logger.LogError(excepcion, "Error inesperado");
                    break;
            }

            var response = new
            {
                exito = false,
                codigo,
                mensaje,
                errores,
                traceId
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;

            var json = JsonSerializer.Serialize(response);

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