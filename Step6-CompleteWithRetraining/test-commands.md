# Step 6 - Quick Test Commands

## Run the API

```powershell
dotnet run
```

## Complete self-learning cycle

### 1. Check initial stats

```powershell
curl http://localhost:5000/stats | ConvertFrom-Json
```

Expected: `modelVersion: 1`, `totalPredictions: 0`, `readyForRetraining: false`

### 2. Make predictions and save observation IDs

```powershell
$r1 = curl http://localhost:5000/predict/0.48 | ConvertFrom-Json
$r2 = curl http://localhost:5000/predict/0.52 | ConvertFrom-Json
$r3 = curl http://localhost:5000/predict/0.50 | ConvertFrom-Json

Write-Host "Model version: $($r1.modelVersion)"
```

### 3. Label observations with ground truth

```powershell
# Teaching the model these should alert
Invoke-WebRequest -Method POST -Uri "http://localhost:5000/label/$($r1.observationId)?actualAlert=true"
Invoke-WebRequest -Method POST -Uri "http://localhost:5000/label/$($r2.observationId)?actualAlert=true"
Invoke-WebRequest -Method POST -Uri "http://localhost:5000/label/$($r3.observationId)?actualAlert=true"
```

### 4. Check stats before retraining

```powershell
curl http://localhost:5000/stats | ConvertFrom-Json
```

Expected: `labeledPredictions: 3`, `readyForRetraining: true`

### 5. Retrain the model

```powershell
$retrain = Invoke-WebRequest -Method POST -Uri "http://localhost:5000/retrain" | ConvertFrom-Json
Write-Host "Retrained: v$($retrain.oldVersion) -> v$($retrain.newVersion)"
```

Expected: `oldVersion: 1`, `newVersion: 2`, `trainingSamples: 11` (8 initial + 3 labeled)

### 6. Test the improved model

```powershell
$r4 = curl http://localhost:5000/predict/0.50 | ConvertFrom-Json
Write-Host "Model version now: $($r4.modelVersion)"
Write-Host "Probability: $($r4.probability)"
```

Notice: Model version is now 2, predictions may have changed based on new training data!

### 7. Verify stats after retraining

```powershell
curl http://localhost:5000/stats | ConvertFrom-Json
```

## One-liner complete test

```powershell
# Complete cycle in one script
$r1 = curl http://localhost:5000/predict/0.48 | ConvertFrom-Json
$r2 = curl http://localhost:5000/predict/0.52 | ConvertFrom-Json
$r3 = curl http://localhost:5000/predict/0.50 | ConvertFrom-Json

Invoke-WebRequest -Method POST -Uri "http://localhost:5000/label/$($r1.observationId)?actualAlert=true" | Out-Null
Invoke-WebRequest -Method POST -Uri "http://localhost:5000/label/$($r2.observationId)?actualAlert=true" | Out-Null
Invoke-WebRequest -Method POST -Uri "http://localhost:5000/label/$($r3.observationId)?actualAlert=true" | Out-Null

Invoke-WebRequest -Method POST -Uri "http://localhost:5000/retrain" | Out-Null

curl http://localhost:5000/predict/0.50 | ConvertFrom-Json
curl http://localhost:5000/stats | ConvertFrom-Json
```

## ✅ Success Criteria

- Initial model loads on startup (version 1)
- Predictions return model version
- Labeling works as in Step 5
- Retraining requires minimum 3 labeled observations
- After retraining, model version increments
- New predictions use the retrained model
- Thread-safe model swapping (no errors during concurrent requests)
- Stats endpoint shows accuracy and training readiness

## Key Addition from Step 5

- **ModelService** with thread-safe model management
- **POST /retrain** endpoint using labeled data
- **Model versioning** tracking which model served each prediction
- **Self-improvement cycle** complete: Predict → Label → Retrain → Predict (better!)

## The "Aha!" Moment

After retraining, make the same prediction again and compare probabilities - the model learned from your corrections!
