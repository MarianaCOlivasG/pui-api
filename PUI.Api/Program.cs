using Hangfire;
using Hangfire.MySql;
using PUI.Api.Configuration;
using PUI.Api.Filters;
using PUI.Api.Middlewares;
using PUI.Application;
using PUI.Application.Interfaces.Jobs;
using PUI.Identity;
using PUI.Persistencia;
using System.Diagnostics;
using System.Transactions;


var builder = WebApplication.CreateBuilder(args);

// OpenAPI
builder.Services.AddOpenApi();

// Servicios
builder.Services.AgregarServiciosDeAplicacion();
builder.Services.AgregarServiciosDePersistencia(builder.Configuration);
builder.Services.AgregarServiciosDeInfraestructure(builder.Configuration);
builder.Services.AgregarServiciosDeIdentity(builder.Configuration);

// Hangfire config
var hangfireConnection = builder.Configuration.GetConnectionString("HangfireConnection");

builder.Services.AddHangfire(config =>
{
    config.UseStorage(new MySqlStorage(
        hangfireConnection,
        new MySqlStorageOptions
        {
            TablesPrefix = "hf_",
            PrepareSchemaIfNecessary = true,
            QueuePollInterval = TimeSpan.FromSeconds(10),

            //  CLAVE
            TransactionIsolationLevel = System.Transactions.IsolationLevel.ReadCommitted,

        }));
});

builder.Services.AddHangfireServer(options =>
{
    options.WorkerCount = 10; // baja esto temporalmente
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

//using (var scope = app.Services.CreateScope())
//{
//    var recurring = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
//
//    recurring.AddOrUpdate<IBusquedaSchedulerJob>(
//        "busqueda-continua-pui",
//        job => job.EjecutarBatch(),
//        "*/3 * * * *"
//    );
//}
using (var scope = app.Services.CreateScope())
{
    var recurring = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();

    recurring.AddOrUpdate(
        "test-cada-minuto",
        () => Console.WriteLine($"[HANGFIRE] Ejecutando: {DateTime.Now}"),
        "* * * * *"
    );
}

app.Run();