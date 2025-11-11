using Microsoft.ML;
using Microsoft.ML.Data;
using Serilog;

// ============================================================================
// TASK 1: Uncomment Serilog configuration (lines 9-11)
// ============================================================================
// This sets up structured logging with a console output format
// ============================================================================

// Log.Logger = new LoggerConfiguration()
//     .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
//     .CreateLogger();

// var builder = WebApplication.CreateBuilder(args);

// ============================================================================
// TASK 2: Uncomment UseSerilog (line 21)
// ============================================================================
// This replaces ASP.NET Core's default logging with Serilog
// ============================================================================

// builder.Host.UseSerilog();

var app = builder.Build();

// ============================================================================
// ML.NET Setup - Same as Step 2, just uncomment
// ============================================================================

// var mlContext = new MLContext();

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

// var dataView = mlContext.Data.LoadFromEnumerable(trainingData);

// var pipeline = mlContext.Transforms.Concatenate("Features", nameof(SignalData.Value))
//     .Append(mlContext.BinaryClassification.Trainers.LbfgsLogisticRegression(
//         labelColumnName: nameof(SignalData.ShouldAlert)));

// var model = pipeline.Fit(dataView);

// var predictionEngine = mlContext.Model.CreatePredictionEngine<SignalData, AlertPrediction>(model);

// ============================================================================
// TASK 3: Uncomment the /predict endpoint with logging (lines 68-87)
// ============================================================================
// Notice the Log.Information call with structured parameters
// ============================================================================

// app.MapGet("/predict/{value:float}", (float value) =>
// {
//     var input = new SignalData { Value = value };
//     var prediction = predictionEngine.Predict(input);

//     // TASK 4: Examine this structured logging call
//     Log.Information(
//         "Prediction: Value={Value}, Alert={Alert}, Probability={Probability:F2}",
//         value,
//         prediction.ShouldAlert,
//         prediction.Probability
//     );

//     return new
//     {
//         value,
//         shouldAlert = prediction.ShouldAlert,
//         probability = prediction.Probability
//     };
// });

app.Run();
