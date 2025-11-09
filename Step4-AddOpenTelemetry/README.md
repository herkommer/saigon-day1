# Step 4 — Add OpenTelemetry

Add distributed tracing to track prediction requests.

## Goal

Configure OpenTelemetry and add custom spans with tags to the `/predict` endpoint.

## Quick Start

```powershell
dotnet run
```

Test:

```powershell
curl http://localhost:5000/predict/0.5
```

Watch console for trace output showing activity spans with tags.

## What to Build

1. Configure OpenTelemetry with Console exporter
2. Create `ActivitySource("AlertPredictor")`
3. Add custom span to `/predict` endpoint
4. Add tags: `input.value`, `prediction.alert`, `prediction.probability`
