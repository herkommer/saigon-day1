# Step 6 — Complete with Retraining

Close the feedback loop — retrain the model from labeled observations.

## Goal

Add retraining capability using labeled observations to create an improved model.

## Quick Start

```powershell
dotnet run
```

Test the complete cycle:

```powershell
# Make predictions
$r1 = curl http://localhost:5000/predict/0.48 | ConvertFrom-Json
$r2 = curl http://localhost:5000/predict/0.52 | ConvertFrom-Json
$r3 = curl http://localhost:5000/predict/0.50 | ConvertFrom-Json

# Label them
Invoke-WebRequest -Method POST -Uri "http://localhost:5000/label/$($r1.observationId)?actualAlert=true"
Invoke-WebRequest -Method POST -Uri "http://localhost:5000/label/$($r2.observationId)?actualAlert=true"
Invoke-WebRequest -Method POST -Uri "http://localhost:5000/label/$($r3.observationId)?actualAlert=true"

# Retrain
Invoke-WebRequest -Method POST -Uri "http://localhost:5000/retrain"

# Test improved model
curl http://localhost:5000/predict/0.50 | ConvertFrom-Json
```

## What to Build

1. **Models.cs**:

   - Add `ModelService` class with thread-safe model management
   - Include `ModelVersion` tracking
   - Implement `TrainInitialModel()`, `Retrain()`, and `Predict()` methods

2. **Program.cs**:
   - Register `ModelService` as singleton
   - Initialize model on startup
   - Add `POST /retrain` endpoint using labeled observations
   - Add `GET /stats` endpoint showing model version and accuracy

Key concepts: Thread-safe model swapping with `lock`, version tracking, using ground truth (ActualAlert) for training.
