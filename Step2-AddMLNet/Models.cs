using Microsoft.ML.Data;

// ============================================================================
// TASK 1: Uncomment the SignalData class below (lines 8-12)
// ============================================================================
// This class represents our training data and prediction input
// ============================================================================

public class SignalData
{
    public float Value { get; set; }
    public bool ShouldAlert { get; set; }
}

// ============================================================================
// TASK 2: Uncomment the AlertPrediction class below (lines 20-27)
// ============================================================================
// This class receives the ML.NET model's predictions
// The [ColumnName] attributes map to ML.NET's output columns
// ============================================================================

public class AlertPrediction
{
    [ColumnName("PredictedLabel")]
    public bool ShouldAlert { get; set; }

    [ColumnName("Probability")]
    public float Probability { get; set; }
}
