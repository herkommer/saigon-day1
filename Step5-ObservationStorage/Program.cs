using Microsoft.ML;
using Serilog;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Diagnostics;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

builder.Services.AddOpenTelemetry()
    .ConfigureResource(r => r.AddService("AlertPredictor"))
    .WithTracing(tracing => tracing
        .AddAspNetCoreInstrumentation()
        .AddSource("AlertPredictor")
        .AddConsoleExporter());

// TODO: Register ObservationStore as singleton
// builder.Services.AddSingleton<ObservationStore>();

var app = builder.Build();

var activitySource = new ActivitySource("AlertPredictor");

// TODO: Train model and create prediction engine

// TODO: Update /predict endpoint to store observations
// Return observationId in response

// TODO: Add POST /label/{observationId:guid} endpoint
// Accept actualAlert parameter
// Update observation with ground truth

app.Run();
