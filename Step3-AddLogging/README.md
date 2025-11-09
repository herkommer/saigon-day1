# Step 3 — Add Logging

Add structured logging with Serilog to track predictions.

## Goal

Configure Serilog and log every prediction with structured fields.

## Quick Start

```powershell
dotnet run
```

Test:

```powershell
curl http://localhost:5000/predict/0.5
```

Watch the console for structured log output:

```
[HH:mm:ss INF] Prediction: Value=0.5, Alert=True, Probability=0.73
```

## What to Build

1. Configure Serilog in `Program.cs`
2. Add structured logging to `/predict` endpoint
3. Log: value, shouldAlert, probability with named parameters
