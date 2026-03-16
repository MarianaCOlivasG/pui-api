
using Microsoft.AspNetCore.Mvc;

namespace PUI.Api.Configuration
{
    public static class ApiBehaviorConfiguration
    {
        public static IServiceCollection AddApiValidation(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errores = context.ModelState
                        .Where(e => e.Value?.Errors.Count > 0)
                        .SelectMany(e => e.Value!.Errors.Select(x =>
                        {
                            if (!string.IsNullOrWhiteSpace(x.ErrorMessage))
                                return x.ErrorMessage;

                            if (e.Key == "$")
                                return "El JSON enviado no es válido.";

                            return $"El campo '{e.Key}' es inválido.";
                        }))
                        .Distinct()
                        .ToList();

                    var respuesta = new
                    {
                        mensaje = "Errores de validación",
                        errores,
                        traceId = context.HttpContext.TraceIdentifier
                    };

                    return new BadRequestObjectResult(respuesta);
                };
            });

            return services;
        }
    }
}