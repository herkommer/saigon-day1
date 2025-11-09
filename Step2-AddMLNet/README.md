# Step 2 — Add ML.NET

Replace the hardcoded threshold with a trained ML.NET model.

## Goal

Train a simple binary classification model and serve predictions via `/predict/{value}` endpoint.

## Quick Start

```powershell
dotnet run
```

Test:

```powershell
curl http://localhost:5000/predict/0.5   # Returns probability score
```

## What to Build

1. **Models.cs**: Create `SignalData` and `AlertPrediction` classes with ML.NET attributes
2. **Program.cs**:
   - Train model with sample data (values < 0.4 = false, > 0.6 = true)
   - Create prediction engine
   - Add `/predict` endpoint returning `{ value, shouldAlert, probability }`
