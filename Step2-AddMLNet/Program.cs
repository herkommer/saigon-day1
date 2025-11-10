using Microsoft.ML;
using Microsoft.ML.Data;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// TODO: Create MLContext and train model
// TODO: Create prediction engine
// TODO: Add GET /predict/{value:float} endpoint

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

var pipeline = mlContext.Transforms.Concatenate("Features", nameof(SignalData.Value)).Append(mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(labelColumnName: nameof(SignalData.ShouldAlert), featureColumnName: "Features"));

var model = pipeline.Fit(dataView);

var predictionEngine = mlContext.Model.CreatePredictionEngine<SignalData, AlertPrediction>(model);

app.MapGet("/predict/{value:float}", (float value) =>
{
    var input = new SignalData { Value = value };
    var prediction = predictionEngine.Predict(input);

    return new
    {
        value,
        shouldAlert = prediction.ShouldAlert,
        probability = prediction.Probability,
        method = "learned"
    };
});

app.Run();


public class SignalData
{
    [LoadColumn(0)]
    public float Value { get; set; }
    [LoadColumn(1)]
    public bool ShouldAlert { get; set; }
}
public class AlertPrediction
{
    [ColumnName("PredictedLabel")]
    public bool ShouldAlert { get; set; }
    [ColumnName("Probability")]
    public float Probability { get; set; }
};
