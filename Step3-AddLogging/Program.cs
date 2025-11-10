using Microsoft.ML;
using Serilog;
using Microsoft.ML.Data;

// TODO: Configure Serilog with Console output
// Log.Logger = new LoggerConfiguration()...
Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();


var builder = WebApplication.CreateBuilder(args);
// TODO: Add UseSerilog() to builder.Host
builder.Host.UseSerilog();

var app = builder.Build();

// TODO: Train model and create prediction engine (copy from Step2)
var mlContext = new MLContext();

var trainingData = new[]
{
    new SignalData{ Value = 0.1f, ShouldAlert = false},
    new SignalData{ Value = 0.2f, ShouldAlert = false},
    new SignalData{ Value = 0.3f, ShouldAlert = false},
    new SignalData{ Value = 0.4f, ShouldAlert = false},
    new SignalData{ Value = 0.6f, ShouldAlert = true},
    new SignalData{ Value = 0.7f, ShouldAlert = true},
    new SignalData{ Value = 0.8f, ShouldAlert = true},
    new SignalData{ Value = 0.9f, ShouldAlert = true},
};

var dataView = mlContext.Data.LoadFromEnumerable(trainingData);

var pipeline = mlContext.Transforms.Concatenate("Features", nameof(SignalData.Value))
    .Append(mlContext.BinaryClassification.Trainers.LbfgsLogisticRegression(
        labelColumnName: nameof(SignalData.ShouldAlert)));

var model = pipeline.Fit(dataView);

var predictionEngine = mlContext.Model.CreatePredictionEngine<SignalData, AlertPrediction>(model);

// TODO: Add /predict endpoint with structured logging
// Log.Information("Prediction: Value={Value}, Alert={Alert}, Probability={Probability:F2}", ...)
app.MapGet("/predict/{value:float}", (float value) =>
{
    var input = new SignalData { Value = value };
    var prediction = predictionEngine.Predict(input);

    //Add Logging
    Log.Information("Prediction: Value={Value}, Alert={Alert}, Probability={Probability:F2}",
    value,
    prediction.ShouldAlert,
    prediction.Probability
    );

    return new
    {
        value,
        prediction.ShouldAlert,
        prediction.Probability
    };
});

app.Run();
public class SignalData
{
    public float Value { get; set; }
    public bool ShouldAlert { get; set; }
}
public class AlertPrediction
{
    [ColumnName("PredictedLabel")]
    public bool ShouldAlert { get; set; }
    [ColumnName("Probability")]
    public float Probability { get; set; }
};
