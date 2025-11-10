# Step 5 - Quick Test Commands

## Run the API

```powershell
dotnet run
```

## Complete labeling workflow

### 1. Make a prediction and save the observation ID

```powershell
$response = curl http://localhost:5000/predict/0.52 | ConvertFrom-Json
$obsId = $response.observationId
Write-Host "Observation ID: $obsId"
```

### 2. Label the observation with ground truth

```powershell
# Label as "should alert" (true)
Invoke-WebRequest -Method POST -Uri "http://localhost:5000/label/$obsId?actualAlert=true"
```

### 3. Try to label it again (should fail)

```powershell
# This should return 400 Bad Request
Invoke-WebRequest -Method POST -Uri "http://localhost:5000/label/$obsId?actualAlert=false"
```

Expected error: `"Observation already labeled"`

### 4. View all observations

```powershell
curl http://localhost:5000/observations | ConvertFrom-Json
```

Expected output:

```json
{
  "total": 1,
  "labeled": 1,
  "unlabeled": 0,
  "observations": [...]
}
```

## Full test sequence

```powershell
# Make 3 predictions
$r1 = curl http://localhost:5000/predict/0.48 | ConvertFrom-Json
$r2 = curl http://localhost:5000/predict/0.52 | ConvertFrom-Json
$r3 = curl http://localhost:5000/predict/0.50 | ConvertFrom-Json

# Label them
Invoke-WebRequest -Method POST -Uri "http://localhost:5000/label/$($r1.observationId)?actualAlert=false"
Invoke-WebRequest -Method POST -Uri "http://localhost:5000/label/$($r2.observationId)?actualAlert=true"
Invoke-WebRequest -Method POST -Uri "http://localhost:5000/label/$($r3.observationId)?actualAlert=true"

# Check stats
curl http://localhost:5000/observations
```

## ✅ Success Criteria

- Predictions return `observationId`
- Labeling endpoint accepts observation ID and actual result
- Cannot label the same observation twice
- `/observations` endpoint shows labeled vs unlabeled counts
- Logs show correct/incorrect predictions

## Key Addition from Step 4

- **Observation storage** for predictions
- **Labeling endpoint** to record ground truth
- **Data integrity** (can't relabel)
- Foundation for retraining (labeled data)
