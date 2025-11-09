# Step 5 — Observation Storage

Store predictions for later labeling — the foundation of the learning loop.

## Goal

Store every prediction and add an endpoint to label observations with ground truth.

## Quick Start

```powershell
dotnet run
```

Test:

```powershell
# Make prediction and capture observationId
$r = curl http://localhost:5000/predict/0.52 | ConvertFrom-Json
$obsId = $r.observationId

# Label it with ground truth
Invoke-WebRequest -Method POST -Uri "http://localhost:5000/label/$obsId?actualAlert=true"
```

## What to Build

1. **Models.cs**:

   - Add `Observation` class (Id, Timestamp, Value, PredictedAlert, Probability, ActualAlert?)
   - Add `ObservationStore` class (in-memory storage with Add/GetById/GetLabeled methods)

2. **Program.cs**:
   - Register `ObservationStore` as singleton
   - Update `/predict` to store observations and return `observationId`
   - Add `POST /label/{observationId}` endpoint to update `ActualAlert`
