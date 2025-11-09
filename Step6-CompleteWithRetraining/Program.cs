using Microsoft.ML;
using Serilog;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Diagnostics;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

builder.Services.AddOpenTelemetry()
    .ConfigureResource(r => r.AddService("AlertPredictor"))
    .WithTracing(tracing => tracing
        .AddAspNetCoreInstrumentation()
        .AddSource("AlertPredictor")
        .AddConsoleExporter());

// TODO: Register ObservationStore and ModelService as singletons
// builder.Services.AddSingleton<ObservationStore>();
// builder.Services.AddSingleton<ModelService>();

var app = builder.Build();

// TODO: Initialize model on startup
// var modelService = app.Services.GetRequiredService<ModelService>();
// modelService.TrainInitialModel();

var activitySource = new ActivitySource("AlertPredictor");

// TODO: Add /predict endpoint using ModelService
// TODO: Add /label endpoint
// TODO: Add POST /retrain endpoint
// TODO: Add /stats endpoint

app.Run();
