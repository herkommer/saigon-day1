using Microsoft.ML;
using Serilog;

// TODO: Configure Serilog with Console output
// Log.Logger = new LoggerConfiguration()...

var builder = WebApplication.CreateBuilder(args);
// TODO: Add UseSerilog() to builder.Host

var app = builder.Build();

// TODO: Train model and create prediction engine (copy from Step2)

// TODO: Add /predict endpoint with structured logging
// Log.Information("Prediction: Value={Value}, Alert={Alert}, Probability={Probability:F2}", ...)

app.Run();
