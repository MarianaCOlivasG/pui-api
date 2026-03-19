using PUI.Api.Configuration;
using PUI.Api.Filters;
using PUI.Api.Middlewares;
using PUI.Application;
using PUI.Identity;
using PUI.Infrastructure.Jobs;
using PUI.Persistencia;

var builder = WebApplication.CreateBuilder(args);

// OpenAPI
builder.Services.AddOpenApi();

// Servicios
builder.Services.AgregarServiciosDeAplicacion();
builder.Services.AgregarServiciosDePersistencia(builder.Configuration);
builder.Services.AgregarServiciosDeInfraestructure(builder.Configuration);
builder.Services.AgregarServiciosDeIdentity(builder.Configuration);

// JOB
//builder.Services.AddHostedService<BusquedaContinuaJob>();
builder.Services.AddHostedService<BusquedaContinuaOptimizadaJob>();

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

builder.Services.AddControllers(options =>
{
    options.Filters.Add<SanitizeInputFilter>();
});
// No permitir campos extra en el body
// .AddJsonOptions(options =>
//{
//    options.JsonSerializerOptions.UnmappedMemberHandling =
//        JsonUnmappedMemberHandling.Disallow;
//});

builder.Services.AddApiValidation();

var app = builder.Build();

// Middleware
app.UseHttpsRedirection();

app.UseCors();

app.UseApiLogging();
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