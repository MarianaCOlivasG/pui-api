using PUI.Api.Configuration;
using PUI.Api.Middlewares;
using PUI.Application;
using PUI.Identity;
using PUI.Persistencia;

var builder = WebApplication.CreateBuilder(args);

// OpenAPI
builder.Services.AddOpenApi();

// Servicios
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

builder.Services.AddControllers();
builder.Services.AddApiValidation();

var app = builder.Build();

// Middleware
app.UseHttpsRedirection();

app.UseCors();

app.UseManejadorExcepciones();

app.UseAuthentication();
app.UseAuthorization();

// OpenAPI
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllers();

app.Run();