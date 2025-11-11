using Microsoft.ML;
using Microsoft.ML.Data;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// ============================================================================
// TASK 3: Uncomment the ML.NET setup below (lines 12-45)
// ============================================================================
// This section:
// 1. Creates an MLContext (the ML.NET workspace)
// 2. Defines training data (notice the pattern: < 0.5 = false, > 0.5 = true)
// 3. Trains a logistic regression model
// 4. Creates a prediction engine for making predictions
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
// TASK 4: Uncomment the /predict endpoint below (lines 52-66)
// ============================================================================
// This endpoint uses the trained model instead of a hardcoded threshold
// Notice it returns a probability score showing model confidence
// ============================================================================

// app.MapGet("/predict/{value:float}", (float value) =>
// {
//     var input = new SignalData { Value = value };
//     var prediction = predictionEngine.Predict(input);
//
//     return new
//     {
//         value,
//         shouldAlert = prediction.ShouldAlert,
//         probability = prediction.Probability,
//         method = "learned"
//     };
// });

app.Run();
