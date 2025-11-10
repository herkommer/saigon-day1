using Microsoft.ML;
using Serilog;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Diagnostics;
using Microsoft.ML.Data;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

// TODO: Add OpenTelemetry configuration
// builder.Services.AddOpenTelemetry()
//     .ConfigureResource(r => r.AddService("AlertPredictor"))
//     .WithTracing(tracing => tracing...);
builder.Services.AddOpenTelemetry().ConfigureResource(r => r.AddService("AlertPredictor")).WithTracing(tracing => tracing.AddAspNetCoreInstrumentation().AddSource("AlertPredictor").AddConsoleExporter());

builder.Services.AddSingleton<ObservationStore>();

var app = builder.Build();
var mlContext = new MLContext();

var trainingData = new[]
{
    new SignalData{ Value = 0.1f, ShouldAlert = false},
    new SignalData{ Value = 0.15f, ShouldAlert = false},
    new SignalData{ Value = 0.2f, ShouldAlert = false},
    new SignalData{ Value = 0.25f, ShouldAlert = false},
    new SignalData{ Value = 0.3f, ShouldAlert = false},
    new SignalData{ Value = 0.35f, ShouldAlert = false},
    new SignalData{ Value = 0.4f, ShouldAlert = false},
    new SignalData{ Value = 0.45f, ShouldAlert = false},
    new SignalData{ Value = 0.55f, ShouldAlert = true},
    new SignalData{ Value = 0.6f, ShouldAlert = true},
    new SignalData{ Value = 0.65f, ShouldAlert = true},
    new SignalData{ Value = 0.7f, ShouldAlert = true},
    new SignalData{ Value = 0.75f, ShouldAlert = true},
    new SignalData{ Value = 0.8f, ShouldAlert = true},
    new SignalData{ Value = 0.85f, ShouldAlert = true},
    new SignalData{ Value = 0.9f, ShouldAlert = true},
};

var dataView = mlContext.Data.LoadFromEnumerable(trainingData);

var pipeline = mlContext.Transforms.Concatenate("Features", nameof(SignalData.Value))
    .Append(mlContext.BinaryClassification.Trainers.LbfgsLogisticRegression(
        labelColumnName: nameof(SignalData.ShouldAlert)));

var model = pipeline.Fit(dataView);

var predictionEngine = mlContext.Model.CreatePredictionEngine<SignalData, AlertPrediction>(model);

// TODO: Create ActivitySource("AlertPredictor")
// TODO: Train model and create prediction engine

// TODO: Add /predict endpoint with tracing
// using var activity = activitySource.StartActivity("PredictAlert", ActivityKind.Internal);
// activity?.SetTag("input.value", value);
// activity?.SetTag("prediction.alert", prediction.ShouldAlert);


var activitySource = new ActivitySource("AlertPredictor");

app.MapGet("/predict/{value:float}", (float value, ObservationStore store) =>
{
    var activity = activitySource.StartActivity("PredictAlert", ActivityKind.Internal);
    activity?.SetTag("input.value", value);

    var input = new SignalData { Value = value };
    var prediction = predictionEngine.Predict(input);

    // Store observation for later labeling
    var observation = new Observation
    {
        Id = Guid.NewGuid(),
        Value = value,
        PredictedAlert = prediction.ShouldAlert,
        Probability = prediction.Probability,
        Timestamp = DateTime.UtcNow
    };
    store.Add(observation);

    activity?.SetTag("prediction.alert", prediction.ShouldAlert);
    activity?.SetTag("prediction.probability", prediction.Probability);
    activity?.SetTag("observation.id", observation.Id);

    Log.Information(
        "Prediction: Value={Value}, Alert={Alert}, Probability={Probability:F2}, ObservationId={ObservationId}",
        value,
        prediction.ShouldAlert,
        prediction.Probability,
        observation.Id
    );

    return new
    {
        observationId = observation.Id,
        value,
        shouldAlert = prediction.ShouldAlert,
        probability = prediction.Probability
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
}

public class Observation
{
    public Guid Id { get; set; }
    public float Value { get; set; }
    public bool PredictedAlert { get; set; }
    public float Probability { get; set; }
    public DateTime Timestamp { get; set; }
    public bool? ActualAlert { get; set; }  // null until labeled
}

public class ObservationStore
{
    private readonly ConcurrentBag<Observation> _observations = new();

    public void Add(Observation observation)
    {
        _observations.Add(observation);
    }

    public Observation? GetById(Guid id)
    {
        return _observations.FirstOrDefault(o => o.Id == id);
    }

    public List<Observation> GetAll()
    {
        return _observations.ToList();
    }

    public List<Observation> GetLabeled()
    {
        return _observations.Where(o => o.ActualAlert.HasValue).ToList();
    }
}

