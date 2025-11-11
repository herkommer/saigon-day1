using Microsoft.ML;
using Microsoft.ML.Data;
using System.Collections.Concurrent;

// ============================================================================
// Basic models from previous steps
// ============================================================================

// public class SignalData
// {
//     public float Value { get; set; }
//     public bool ShouldAlert { get; set; }
// }

// public class AlertPrediction
// {
//     [ColumnName("PredictedLabel")]
//     public bool ShouldAlert { get; set; }
//
//     [ColumnName("Probability")]
//     public float Probability { get; set; }
// }

// public class Observation
// {
//     public Guid Id { get; set; }
//     public float Value { get; set; }
//     public bool PredictedAlert { get; set; }
//     public float Probability { get; set; }
//     public DateTime Timestamp { get; set; }
//     public bool? ActualAlert { get; set; }
// }

// public class ObservationStore
// {
//     private readonly ConcurrentBag<Observation> _observations = new();
//
//     public void Add(Observation observation) => _observations.Add(observation);
//     public Observation? GetById(Guid id) => _observations.FirstOrDefault(o => o.Id == id);
//     public List<Observation> GetAll() => _observations.ToList();
//     public List<Observation> GetLabeled() => _observations.Where(o => o.ActualAlert.HasValue).ToList();
// }

// ============================================================================
// TASK 1: Uncomment ModelService class (lines 50-127)
// ============================================================================
// This class encapsulates ML model training and prediction
// Key features:
// - Thread-safe model swapping (using lock)
// - Versioning (to track which model is active)
// - Retraining from labeled observations
// ============================================================================

// public class ModelService
// {
//     private readonly MLContext _mlContext;
//     private readonly object _modelLock = new();
//     private PredictionEngine<SignalData, AlertPrediction> _predictionEngine;
//     
//     public int ModelVersion { get; private set; } = 0;
//
//     public ModelService()
//     {
//         _mlContext = new MLContext();
//     }
//
//     public void TrainInitialModel()
//     {
//         var trainingData = new[]
//         {
//             new SignalData { Value = 0.1f, ShouldAlert = false },
//             new SignalData { Value = 0.15f, ShouldAlert = false },
//             new SignalData { Value = 0.2f, ShouldAlert = false },
//             new SignalData { Value = 0.25f, ShouldAlert = false },
//             new SignalData { Value = 0.3f, ShouldAlert = false },
//             new SignalData { Value = 0.35f, ShouldAlert = false },
//             new SignalData { Value = 0.4f, ShouldAlert = false },
//             new SignalData { Value = 0.45f, ShouldAlert = false },
//             new SignalData { Value = 0.55f, ShouldAlert = true },
//             new SignalData { Value = 0.6f, ShouldAlert = true },
//             new SignalData { Value = 0.65f, ShouldAlert = true },
//             new SignalData { Value = 0.7f, ShouldAlert = true },
//             new SignalData { Value = 0.75f, ShouldAlert = true },
//             new SignalData { Value = 0.8f, ShouldAlert = true },
//             new SignalData { Value = 0.85f, ShouldAlert = true },
//             new SignalData { Value = 0.9f, ShouldAlert = true },
//         };
//
//         Train(trainingData);
//     }
//
//     public void Retrain(SignalData[] labeledData)
//     {
//         if (labeledData.Length == 0)
//         {
//             throw new InvalidOperationException("Cannot retrain with empty dataset");
//         }
//
//         Train(labeledData);
//     }
//
//     private void Train(SignalData[] data)
//     {
//         var dataView = _mlContext.Data.LoadFromEnumerable(data);
//
//         var pipeline = _mlContext.Transforms.Concatenate("Features", nameof(SignalData.Value))
//             .Append(_mlContext.BinaryClassification.Trainers.LbfgsLogisticRegression(
//                 labelColumnName: nameof(SignalData.ShouldAlert)));
//
//         var model = pipeline.Fit(dataView);
//         var newPredictionEngine = _mlContext.Model.CreatePredictionEngine<SignalData, AlertPrediction>(model);
//
//         // Thread-safe model swap
//         lock (_modelLock)
//         {
//             _predictionEngine = newPredictionEngine;
//             ModelVersion++;
//         }
//     }
//
//     public AlertPrediction Predict(float value)
//     {
//         var input = new SignalData { Value = value };
//         
//         lock (_modelLock)
//         {
//             return _predictionEngine.Predict(input);
//         }
//     }
// }
