# Step 2 Tasks - Add ML.NET

## Overview

Replace the hardcoded threshold with a machine learning model that learns the decision boundary from training data.

## Key Concept

Instead of hardcoding `value > 0.5`, we'll train a model to learn this pattern from examples. The model can then make predictions with confidence scores.

## Tasks

### ✅ Task 1: Uncomment the SignalData class

**File:** `Models.cs` (lines 8-12)

1. Remove `//` from the `SignalData` class
2. This is our data structure for both training and predictions
3. Notice it has two properties: the input (`Value`) and the label (`ShouldAlert`)

### ✅ Task 2: Uncomment the AlertPrediction class

**File:** `Models.cs` (lines 20-27)

1. Remove `//` from the `AlertPrediction` class
2. This receives the model's output
3. Notice `[ColumnName]` attributes map to ML.NET's internal column names

**Discussion:** Why separate classes for input and output?

### ✅ Task 3: Uncomment the ML.NET training code

**File:** `Program.cs` (lines 12-45)

1. Remove `//` from the entire ML.NET setup block
2. Read through the training data - notice the pattern
3. Observe the pipeline: Feature preparation → Training algorithm

**Discussion:**

- Why do we have more training examples than in Step 1?
- What is `Concatenate("Features", ...)`doing?
- What is `LbfgsLogisticRegression`?

### ✅ Task 4: Uncomment the /predict endpoint

**File:** `Program.cs` (lines 52-66)

1. Remove `//` from the endpoint
2. Notice how we create `SignalData` for input
3. Observe the response includes `probability` - the model's confidence

### ✅ Task 5: Run and test

```powershell
dotnet run
```

Test the ML endpoint:

```powershell
# Low value
curl http://localhost:5000/predict/0.3

# High value
curl http://localhost:5000/predict/0.7

# Edge case - notice the probability!
curl http://localhost:5000/predict/0.5
```

### ✅ Task 6: Compare with Step 1

**Discussion Points:**

1. **What's different from Step 1?**

   - Response includes `probability` (confidence score)
   - Method shows "learned" instead of "hardcoded"
   - The decision boundary was learned, not coded

2. **What's the benefit of probability?**

   - Values near 0.5 show low confidence
   - Values near 0.0 or 1.0 show high confidence
   - We can act differently based on confidence

3. **What's still missing?**
   - No visibility into what the model is doing
   - No way to track if predictions are correct
   - No way to improve the model over time

## Success Criteria ✓

- [ ] Both classes in `Models.cs` are uncommented and build successfully
- [ ] ML.NET training code runs without errors
- [ ] `/predict/0.3` returns `shouldAlert: false` with high confidence
- [ ] `/predict/0.7` returns `shouldAlert: true` with high confidence
- [ ] `/predict/0.5` returns a result with probability near 0.5
- [ ] You understand the difference between hardcoded and learned thresholds

## Next Step

Move to `Step3-AddLogging` to add observability with Serilog.
