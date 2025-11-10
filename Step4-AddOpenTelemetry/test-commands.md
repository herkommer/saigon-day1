# Step 4 - Quick Test Commands

## Run the API

```powershell
dotnet run
```

## Test predictions and observe traces

```powershell
# Make a prediction
curl http://localhost:5000/predict/0.5
```

## Expected Console Output

Look for OpenTelemetry traces along with logs:

```
Activity.TraceId:            abc123def456...
Activity.SpanId:             789ghi...
Activity.DisplayName:        PredictAlert
Activity.Kind:               Internal
    input.value: 0.5
    prediction.alert: True
    prediction.probability: 0.58

[10:15:45 INF] Prediction: Value=0.5, Alert=True, Probability=0.58
```

## Test distributed tracing with multiple requests

```powershell
# Make several predictions quickly
for ($i=1; $i -le 5; $i++) {
    curl "http://localhost:5000/predict/0.$i"
    Start-Sleep -Milliseconds 200
}
```

## ✅ Success Criteria

- API runs with OpenTelemetry configured
- Console shows both logs AND traces
- Each prediction creates an Activity (span)
- Tags include input value, prediction, and probability
- Each trace has a unique TraceId

## Key Addition from Step 3

- **Distributed tracing** with OpenTelemetry
- Custom spans with tags
- Request correlation via TraceId
- Foundation for production observability (export to Jaeger/Zipkin later)
