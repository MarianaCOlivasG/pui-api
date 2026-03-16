using Microsoft.AspNetCore.Mvc;
using PUI.Api.Middlewares;
using PUI.Application;
using PUI.Identity;
using PUI.Persistencia;

var builder = WebApplication.CreateBuilder(args);

// OpenAPI
builder.Services.AddOpenApi();

// Servicios de la aplicación
builder.Services.AgregarServiciosDeAplicacion();
builder.Services.AgregarServiciosDePersistencia(builder.Configuration);
builder.Services.AgregarServiciosDeIdentity(builder.Configuration);

// CORS
var origenesPermitidos = builder.Configuration
    .GetSection("origenesPermitidos")
    .Get<string[]>()!;

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(origenesPermitidos) 
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();  
    });
});

// Controllers + manejo limpio de errores de validación
builder.Services
    .AddControllers()
    .ConfigureApiBehaviorOptions(options =>
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

var app = builder.Build();

// Middleware
app.UseHttpsRedirection();

app.UseCors();

app.UseManejadorExcepciones();

app.UseAuthentication();
app.UseAuthorization();

// OpenAPI solo en desarrollo
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllers();

app.Run();