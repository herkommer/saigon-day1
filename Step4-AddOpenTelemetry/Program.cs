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

// TODO: Add OpenTelemetry configuration
// builder.Services.AddOpenTelemetry()
//     .ConfigureResource(r => r.AddService("AlertPredictor"))
//     .WithTracing(tracing => tracing...);

var app = builder.Build();

// TODO: Create ActivitySource("AlertPredictor")
// TODO: Train model and create prediction engine

// TODO: Add /predict endpoint with tracing
// using var activity = activitySource.StartActivity("PredictAlert", ActivityKind.Internal);
// activity?.SetTag("input.value", value);
// activity?.SetTag("prediction.alert", prediction.ShouldAlert);

app.Run();
