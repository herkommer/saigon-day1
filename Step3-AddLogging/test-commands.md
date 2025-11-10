# Step 3 - Quick Test Commands

## Run the API

```powershell
dotnet run
```

## Test predictions and observe logs

```powershell
# Make several predictions
curl http://localhost:5000/predict/0.3
curl http://localhost:5000/predict/0.5
curl http://localhost:5000/predict/0.7
```

## Expected Console Output

Look for structured logs like:

```
[10:15:23 INF] Model trained and ready
[10:15:45 INF] Prediction: Value=0.3, Alert=False, Probability=0.23
[10:15:48 INF] Prediction: Value=0.5, Alert=True, Probability=0.58
[10:15:51 INF] Prediction: Value=0.7, Alert=True, Probability=0.89
```

## ✅ Success Criteria

- API runs with Serilog configured
- Console shows formatted log messages
- Each prediction is logged with structured data
- Logs include timestamp, value, decision, and probability

## Key Addition from Step 2

- **Structured logging** with Serilog
- Every prediction is observable
- Logs are machine-readable (key-value pairs)
