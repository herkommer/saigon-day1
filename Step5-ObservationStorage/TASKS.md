# Step 5 Tasks - Observation Storage & Labeling

## Overview

Add the ability to label predictions with ground truth. This creates the feedback loop needed for continuous learning.

## Key Concept

**The Feedback Loop:**

1. System makes a prediction
2. We store the prediction
3. Human or system provides the **actual** outcome
4. We compare predicted vs actual
5. Use this data to retrain and improve

This is the heart of a self-improving system!

## Tasks

### ✅ Task 1: Uncomment all setup code

**File:** `Models.cs` - Uncomment all classes (lines 7-54)
**File:** `Program.cs` - Uncomment:

- Serilog setup (lines 11-18)
- OpenTelemetry setup (lines 20-25)
- ObservationStore registration (line 32)
- ActivitySource (line 36)
- ML.NET training (lines 43-73)
- /predict endpoint (lines 80-118)

This gives us the same functionality as Step 4.

### ✅ Task 2: Uncomment /label endpoint

**File:** `Program.cs` (lines 125-152)

1. Remove `//` from the POST `/label/{observationId:guid}` endpoint
2. **Read the code carefully:**
   - Takes observationId and actualAlert
   - Finds the observation
   - Checks if already labeled
   - Updates with ground truth
   - Logs whether we were correct

**Discussion:**

- Why check if already labeled?
- What happens if observation not found?
- Why log whether we were correct?

### ✅ Task 3: Uncomment /observations endpoint

**File:** `Program.cs` (lines 159-171)

1. Remove `//` from the GET `/observations` endpoint
2. Notice it shows total, labeled, and unlabeled counts

### ✅ Task 4: Run and test the feedback loop

```powershell
dotnet run
```

**Scenario: Simulate real-world usage**

```powershell
# 1. Make some predictions and capture observationIds in variables
$r1 = Invoke-RestMethod -Uri "http://localhost:5000/predict/0.3"
$o1 = $r1.observationId

$r2 = Invoke-RestMethod -Uri "http://localhost:5000/predict/0.7"
$o2 = $r2.observationId

$r3 = Invoke-RestMethod -Uri "http://localhost:5000/predict/0.5"
$o3 = $r3.observationId

# 2. Check observations
Invoke-RestMethod -Uri "http://localhost:5000/observations"
# Notice: labeled=0, unlabeled=3

# 3. Label the predictions using the captured observationIds

# For 0.3 - model predicted false, we confirm it's correct
Invoke-WebRequest -Uri "http://localhost:5000/label/$o1?actualAlert=false" -Method POST | ConvertFrom-Json

# For 0.7 - model predicted true, we confirm it's correct
Invoke-WebRequest -Uri "http://localhost:5000/label/$o2?actualAlert=true" -Method POST | ConvertFrom-Json

# For 0.5 - model predicted something, but maybe it was WRONG
# Let's say the actual was false
Invoke-WebRequest -Uri "http://localhost:5000/label/$o3?actualAlert=false" -Method POST | ConvertFrom-Json

# 4. Check observations again
Invoke-RestMethod -Uri "http://localhost:5000/observations"
# Notice: labeled=3, unlabeled=0
```

**Observe the logs:**

- Each label shows: Predicted, Actual, Correct (true/false)
- This tells us our model's accuracy!

### ✅ Task 5: Try error scenarios

```powershell
# Label same observation twice (use $o1 from above)
Invoke-WebRequest -Uri "http://localhost:5000/label/$o1?actualAlert=true" -Method POST
# Should get 400 Bad Request: "Observation already labeled"

# Invalid GUID
Invoke-WebRequest -Uri "http://localhost:5000/label/00000000-0000-0000-0000-000000000000?actualAlert=true" -Method POST
# Should get 404 Not Found: "Observation not found"
```

### ✅ Task 6: Discussion - The Feedback Loop

**Questions to discuss:**

1. **Where does ground truth come from in production?**

   - Human labeling (e.g., "Was this transaction fraudulent?")
   - Delayed feedback (e.g., "Did the server crash in the next hour?")
   - External systems (e.g., "What was the actual sales number?")

2. **What can we do with labeled data?**

   - Measure accuracy
   - Identify patterns in mistakes
   - Retrain the model to improve
   - Detect concept drift (when patterns change)

3. **Why store observations before labeling?**
   - Labels might come hours/days later
   - Need to correlate predictions with outcomes
   - Build training dataset over time

## Success Criteria ✓

- [ ] All setup code is uncommented and working
- [ ] `/predict` stores observations with generated IDs
- [ ] `/label` accepts ground truth and updates observations
- [ ] Cannot label the same observation twice
- [ ] Invalid observation IDs return 404
- [ ] Logs show whether predictions were correct
- [ ] `/observations` shows labeled vs unlabeled counts
- [ ] You understand the feedback loop concept

## Key Takeaway

We now have a **complete feedback loop**:

- Make predictions ✓
- Store them ✓
- Label with ground truth ✓
- Track accuracy ✓

**Next:** Use this labeled data to retrain the model!

## Next Step

Move to `Step6-CompleteWithRetraining` to close the loop with automatic model improvement.
