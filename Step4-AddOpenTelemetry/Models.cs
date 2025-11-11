using Microsoft.ML.Data;
using System.Collections.Concurrent;

// ============================================================================
// Models from previous steps - Uncomment to activate
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

// ============================================================================
// TASK 5: Uncomment Observation class (lines 28-36)
// ============================================================================
// This stores each prediction for later labeling
// ============================================================================

// public class Observation
// {
//     public Guid Id { get; set; }
//     public float Value { get; set; }
//     public bool PredictedAlert { get; set; }
//     public float Probability { get; set; }
//     public DateTime Timestamp { get; set; }
//     public bool? ActualAlert { get; set; }  // null until labeled
// }

// ============================================================================
// TASK 6: Uncomment ObservationStore class (lines 44-65)
// ============================================================================
// Simple in-memory store for observations (thread-safe)
// ============================================================================

// public class ObservationStore
// {
//     private readonly ConcurrentBag<Observation> _observations = new();
//
//     public void Add(Observation observation)
//     {
//         _observations.Add(observation);
//     }
//
//     public Observation? GetById(Guid id)
//     {
//         return _observations.FirstOrDefault(o => o.Id == id);
//     }
//
//     public List<Observation> GetAll()
//     {
//         return _observations.ToList();
//     }
//
//     public List<Observation> GetLabeled()
//     {
//         return _observations.Where(o => o.ActualAlert.HasValue).ToList();
//     }
// }
