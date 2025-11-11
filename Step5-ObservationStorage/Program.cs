using Microsoft.ML;
using Microsoft.ML.Data;
using Serilog;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Diagnostics;

// ============================================================================
// Setup from previous steps - Uncomment all
// ============================================================================

// Log.Logger = new LoggerConfiguration()
//     .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
//     .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// builder.Host.UseSerilog();

// builder.Services.AddOpenTelemetry()
//     .ConfigureResource(r => r.AddService("AlertPredictor"))
//     .WithTracing(tracing => tracing
//         .AddAspNetCoreInstrumentation()
//         .AddSource("AlertPredictor")
//         .AddConsoleExporter());

// ============================================================================
// TASK 1: Uncomment service registration (line 32)
// ============================================================================

// builder.Services.AddSingleton<ObservationStore>();

var app = builder.Build();

// var activitySource = new ActivitySource("AlertPredictor");

// ============================================================================
// ML.NET setup - Same as previous steps
// ============================================================================

// var mlContext = new MLContext();
//
// var trainingData = new[]
// {
//     new SignalData { Value = 0.1f, ShouldAlert = false },
//     new SignalData { Value = 0.15f, ShouldAlert = false },
//     new SignalData { Value = 0.2f, ShouldAlert = false },
//     new SignalData { Value = 0.25f, ShouldAlert = false },
//     new SignalData { Value = 0.3f, ShouldAlert = false },
//     new SignalData { Value = 0.35f, ShouldAlert = false },
//     new SignalData { Value = 0.4f, ShouldAlert = false },
//     new SignalData { Value = 0.45f, ShouldAlert = false },
//     new SignalData { Value = 0.55f, ShouldAlert = true },
//     new SignalData { Value = 0.6f, ShouldAlert = true },
//     new SignalData { Value = 0.65f, ShouldAlert = true },
//     new SignalData { Value = 0.7f, ShouldAlert = true },
//     new SignalData { Value = 0.75f, ShouldAlert = true },
//     new SignalData { Value = 0.8f, ShouldAlert = true },
//     new SignalData { Value = 0.85f, ShouldAlert = true },
//     new SignalData { Value = 0.9f, ShouldAlert = true },
// };
//
// var dataView = mlContext.Data.LoadFromEnumerable(trainingData);
//
// var pipeline = mlContext.Transforms.Concatenate("Features", nameof(SignalData.Value))
//     .Append(mlContext.BinaryClassification.Trainers.LbfgsLogisticRegression(
//         labelColumnName: nameof(SignalData.ShouldAlert)));
//
// var model = pipeline.Fit(dataView);
//
// var predictionEngine = mlContext.Model.CreatePredictionEngine<SignalData, AlertPrediction>(model);

// ============================================================================
// /predict endpoint - Same as Step 4
// ============================================================================

// app.MapGet("/predict/{value:float}", (float value, ObservationStore store) =>
// {
//     using var activity = activitySource.StartActivity("PredictAlert", ActivityKind.Internal);
//     activity?.SetTag("input.value", value);
//
//     var input = new SignalData { Value = value };
//     var prediction = predictionEngine.Predict(input);
//
//     var observation = new Observation
//     {
//         Id = Guid.NewGuid(),
//         Value = value,
//         PredictedAlert = prediction.ShouldAlert,
//         Probability = prediction.Probability,
//         Timestamp = DateTime.UtcNow
//     };
//     store.Add(observation);
//
//     activity?.SetTag("prediction.alert", prediction.ShouldAlert);
//     activity?.SetTag("prediction.probability", prediction.Probability);
//     activity?.SetTag("observation.id", observation.Id);
//
//     Log.Information(
//         "Prediction: Value={Value}, Alert={Alert}, Probability={Probability:F2}, ObservationId={ObservationId}",
//         value,
//         prediction.ShouldAlert,
//         prediction.Probability,
//         observation.Id
//     );
//
//     return new
//     {
//         observationId = observation.Id,
//         value,
//         shouldAlert = prediction.ShouldAlert,
//         probability = prediction.Probability
//     };
// });

// ============================================================================
// TASK 2: Uncomment /label endpoint (lines 125-152)
// ============================================================================
// This is the KEY to the feedback loop - humans label the predictions!
// ============================================================================

// app.MapPost("/label/{observationId:guid}", (Guid observationId, bool actualAlert, ObservationStore store) =>
// {
//     var observation = store.GetById(observationId);
//     if (observation == null)
//     {
//         return Results.NotFound(new { error = "Observation not found" });
//     }
//
//     if (observation.ActualAlert.HasValue)
//     {
//         return Results.BadRequest(new { error = "Observation already labeled" });
//     }
//
//     observation.ActualAlert = actualAlert;
//
//     Log.Information(
//         "Labeled observation: Id={ObservationId}, Predicted={Predicted}, Actual={Actual}, Correct={Correct}",
//         observationId,
//         observation.PredictedAlert,
//         actualAlert,
//         observation.PredictedAlert == actualAlert
//     );
//
//     return Results.Ok(new
//     {
//         observationId,
//         predicted = observation.PredictedAlert,
//         actual = actualAlert,
//         wasCorrect = observation.PredictedAlert == actualAlert
//     });
// });

// ============================================================================
// TASK 3: Uncomment /observations endpoint (lines 159-171)
// ============================================================================

// app.MapGet("/observations", (ObservationStore store) =>
// {
//     var observations = store.GetAll();
//     var labeled = observations.Where(o => o.ActualAlert.HasValue).ToList();
//
//     return new
//     {
//         total = observations.Count,
//         labeled = labeled.Count,
//         unlabeled = observations.Count - labeled.Count,
//         observations = observations.OrderByDescending(o => o.Timestamp).Take(10)
//     };
// });

app.Run();
