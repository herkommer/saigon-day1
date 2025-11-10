# Step 2 - Quick Test Commands

## Run the API

```powershell
dotnet run
```

## Test the ML.NET endpoint

```powershell
# Low value
curl http://localhost:5000/predict/0.3

# High value
curl http://localhost:5000/predict/0.7

# Edge case - now with confidence!
curl http://localhost:5000/predict/0.5
```

## Expected Results

Each response should include:

- `value`: The input value
- `shouldAlert`: Boolean decision
- `probability`: Confidence score (0.0 to 1.0)
- `method`: "learned"

Example for 0.5:

```json
{
  "value": 0.5,
  "shouldAlert": true,
  "probability": 0.58,
  "method": "learned"
}
```

## ✅ Success Criteria

- API runs without errors
- Predictions return probability scores
- Probability around 0.5 means "uncertain" (edge case)
- Probability > 0.5 means "confident it should alert"
- Model learned threshold from training data, not hardcoded

## Key Difference from Step 1

- **Step 1**: Binary true/false
- **Step 2**: Includes confidence (probability)
