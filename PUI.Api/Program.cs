using Hangfire;
using Hangfire.Redis.StackExchange;
using PUI.Api.Configuration;
using PUI.Api.Filters;
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
builder.Services.AgregarServiciosDeInfraestructure(builder.Configuration);
builder.Services.AgregarServiciosDeIdentity(builder.Configuration);

// Hangfire config
var redisConnection = builder.Configuration.GetConnectionString("RedisConnection");

builder.Services.AddHangfire(config =>
{
    config.UseRedisStorage(redisConnection, new RedisStorageOptions
    {
        Prefix = "hangfire:",
        Db = 0
    });
});

builder.Services.AddHangfireServer(options =>
{
    options.WorkerCount = Environment.ProcessorCount * 2;
});

// JOB
// builder.Services.AddHostedService<BusquedaContinuaJob>();
// builder.Services.AddHostedService<BusquedaContinuaOptimizadaJob>();

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


app.UseHangfireDashboard("/hangfire");


using (var scope = app.Services.CreateScope())
{
    var recurring = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();

    recurring.AddOrUpdate<TestJob>(
        "test-cada-minuto",
        job => job.Ejecutar(),
        "* * * * *",
        new RecurringJobOptions
        {
            TimeZone = TimeZoneInfo.Local
        }
    );
}

app.Run();


public class TestJob
{
    public void Ejecutar()
    {
        Console.WriteLine($"[HANGFIRE] Ejecutando: {DateTime.Now:HH:mm:ss.fff}");
    }
}