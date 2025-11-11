using Microsoft.ML;
using Microsoft.ML.Data;
using Serilog;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Diagnostics;

// ============================================================================
// Setup from previous steps
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
// TASK 1: Uncomment service registrations (lines 32-33)
// ============================================================================

// builder.Services.AddSingleton<ObservationStore>();
// builder.Services.AddSingleton<ModelService>();

var app = builder.Build();

// ============================================================================
// TASK 2: Uncomment model initialization (lines 42-43)
// ============================================================================
// Train the initial model on startup
// ============================================================================

// var modelService = app.Services.GetRequiredService<ModelService>();
// modelService.TrainInitialModel();

// var activitySource = new ActivitySource("AlertPredictor");

// ============================================================================
// TASK 3: Uncomment /predict endpoint (lines 53-83)
// ============================================================================
// Now uses ModelService instead of direct prediction engine
// ============================================================================

// app.MapGet("/predict/{value:float}", (float value, ObservationStore store, ModelService modelService) =>
// {
//     using var activity = activitySource.StartActivity("PredictAlert", ActivityKind.Internal);
//     activity?.SetTag("input.value", value);
//     activity?.SetTag("model.version", modelService.ModelVersion);
//
//     var prediction = modelService.Predict(value);
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
//         "Prediction: Value={Value}, Alert={Alert}, Probability={Probability:F2}, ModelVersion={ModelVersion}, ObservationId={ObservationId}",
//         value, prediction.ShouldAlert, prediction.Probability, modelService.ModelVersion, observation.Id);
//
//     return new
//     {
//         observationId = observation.Id,
//         value,
//         shouldAlert = prediction.ShouldAlert,
//         probability = prediction.Probability,
//         modelVersion = modelService.ModelVersion
//     };
// });

// ============================================================================
// /label endpoint - Same as Step 5
// ============================================================================

// app.MapPost("/label/{observationId:guid}", (Guid observationId, bool actualAlert, ObservationStore store) =>
// {
//     var observation = store.GetById(observationId);
//     if (observation == null)
//         return Results.NotFound(new { error = "Observation not found" });
//
//     if (observation.ActualAlert.HasValue)
//         return Results.BadRequest(new { error = "Observation already labeled" });
//
//     observation.ActualAlert = actualAlert;
//
//     Log.Information(
//         "Labeled observation: Id={ObservationId}, Predicted={Predicted}, Actual={Actual}, Correct={Correct}",
//         observationId, observation.PredictedAlert, actualAlert, observation.PredictedAlert == actualAlert);
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
// TASK 4: Uncomment /retrain endpoint (lines 127-163)
// ============================================================================
// This is THE KEY - using labeled data to improve the model!
// ============================================================================

// app.MapPost("/retrain", (ObservationStore store, ModelService modelService) =>
// {
//     using var activity = activitySource.StartActivity("RetrainModel", ActivityKind.Internal);
//     
//     var labeled = store.GetLabeled();
//     
//     if (labeled.Count == 0)
//     {
//         return Results.BadRequest(new { error = "No labeled observations available for retraining" });
//     }
//
//     // Convert observations to training data
//     var trainingData = labeled.Select(o => new SignalData
//     {
//         Value = o.Value,
//         ShouldAlert = o.ActualAlert!.Value  // We know it's not null because we got labeled observations
//     }).ToArray();
//
//     var oldVersion = modelService.ModelVersion;
//     modelService.Retrain(trainingData);
//     var newVersion = modelService.ModelVersion;
//
//     activity?.SetTag("old.version", oldVersion);
//     activity?.SetTag("new.version", newVersion);
//     activity?.SetTag("training.size", trainingData.Length);
//
//     Log.Information(
//         "Model retrained: OldVersion={OldVersion}, NewVersion={NewVersion}, TrainingSize={TrainingSize}",
//         oldVersion, newVersion, trainingData.Length);
//
//     return Results.Ok(new
//     {
//         message = "Model retrained successfully",
//         oldVersion,
//         newVersion,
//         trainingDataSize = trainingData.Length
//     });
// });

// ============================================================================
// TASK 5: Uncomment /stats endpoint (lines 170-203)
// ============================================================================
// Shows model performance metrics
// ============================================================================

// app.MapGet("/stats", (ObservationStore store, ModelService modelService) =>
// {
//     var all = store.GetAll();
//     var labeled = store.GetLabeled();
//
//     var correct = labeled.Count(o => o.PredictedAlert == o.ActualAlert);
//     var incorrect = labeled.Count - correct;
//     var accuracy = labeled.Count > 0 ? (double)correct / labeled.Count : 0.0;
//
//     return new
//     {
//         modelVersion = modelService.ModelVersion,
//         observations = new
//         {
//             total = all.Count,
//             labeled = labeled.Count,
//             unlabeled = all.Count - labeled.Count
//         },
//         performance = new
//         {
//             correct,
//             incorrect,
//             accuracy = $"{accuracy:P1}"
//         },
//         recentObservations = all.OrderByDescending(o => o.Timestamp).Take(5).Select(o => new
//         {
//             o.Id,
//             o.Value,
//             predicted = o.PredictedAlert,
//             actual = o.ActualAlert,
//             o.Probability,
//             o.Timestamp
//         })
//     };
// });

app.Run();
