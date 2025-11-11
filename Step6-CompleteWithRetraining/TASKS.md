# Step 6 Tasks - Complete Self-Improving System

## Overview

Complete the MAPE-K loop by adding model retraining. The system can now learn from labeled observations and improve over time.

## Key Concept: The MAPE-K Loop

**Monitor** → **Analyze** → **Plan** → **Execute** → **Knowledge**

1. **Monitor**: Collect predictions (observations)

   - _In our system:_ `/predict` endpoint stores each prediction
   - _Example:_ "Predicted alert=true for value 0.52"

2. **Analyze**: Label them with ground truth

   - _In our system:_ `/label` endpoint compares predicted vs actual
   - _Example:_ "Predicted true, but actual was false - WRONG!"

3. **Plan**: Determine when to retrain

   - _In our system:_ `/stats` shows accuracy, we decide to retrain
   - _Example:_ "Accuracy dropped to 75%, need to retrain"

4. **Execute**: Retrain the model

   - _In our system:_ `/retrain` trains new model from labeled data
   - _Example:_ "Training model v2 with 10 labeled observations"

5. **Knowledge**: Updated model makes better predictions
   - _In our system:_ `ModelService` holds the new active model
   - _Example:_ "Now using model v2 for all predictions"

This is **autonomic computing** - systems that manage themselves!

## Tasks

### ✅ Task 1: Uncomment ModelService class

**File:** `Models.cs` (lines 50-127)

1. Remove `//` from the entire `ModelService` class
2. **Read the code carefully:**
   - `_modelLock` - Why do we need this?
   - `ModelVersion` - Tracks which model is active
   - `TrainInitialModel()` - Seeds with baseline data
   - `Retrain(SignalData[])` - Trains from labeled observations
   - `Predict(float)` - Thread-safe prediction
   - `lock (_modelLock)` - Prevents race conditions

**Discussion:**

- What happens if two requests try to predict while retraining?
- Why increment `ModelVersion` after each training?

### ✅ Task 2: Uncomment basic models

**File:** `Models.cs` (lines 9-43)

1. Remove `//` from SignalData, AlertPrediction, Observation, and ObservationStore

### ✅ Task 3: Uncomment setup and registrations

**File:** `Program.cs`

1. Uncomment Serilog (lines 12-18)
2. Uncomment OpenTelemetry (lines 20-25)
3. Uncomment service registrations (lines 32-33) - **Note the ModelService!**
4. Uncomment model initialization (lines 42-43)
5. Uncomment ActivitySource (line 46)

**Discussion:** Why train the model at startup?

### ✅ Task 4: Uncomment /predict endpoint

**File:** `Program.cs` (lines 53-83)

1. Remove `//` from the endpoint
2. **Notice the changes from Step 5:**
   - Uses `ModelService` instead of direct `predictionEngine`
   - Response includes `modelVersion`
   - Logs include model version
   - Trace tags include model version

**Discussion:** Why expose model version to callers?

### ✅ Task 5: Uncomment /label endpoint

**File:** `Program.cs` (lines 90-112)

1. Remove `//` from the endpoint (same as Step 5)

### ✅ Task 6: Uncomment /retrain endpoint

**File:** `Program.cs` (lines 127-163)

1. Remove `//` from the POST `/retrain` endpoint
2. **This is the CRITICAL piece - study it:**
   - Gets all labeled observations
   - Converts to `SignalData[]` for training
   - Calls `modelService.Retrain()`
   - Logs old and new version
   - Returns training stats

**Discussion:**

- What prevents retraining with no labeled data?
- Could we retrain automatically (e.g., every 100 labels)?
- What if labeled data contradicts initial training data?

### ✅ Task 7: Uncomment /stats endpoint

**File:** `Program.cs` (lines 170-203)

1. Remove `//` from the GET `/stats` endpoint
2. **Notice it calculates:**
   - Current model version
   - Total observations (labeled vs unlabeled)
   - Accuracy (correct vs incorrect predictions)
   - Recent observations

**Discussion:** How would you use these stats in production?

### ✅ Task 8: Run the complete system

```powershell
dotnet run
```

### ✅ Task 9: Execute the full MAPE-K loop

**Step 1: Monitor - Make predictions**

```powershell
Invoke-RestMethod -Uri "http://localhost:5000/predict/0.25"
# Note: modelVersion=1, shouldAlert=false

Invoke-RestMethod -Uri "http://localhost:5000/predict/0.75"
# Note: modelVersion=1, shouldAlert=true

Invoke-RestMethod -Uri "http://localhost:5000/predict/0.48"
# Note: modelVersion=1, observe probability

Invoke-RestMethod -Uri "http://localhost:5000/predict/0.52"
# Note: modelVersion=1, observe probability
```

**Step 2: Analyze - Label with ground truth**

Let's simulate a scenario where the optimal threshold is actually 0.6, not 0.5:

```powershell
# 0.25 - correctly predicted false
$r1 = Invoke-RestMethod -Uri "http://localhost:5000/predict/0.25"
$o1 = $r1.observationId
Invoke-WebRequest -Uri "http://localhost:5000/label/${o1}?actualAlert=false" -Method POST | ConvertFrom-Json

# 0.75 - correctly predicted true
$r2 = Invoke-RestMethod -Uri "http://localhost:5000/predict/0.75"
$o2 = $r2.observationId
Invoke-WebRequest -Uri "http://localhost:5000/label/${o2}?actualAlert=true" -Method POST | ConvertFrom-Json

# 0.48 - model predicted false, but ACTUALLY should be false (correct)
$r3 = Invoke-RestMethod -Uri "http://localhost:5000/predict/0.48"
$o3 = $r3.observationId
Invoke-WebRequest -Uri "http://localhost:5000/label/${o3}?actualAlert=false" -Method POST | ConvertFrom-Json

# 0.52 - model predicted true, but ACTUALLY should be FALSE (incorrect!)
$r4 = Invoke-RestMethod -Uri "http://localhost:5000/predict/0.52"
$o4 = $r4.observationId
Invoke-WebRequest -Uri "http://localhost:5000/label/${o4}?actualAlert=false" -Method POST | ConvertFrom-Json
```

**Step 3: Check stats**

```powershell
Invoke-RestMethod -Uri "http://localhost:5000/stats"
```

Notice:

- `accuracy`: 75% (3/4 correct)
- The model made a mistake on 0.52

**Step 4: Plan & Execute - Retrain the model**

```powershell
Invoke-WebRequest -Uri "http://localhost:5000/retrain" -Method POST | ConvertFrom-Json
```

Observe:

- `oldVersion: 1, newVersion: 2`
- `trainingDataSize: 4`

**Step 5: Knowledge - Test the improved model**

```powershell
# Test the same values again
Invoke-RestMethod -Uri "http://localhost:5000/predict/0.52"
# Note: modelVersion=2, check if shouldAlert changed!

Invoke-RestMethod -Uri "http://localhost:5000/stats"
```

**Discussion:**

- Did the model improve?
- What would happen with more labeled data?
- How often should we retrain in production?

### ✅ Task 10: Advanced scenario - Concept drift

Simulate the pattern changing over time:

```powershell
# Add more labels with new pattern (threshold now 0.6)
$r5 = Invoke-RestMethod -Uri "http://localhost:5000/predict/0.55"
$o5 = $r5.observationId
Invoke-WebRequest -Uri "http://localhost:5000/label/${o5}?actualAlert=false" -Method POST | ConvertFrom-Json

$r6 = Invoke-RestMethod -Uri "http://localhost:5000/predict/0.65"
$o6 = $r6.observationId
Invoke-WebRequest -Uri "http://localhost:5000/label/${o6}?actualAlert=true" -Method POST | ConvertFrom-Json

$r7 = Invoke-RestMethod -Uri "http://localhost:5000/predict/0.58"
$o7 = $r7.observationId
Invoke-WebRequest -Uri "http://localhost:5000/label/${o7}?actualAlert=false" -Method POST | ConvertFrom-Json

$r8 = Invoke-RestMethod -Uri "http://localhost:5000/predict/0.62"
$o8 = $r8.observationId
Invoke-WebRequest -Uri "http://localhost:5000/label/${o8}?actualAlert=true" -Method POST | ConvertFrom-Json

# Retrain with new pattern
Invoke-WebRequest -Uri "http://localhost:5000/retrain" -Method POST | ConvertFrom-Json

# Check stats
Invoke-RestMethod -Uri "http://localhost:5000/stats"

# Test boundary values
Invoke-RestMethod -Uri "http://localhost:5000/predict/0.50"
Invoke-RestMethod -Uri "http://localhost:5000/predict/0.60"
```

**Discussion:**

- Did the model adapt to the new threshold?
- How does accuracy change over time?
- What's the tradeoff between model stability and adaptability?

## Success Criteria ✓

- [ ] `ModelService` correctly manages model versioning
- [ ] Initial model trains on startup (version 1)
- [ ] `/predict` uses the active model and returns version
- [ ] `/label` stores ground truth
- [ ] `/retrain` creates new model version from labeled data
- [ ] `/stats` shows accuracy and recent observations
- [ ] Model version increments after retraining
- [ ] Predictions use the latest model version
- [ ] System handles concurrent requests safely (lock works)
- [ ] You understand the complete MAPE-K loop

## Architecture Diagram

```
┌─────────────┐
│   Monitor   │ ← /predict endpoint stores observations
└──────┬──────┘
       ↓
┌─────────────┐
│   Analyze   │ ← /label endpoint adds ground truth
└──────┬──────┘
       ↓
┌─────────────┐
│    Plan     │ ← /stats shows when to retrain
└──────┬──────┘
       ↓
┌─────────────┐
│   Execute   │ ← /retrain creates new model
└──────┬──────┘
       ↓
┌─────────────┐
│  Knowledge  │ ← ModelService updates active model
└──────┬──────┘
       ↓
    (loop back to Monitor)
```

## Key Takeaways

1. **Self-improvement requires feedback** - Without labels, we can't improve
2. **Thread safety matters** - Model swapping must be atomic
3. **Versioning enables tracking** - Know which model made which prediction
4. **Metrics drive decisions** - Stats tell us when to retrain
5. **This is a simplified example** - Production systems add:
   - Model validation before deployment
   - A/B testing between models
   - Gradual rollout
   - Model performance monitoring
   - Automated retraining triggers

## Real-World Applications

This pattern applies to:

- Fraud detection (label suspicious transactions)
- Recommendation systems (track clicks/purchases)
- Anomaly detection (label true anomalies)
- Predictive maintenance (label actual failures)
- Content moderation (label false positives/negatives)

## Congratulations! 🎉

You've built a complete self-improving system that:

1. Makes predictions using ML
2. Logs all actions with structured logging
3. Traces requests with OpenTelemetry
4. Stores observations for labeling
5. Accepts ground truth feedback
6. Retrains to improve over time
7. Tracks accuracy and performance

This is the foundation of **adaptive, autonomic systems**!
